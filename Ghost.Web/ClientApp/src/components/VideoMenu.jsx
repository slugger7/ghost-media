import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Menu, MenuItem, ListItemIcon, CircularProgress, ListItemText } from '@mui/material'
import SyncAltIcon from '@mui/icons-material/SyncAlt';
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';
import axios from 'axios'

import { DeleteConfirmationModal } from './DeleteConfirmationModal.jsx'

const syncFromNfo = async ({ videoId, setVideo }) => {
  const video = await axios.put(`/media/${videoId}/nfo`)
  setVideo(video.data);
}

const deleteVideo = async ({ videoId, removeVideo }) => {
  await axios.delete(`/media/${videoId}`)
  removeVideo();
}

export const VideoMenu = ({ anchorEl, handleClose, videoId, title, removeVideo, setVideo }) => {
  const [loadingSyncNfo, setLoadingSyncNfo] = useState(false)
  const [loadingDelete, setLoadingDelete] = useState(false)
  const [deleteModalOpen, setDeletModalOpen] = useState(false);

  const handleModalClose = () => {
    if (!loadingDelete) {
      setDeletModalOpen(false)
    }
  }

  const handleSyncFromNfo = async () => {
    if (loadingSyncNfo) return;
    setLoadingSyncNfo(true)
    try {
      await syncFromNfo({ videoId, setVideo });
    } finally {
      setLoadingSyncNfo(false)
      handleClose()
    }
  }

  const handleDeleteMenuClick = () => {
    setDeletModalOpen(true)
    handleClose();
  }

  const handleDelete = async () => {
    if (loadingDelete) return;
    setLoadingDelete(true)
    try {
      await deleteVideo({ videoId: localVideo._id, removeVideo });
    } finally {
      setLoadingDelete(false)
      handleModalClose()
      handleClose()
    }
  }

  return <>
    <Menu
      id={`${videoId}-video-card-menu`}
      anchorEl={anchorEl}
      open={!!anchorEl}
      onClose={handleClose}
      MenuListProps={{ 'aria-labelledby': `${videoId}-video-card-menu-button` }}
    >
      <MenuItem onClick={handleSyncFromNfo}>
        <ListItemIcon>
          {!loadingSyncNfo && <SyncAltIcon fontSize="small" />}
          {loadingSyncNfo && <CircularProgress sx={{ mr: 1 }} />}
        </ListItemIcon>
        <ListItemText>Sync from NFO</ListItemText>
      </MenuItem>
      <MenuItem onClick={handleDeleteMenuClick}>
        <ListItemIcon>
          <DeleteForeverIcon fontSize="small" />
        </ListItemIcon>
        <ListItemText>Delete permanently</ListItemText>
      </MenuItem>
    </Menu>
    <DeleteConfirmationModal
      open={deleteModalOpen}
      onClose={handleModalClose}
      title={title}
      loadingConfirm={loadingDelete}
      onConfirm={handleDelete}
    />
  </>
}

VideoMenu.propTypes = {
  anchorEl: PropTypes.any,
  handleClose: PropTypes.func.isRequired,
  videoId: PropTypes.number.isRequired,
  removeVideo: PropTypes.func.isRequired,
  setVideo: PropTypes.func.isRequired,
  title: PropTypes.string.isRequired
}