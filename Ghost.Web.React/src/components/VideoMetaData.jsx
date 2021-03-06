import { Typography, Box } from '@mui/material'
import React from 'react'
import PropTypes from 'prop-types'
import { fileSize, milliseconds } from '../services/formatters'

export const VideoMetaData = ({ video }) => <Box sx={{ py: 1 }}>
  <Typography variant="h5" component="h5">Metadata</Typography>
  <Typography overflow="auto">Path: {video.path}</Typography>
  <Typography>Runtime: {milliseconds(video.runtime)}min</Typography>
  <Typography>Resolution: {video.width}x{video.height}</Typography>
  <Typography>Size: {fileSize(video.size)}</Typography>
  <Typography>Created: {video.created}</Typography>
  <Typography>Added: {video.dateAdded}</Typography>
  <Typography>Meta data updated: {video.lastMetadataUpdate}</Typography>
  <Typography>NFO updated: {video.lastNfoScan}</Typography>
</Box>

VideoMetaData.propTypes = {
  video: PropTypes.shape({
    id: PropTypes.number.isRequired,
    dateAdded: PropTypes.string.isRequired,
    path: PropTypes.string.isRequired,
    size: PropTypes.number.isRequired,
    runtime: PropTypes.number.isRequired,
    width: PropTypes.number.isRequired,
    height: PropTypes.number.isRequired,
    lastNfoScan: PropTypes.string.isRequired,
    lastMetadataUpdate: PropTypes.string.isRequired,
    created: PropTypes.string.isRequired
  })
}