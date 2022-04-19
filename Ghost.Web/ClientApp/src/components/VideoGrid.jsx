import React from 'react'
import PropTypes from 'prop-types'
import { Grid, Pagination } from '@mui/material'

import { VideoCard } from './VideoCard.jsx'
import { VideoCardSkeleton } from './VideoCardSkeleton.jsx'

export const VideoGrid = ({ videosPage, page, count, onPageChange }) => {

  const paginationComponent = <Pagination
    color="primary"
    page={page}
    defaultPage={1}
    count={count}
    showFirstButton showLastButton
    onChange={(e, newPage) => setSearchParams({ page: newPage, limit })}
  />

  return <>
    {paginationComponent}
    <Grid container spacing={2}>
      {videosPage.loading && <Grid item xs={12} sm={6} md={4} lg={3} xl={2}><VideoCardSkeleton /></Grid>}
      {!videosPage.loading && videosPage.result?.content?.length === 0 && <span>nothing here</span>}

      {!videosPage.loading && videosPage.result?.content?.map(video => <Grid key={video._id} item xs={12} sm={6} md={4} lg={3} xl={2}>
        <VideoCard id={video._id} title={video.title} />
      </Grid>)}
    </Grid>
    {paginationComponent}
  </>
}

VideoGrid.propTypes = {
  videosPage: PropTypes.shape({
    loading: PropTypes.bool.isRequired,
    error: PropTypes.object,
    result: PropTypes.object
  }),
  page: PropTypes.number.isRequired,
  count: PropTypes.number.isRequired,
  onPageChange: PropTypes.func.isRequired
}