import React from 'react'
import PropTypes from 'prop-types'
import {
  Checkbox,
  FormControlLabel,
  FormGroup,
  Grid,
  InputLabel,
  MenuItem,
  Pagination,
  Select,
  FormControl,
} from '@mui/material'
import { remove } from 'ramda'

import { VideoCard } from './VideoCard.jsx'
import { VideoCardSkeleton } from './VideoCardSkeleton.jsx'
import { Search } from './Search'
import { NothingHere } from './NothingHere.jsx'
import watchStates from '../constants/watch-states.js'

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
        <Grid item xs={12} md={6} lg={5}>
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
        </Grid>
      )}
    </>
  )

  return (
    <>
      <Grid
        container
        alignItems="center"
        direction="row"
        sx={{ my: 1 }}
        spacing={1}
      >
        <Grid item xs={12} md={6} lg={4}>
          <Search
            search={search}
            setSearch={(...args) => {
              setSearch(...args)
              onPageChange(null, 1)
            }}
          />
        </Grid>
        {paginationComponent}
        {sortComponent && (
          <Grid item xs={12} md={6} lg={3}>
            {sortComponent}
          </Grid>
        )}
        <Grid item xs={6} md={3} lg={2}>
          <FormControl size="small">
            <InputLabel id="watch-state">Watched state</InputLabel>
            <Select
              labelId="watch-state"
              id="watch-state-select"
              value={watchState}
              label="Watch state"
              onChange={(e) => setWatchState(e.target.value)}
            >
              <MenuItem value={watchStates.inProgress}>
                {watchStates.inProgress.name}
              </MenuItem>
              <MenuItem value={watchStates.watched}>
                {watchStates.watched.name}
              </MenuItem>
              <MenuItem value={watchStates.unwatched}>
                {watchStates.unwatched.name}
              </MenuItem>
              <MenuItem value={watchStates.all}>
                {watchStates.all.name}
              </MenuItem>
            </Select>
          </FormControl>
        </Grid>
      </Grid>
      <Grid container spacing={2}>
        {loading && (
          <Grid item xs={12} sm={6} md={4} lg={3} xl={2}>
            <VideoCardSkeleton />
          </Grid>
        )}
        {!loading && videos.length === 0 && (
          <Grid item xs={12}>
            <NothingHere>
              Nothing here. Add a library and sync it to have videos appear here
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
      <Grid container alignItems="center" direction="row" sx={{ my: 1 }}>
        <Grid item md={12}>
          {paginationComponent}
        </Grid>
      </Grid>
    </>
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
