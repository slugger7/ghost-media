import { Paper, Typography } from '@mui/material'
import React from 'react'
import PropTypes from 'prop-types'

export const VideoMetaData = ({ video }) => <Paper sx={{ p: 2 }}>
  <Typography>{video.path}</Typography>
  <Typography>{video.dateAdded}</Typography>
</Paper>

VideoMetaData.propTypes = {
  video: PropTypes.shape({
    dateAdded: PropTypes.string.isRequired,
    path: PropTypes.string.isRequired
  })
}