import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Menu, MenuItem, ListItemIcon, CircularProgress, ListItemText, Divider } from '@mui/material'
import SyncAltIcon from '@mui/icons-material/SyncAlt';
import SyncIcon from '@mui/icons-material/Sync';
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';
import BurstModeIcon from '@mui/icons-material/BurstMode';
import OfflineShareIcon from '@mui/icons-material/OfflineShare';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import FavoriteIcon from '@mui/icons-material/Favorite';
import ImageIcon from '@mui/icons-material/Image';
import axios from 'axios'
import copy from 'copy-to-clipboard';
import { toggleFavourite } from '../services/video.service';

import { DeleteConfirmationModal } from './DeleteConfirmationModal.jsx'

const syncFromNfo = async (id) => (await axios.put(`/media/${id}/nfo`)).data
const updateVideoMetaData = async (id) => (await axios.put(`/media/${id}/metadata`)).data
const generateChapters = async (id) => (await axios.put(`/media/${id}/chapters`)).data
const deleteVideo = async (videoId) => await axios.delete(`/media/${videoId}`)
const chooseThumbnail = async (videoId, progress) => {
  if (progress !== null && !isNaN(progress)) {
    (await axios.put(`/image/video/${videoId}?timestamp=${Math.floor(progress * 1000)}&overwrite=true`))
  }
}

export const items = {
  favourite: "favourite",
  chooseThumbnail: "chooseThumbnail",
  generateChapters: "generateChapters",
  copyStreamUrl: "copyStreamUrl",
  sync: "sync",
  syncNfo: "syncNfo",
  delete: "delete",

}

export const VideoMenu = ({
  anchorEl,
  handleClose,
  videoId,
  favourite,
  title,
  removeVideo,
  setVideo,
  source,
  progress,
  hideItems = [] }) => {
  const [loadingSync, setLoadingSync] = useState(false)
  const [loadingSyncNfo, setLoadingSyncNfo] = useState(false)
  const [loadingDelete, setLoadingDelete] = useState(false)
  const [loadingGeneratChapter, setLoadingGeneratChapter] = useState(false)
  const [deleteModalOpen, setDeletModalOpen] = useState(false);
  const [loadingFavourite, setLoadingFavourite] = useState(false);
  const [loadingChooseThumbnail, setLoadingChooseThumbnail] = useState(false);

  const handleModalClose = () => {
    if (!loadingDelete) {
      setDeletModalOpen(false)
    }
  }

  const handleSyncFromNfo = async () => {
    if (loadingSyncNfo) return;
    setLoadingSyncNfo(true)
    try {
      setVideo(await syncFromNfo(videoId))
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
      await deleteVideo(videoId);
      removeVideo();
    } finally {
      setLoadingDelete(false)
      handleModalClose()
      handleClose()
    }
  }

  const handleFavourite = async () => {
    if (loadingFavourite) return;
    setLoadingFavourite(true);
    try {
      const favourite = await toggleFavourite(videoId)
      setVideo({ favourite });
    } finally {
      setLoadingFavourite(false);
      handleClose()
    }
  }

  const handleSync = async () => {
    if (loadingSync) return;
    setLoadingSync(true)
    try {
      const video = await updateVideoMetaData(videoId)
      setVideo(video);
    } finally {
      setLoadingSync(false);
      handleClose();
    }
  }

  const handleGenerateChapters = async () => {
    if (loadingGeneratChapter) return;
    setLoadingGeneratChapter(true)
    try {
      const video = await generateChapters(videoId)
      setVideo(video)
    } finally {
      setLoadingGeneratChapter(false)
      handleClose();
    }
  }

  const handleCopyStreamUrl = async () => {
    copy(source);
    handleClose();
  }

  const handleChooseThumbnail = async () => {
    if (loadingChooseThumbnail) return;
    setLoadingChooseThumbnail(true);
    try {
      await chooseThumbnail(videoId, progress);
    } finally {
      setLoadingChooseThumbnail(false);
      handleClose();
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

      {!hideItems.includes(items.favourite) && <MenuItem onClick={handleFavourite}>
        <ListItemIcon>
          {!loadingFavourite && (favourite ? <FavoriteIcon fontSize="small" /> : <FavoriteBorderIcon fontSize="small" />)}
          {loadingFavourite && <CircularProgress sx={{ mr: 1 }} />}
        </ListItemIcon>
        <ListItemText>Favourite</ListItemText>
      </MenuItem>}
      {progress !== undefined && !hideItems.includes(items.chooseThumbnail) &&
        <MenuItem onClick={handleChooseThumbnail}>
          <ListItemIcon>
            {!loadingChooseThumbnail && <ImageIcon fontSize="small" />}
            {loadingChooseThumbnail && <CircularProgress sx={{ mr: 1 }} />}
          </ListItemIcon>
          <ListItemText>Set thumbnail</ListItemText>
        </MenuItem>}
      {!hideItems.includes(items.generateChapters) && <MenuItem onClick={handleGenerateChapters}>
        <ListItemIcon>
          {!loadingGeneratChapter && <BurstModeIcon fontSize="small" />}
          {loadingGeneratChapter && <CircularProgress sx={{ mr: 1 }} />}
        </ListItemIcon>
        <ListItemText>Generate Chapters</ListItemText>
      </MenuItem>}
      {!hideItems.includes(items.copyStreamUrl) && <MenuItem onClick={handleCopyStreamUrl}>
        <ListItemIcon>
          <OfflineShareIcon fontSize="small" />
        </ListItemIcon>
        <ListItemText>Copy stream URL</ListItemText>
      </MenuItem>}

      <Divider />

      {!hideItems.includes(items.sync) && <MenuItem onClick={handleSync}>
        <ListItemIcon>
          {!loadingSync && <SyncIcon fontSize="small" />}
          {loadingSync && <CircularProgress sx={{ mr: 1 }} />}
        </ListItemIcon>
        <ListItemText>Sync metadata</ListItemText>
      </MenuItem>}
      {!hideItems.includes(items.syncNfo) && <MenuItem onClick={handleSyncFromNfo}>
        <ListItemIcon>
          {!loadingSyncNfo && <SyncAltIcon fontSize="small" />}
          {loadingSyncNfo && <CircularProgress sx={{ mr: 1 }} />}
        </ListItemIcon>
        <ListItemText>Sync from NFO</ListItemText>
      </MenuItem>}
      {!hideItems.includes(items.delete) && <MenuItem onClick={handleDeleteMenuClick}>
        <ListItemIcon>
          <DeleteForeverIcon fontSize="small" />
        </ListItemIcon>
        <ListItemText>Delete permanently</ListItemText>
      </MenuItem>}
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
  title: PropTypes.string.isRequired,
  source: PropTypes.string.isRequired,
  favourite: PropTypes.bool.isRequired,
  progress: PropTypes.number,
  hideItems: PropTypes.arrayOf(PropTypes.string)
}