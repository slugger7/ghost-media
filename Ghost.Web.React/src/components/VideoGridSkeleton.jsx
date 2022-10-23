import React from 'react'
import PropTypes from 'prop-types'
import { Grid } from '@mui/material'

import { VideoCardSkeleton } from './VideoCardSkeleton'

export const VideoGridSkeleton = ({ count = 12 }) => {
  const skeletons = []

  for (let i = 0; i < count; i++) {
    skeletons.push(
      <Grid item xs={12} sm={6} md={4} lg={3} xl={2}>
        <VideoCardSkeleton />
      </Grid>,
    )
  }

  return skeletons
}

VideoGridSkeleton.propTypes = {
  count: PropTypes.number,
}
