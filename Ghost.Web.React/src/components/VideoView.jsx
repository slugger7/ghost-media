import React, { useState, useEffect } from 'react'
import PropTypes from 'prop-types'
import { Box, Grid, Pagination } from '@mui/material'
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
import { useSearchParams } from 'react-router-dom'
import { RandomVideoButton } from './RandomVideoButton'

const removeVideo =
  ({ index, setVideos }) =>
    () =>
      setVideos(remove(index, 1))

export const VideoView = ({ fetchFn, fetchRandomVideoFn, children }) => {
  const [page, setPage] = useQueryState('page', 1)
  const [limit, setLimit] = useQueryState('limit', 48)
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
  const [selectedGenres, setSelectedGenres] = useQueryState(
    'selectedGenres',
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

  const [, setSearchParams] = useSearchParams();

  useEffect(() => {
    setSearchParams({ page, limit, search, sortBy, sortAscending, watchState })
  }, [page, limit, search, sortBy, sortAscending, watchState])

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
            onChange={(e, newPage) => setPage(newPage)}
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
          setSearch(...args)
          setPage(1)
        }}
      />
      {paginationComponent}
      <LimitPicker limit={limit} setLimit={(...args) => {
        setLimit(...args)
        setPage(1)
      }} />
      <Sort
        sortBy={sortBy}
        setSortBy={setSortBy}
        sortDirection={sortAscending}
        setSortDirection={setSortAscending}
      />
      <GenreFilter
        setSelectedGenres={(...args) => {
          setSelectedGenres(...args)
          setPage(1)
        }}
        selectedGenres={selectedGenres}
      />
      <WatchState watchState={watchState} setWatchState={(...args) => {
        setWatchState(...args)
        setPage(1)
      }} />
      <RandomVideoButton fetchFn={fetchRandomVideoFn} />
    </Box>
  )

  return (
    <Box>
      {paginationAndFiltering}
      {children}
      <Box>
        <Grid container spacing={2}>
          {loading && <VideoGridSkeleton count={limit} />}
          {!loading && videos.length === 0 && (
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
                  remove={removeVideo({ index, setVideos: () => { } })}
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
