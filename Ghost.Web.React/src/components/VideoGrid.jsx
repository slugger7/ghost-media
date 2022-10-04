import React from 'react'
import PropTypes from 'prop-types'
import { Box, Grid, Pagination } from '@mui/material'
import { remove } from 'ramda'

import { VideoCard } from './VideoCard.jsx'
import { VideoCardSkeleton } from './VideoCardSkeleton.jsx'
import { Search } from './Search'
import { NothingHere } from './NothingHere.jsx'
import { WatchState } from './WatchState.jsx'

const removeVideo =
  ({ index, setVideos }) =>
  () =>
    setVideos(remove(index, 1))

export const VideoGrid = ({
  videos = [],
  loading,
  page,
  count,
  onPageChange,
  setSearch,
  search,
  sortComponent,
  watchState,
  setWatchState,
}) => {
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
            onChange={onPageChange}
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
          onPageChange(null, 1)
        }}
      />
      {paginationComponent}
      {sortComponent}
      <WatchState watchState={watchState} setWatchState={setWatchState} />
    </Box>
  )

  return (
    <Box>
      {paginationAndFiltering}
      <Box>
        <Grid container spacing={2}>
          {loading && (
            <Grid item xs={12} sm={6} md={4} lg={3} xl={2}>
              <VideoCardSkeleton />
            </Grid>
          )}
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
                  remove={removeVideo({ index, setVideos: () => {} })}
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

VideoGrid.propTypes = {
  videos: PropTypes.array,
  loading: PropTypes.bool.isRequired,
  page: PropTypes.number,
  count: PropTypes.number.isRequired,
  onPageChange: PropTypes.func.isRequired,
  search: PropTypes.string.isRequired,
  setSearch: PropTypes.func.isRequired,
  sortComponent: PropTypes.node,
  watchState: PropTypes.object.isRequired,
  setWatchState: PropTypes.func.isRequired,
}
