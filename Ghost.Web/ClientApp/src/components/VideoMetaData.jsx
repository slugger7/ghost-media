import { Paper, Typography, IconButton } from '@mui/material'
import React, { useState } from 'react'
import PropTypes from 'prop-types'
import SyncIcon from '@mui/icons-material/Sync';
import { fileSize, milliseconds } from '../services/formatters'
import axios from 'axios';

const updateVideoMetaData = async (id) => (await axios.put(`/media/${id}/metadata`)).data

export const VideoMetaData = ({ video }) => {
  const [localVideo, setLocalVideo] = useState(video)
  const [syncing, setSyncing] = useState(false);

  const handleSync = async () => {
    if (syncing) return;
    setSyncing(true)
    try {
      const updatedVideo = await updateVideoMetaData(video._id);
      setLocalVideo(updatedVideo);
    } finally {
      setSyncing(false)
    }
  }
  return <Paper sx={{ p: 2 }}>
    <Typography variant="h5" component="h5">Metadata <IconButton disabled={syncing} color="primary" onClick={handleSync}><SyncIcon /></IconButton></Typography>
    <Typography>Path: {localVideo.path}</Typography>
    <Typography>Runtime: {milliseconds(localVideo.runtime)}min</Typography>
    <Typography>Resolution: {localVideo.width}x{localVideo.height}</Typography>
    <Typography>Size: {fileSize(localVideo.size)}</Typography>
    <Typography>Created: {localVideo.created}</Typography>
    <Typography>Added: {localVideo.dateAdded}</Typography>
    <Typography>Meta data updated: {localVideo.lastMetadataUpdate}</Typography>
    <Typography>NFO updated: {localVideo.lastNfoScan}</Typography>
  </Paper>
}

VideoMetaData.propTypes = {
  video: PropTypes.shape({
    _id: PropTypes.number.isRequired,
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