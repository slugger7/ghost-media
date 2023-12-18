import React, { useState, useEffect, useCallback } from 'react'
import PropTypes from 'prop-types'
import { Box, Grid, Pagination, Button } from '@mui/material'
import { remove } from 'ramda'

import usePromise from '../services/use-promise'
import useQueryState from '../services/use-query-state'

import watchStates from '../constants/watch-states'

import { VideoCard } from './VideoCard.jsx'
import { Search } from './Search'
import { NothingHere } from './NothingHere.jsx'
import { WatchState } from './WatchState.jsx'
import { Sort } from './Sort.jsx'
import { GenreFilter } from './GenreFilter.jsx'
import { LimitPicker } from './LimitPicker.jsx'
import { VideoGridSkeleton } from './VideoGridSkeleton.jsx'
import { RandomVideoButton } from './RandomVideoButton'
import useLocalState from '../services/use-local-state'

const removeVideo =
  ({ index, setVideos }) =>
    () =>
      setVideos(remove(index, 1))

export const VideoView = ({ fetchFn, fetchRandomVideoFn, children }) => {
  const [page, setPage] = useQueryState('page', 1)
  const [limit, setLimit] = useLocalState('limit', 48)
  const [search, setSearch] = useQueryState('search', '')
  const [total, setTotal] = useState(0)
  const [sortBy, setSortBy] = useQueryState('sortBy', 'date-added')
  const [sortAscending, setSortAscending] = useQueryState(
    'sortAscending',
    false,
  )
  const [watchState, setWatchState] = useQueryState(
    'watchState',
    watchStates.unwatched.value,
  )
  const [selectedGenres, setSelectedGenres] = useLocalState(
    'genres',
    [],
  )
  const [count, setCount] = useState(0)
  const [videos, setVideos] = useState([])
  const [videosPage, error, loading] = usePromise(
    () =>
      fetchFn({
        page,
        limit,
        search,
        sortBy,
        ascending: sortAscending,
        watchState,
        genres: selectedGenres,
      }),
    [page, limit, search, sortBy, sortAscending, watchState, selectedGenres],
  )
  const [selectedVideos, setSelectedVideos] = useState(null)

  useEffect(() => {
    if (!loading && !error) {
      setTotal(videosPage.total)
    }
  }, [videosPage, error, loading])

  useEffect(() => {
    setVideos(videosPage?.content || [])
  }, [videosPage])

  useEffect(() => {
    setCount(Math.ceil(total / limit) || 1)
  }, [total, limit])

  const handlePageChange = (e, newPage) => {
    setPage(newPage)
    setVideos(null)
  }

  const toggleSelectedVideo = useCallback((id) => () => {
    if (!selectedVideos) {
      setSelectedVideos([id])
      return;
    }
    if (selectedVideos.find(vidId => vidId === id)) {
      setSelectedVideos(selectedVideos.filter(vidId => vidId !== id))
    } else {
      setSelectedVideos([...selectedVideos, id])
    }
  });

  const paginationComponent = (
    <>
      {count > 1 && page && (
        <Box sx={{ mb: 1 }}>
          <Pagination
            size="small"
            color="primary"
            page={page}
            defaultPage={1}
            count={count}
            showFirstButton
            showLastButton
            onChange={handlePageChange}
          />
        </Box>
      )}
    </>
  )

  const paginationAndFiltering = (
    <Box
      sx={{
        my: 1,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        flexWrap: 'wrap',
      }}
    >
      <Search
        search={search}
        setSearch={(...args) => {
          setSearch(...args, { page: 1 })
        }}
      />
      {paginationComponent}
      <LimitPicker limit={limit} setLimit={(...args) => {
        setLimit(...args, { page: 1 })
      }} />
      <Sort
        sortBy={sortBy}
        setSortBy={setSortBy}
        sortDirection={sortAscending}
        setSortDirection={setSortAscending}
      />
      <GenreFilter
        setSelectedGenres={(...args) => {
          setSelectedGenres(...args, { page: 1 })
        }}
        selectedGenres={selectedGenres}
      />
      <WatchState watchState={watchState} setWatchState={(...args) => {
        setWatchState(...args, { page: 1 });
      }} />
      {!selectedVideos && <RandomVideoButton fetchFn={fetchRandomVideoFn} />}
      {selectedVideos && <Button onClick={() => setSelectedVideos(null)}>Clear selected</Button>}
    </Box>
  )

  return (
    <Box>
      {paginationAndFiltering}
      {children}
      <Box>
        <Grid container spacing={2}>
          {loading && <VideoGridSkeleton count={limit} />}
          {!loading && videos && videos.length === 0 && (
            <Grid item xs={12}>
              <NothingHere>
                Nothing here. Add a library and sync it to have videos appear
                here
              </NothingHere>
            </Grid>
          )}

          {!loading &&
            videos &&
            videos.map((video, index) => (
              <Grid key={video.id} item xs={12} sm={6} md={4} lg={3} xl={2}>
                <VideoCard
                  video={video}
                  onClick={selectedVideos ? toggleSelectedVideo(video.id) : null}
                  remove={removeVideo({ index, setVideos })}
                  selected={!!selectedVideos?.find(id => id === video.id)}
                  selection={!!selectedVideos}
                  toggleSelected={toggleSelectedVideo(video.id)}
                  overrideLeftAction={selectedVideos?.length ? <></> : undefined}
                />
              </Grid>
            ))}
        </Grid>
      </Box>
      <Box
        sx={{
          my: 1,
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'center',
          mb: 5,
        }}
      >
        {paginationComponent}
      </Box>
    </Box>
  )
}

VideoView.propTypes = {
  fetchFn: PropTypes.func.isRequired,
  fetchRandomVideoFn: PropTypes.func.isRequired,
  children: PropTypes.node,
}
