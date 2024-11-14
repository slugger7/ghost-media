import React, { useContext, useState } from 'react'
import PropTypes from 'prop-types'
import {
  Menu,
  MenuItem,
  ListItemIcon,
  CircularProgress,
  ListItemText,
  Divider,
} from '@mui/material'
import { Link, useParams } from 'react-router-dom'
import SyncAltIcon from '@mui/icons-material/SyncAlt'
import SyncIcon from '@mui/icons-material/Sync'
import DeleteForeverIcon from '@mui/icons-material/DeleteForever'
import BurstModeIcon from '@mui/icons-material/BurstMode'
import OfflineShareIcon from '@mui/icons-material/OfflineShare'
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder'
import FavoriteIcon from '@mui/icons-material/Favorite'
import ImageIcon from '@mui/icons-material/Image'
import EditIcon from '@mui/icons-material/Edit';
import CompareIcon from '@mui/icons-material/Compare';
import CheckBoxOutlineBlankIcon from '@mui/icons-material/CheckBoxOutlineBlank';
import CheckBoxIcon from '@mui/icons-material/CheckBox';
import PlaylistAddIcon from '@mui/icons-material/PlaylistAdd';
import PlaylistRemoveIcon from '@mui/icons-material/PlaylistRemove';
import axios from 'axios'
import copy from 'copy-to-clipboard'
import { toggleFavourite } from '../services/video.service'

import { DeleteConfirmationModal } from './DeleteConfirmationModal.jsx'
import SelectedVideoContext from '../context/selectedVideos.context.js'
import { videoMenuItems } from '../constants/video-menu-items'
import { removeFromPlaylist } from '../services/playlists.service.js'

const syncFromNfo = async (id) => (await axios.put(`/media/${id}/nfo`)).data
const updateVideoMetaData = async (id) =>
  (await axios.put(`/media/${id}/metadata`)).data
const generateChapters = async (id) =>
  (await axios.put(`/media/${id}/chapters`)).data
const deleteVideo = async (videoId) => await axios.delete(`/media/${videoId}`)
const chooseThumbnail = async (videoId, progress) => {
  if (progress !== null && !isNaN(progress)) {
    await axios.put(
      `/image/video/${videoId}?timestamp=${Math.floor(
        progress * 1000,
      )}&overwrite=true`,
    )
  }
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
  toggleSelected,
  hideItems = [],
  editing = false,
  selected = false,
  setEditing = () => { }
}) => {
  const { selectedVideos } = useContext(SelectedVideoContext)

  const params = useParams()

  const [loadingSync, setLoadingSync] = useState(false)
  const [loadingSyncNfo, setLoadingSyncNfo] = useState(false)
  const [loadingDelete, setLoadingDelete] = useState(false)
  const [loadingGenerateChapter, setLoadingGenerateChapter] = useState(false)
  const [deleteModalOpen, setDeleteModalOpen] = useState(false)
  const [loadingFavourite, setLoadingFavourite] = useState(false)
  const [loadingChooseThumbnail, setLoadingChooseThumbnail] = useState(false)

  const handleDeleteModalClose = () => {
    if (!loadingDelete) {
      setDeleteModalOpen(false)
    }
  }

  const handleSyncFromNfo = async () => {
    if (loadingSyncNfo) return
    setLoadingSyncNfo(true)
    try {
      setVideo(await syncFromNfo(videoId))
    } finally {
      setLoadingSyncNfo(false)
      handleClose()
    }
  }

  const handleDeleteMenuClick = () => {
    setDeleteModalOpen(true)
    handleClose()
  }

  const handleDelete = async () => {
    if (loadingDelete) return
    setLoadingDelete(true)
    try {
      await deleteVideo(videoId)
      if (removeVideo) {
        removeVideo()
      }
    } finally {
      setLoadingDelete(false)
      handleDeleteModalClose()
      handleClose()
    }
  }

  const handleFavourite = async () => {
    if (loadingFavourite) return
    setLoadingFavourite(true)
    try {
      const favourite = await toggleFavourite(videoId)
      setVideo({ favourite })
    } finally {
      setLoadingFavourite(false)
      handleClose()
    }
  }

  const handleSync = async () => {
    if (loadingSync) return
    setLoadingSync(true)
    try {
      const video = await updateVideoMetaData(videoId)
      setVideo(video)
    } finally {
      setLoadingSync(false)
      handleClose()
    }
  }

  const handleGenerateChapters = async () => {
    if (loadingGenerateChapter) return
    setLoadingGenerateChapter(true)
    try {
      const video = await generateChapters(videoId)
      setVideo({ chapters: video.chapters })
    } finally {
      setLoadingGenerateChapter(false)
      handleClose()
    }
  }

  const handleCopyStreamUrl = async () => {
    copy(source)
    handleClose()
  }

  const handleChooseThumbnail = async () => {
    if (loadingChooseThumbnail) return
    setLoadingChooseThumbnail(true)
    try {
      await chooseThumbnail(videoId, progress)
    } finally {
      setLoadingChooseThumbnail(false)
      handleClose()
    }
  }

  const handleEditMenuClick = () => {
    handleClose()
    setEditing(!editing)
  }

  const handleSelectedClick = () => {
    handleClose()
    toggleSelected()
  }

  const handleRemoveFromPlaylist = async () => {
    try {
      await removeFromPlaylist(params.playlistId, videoId);
      if (removeVideo) {
        removeVideo()
      }
    } finally {
      handleClose()
    }
  }

  return (
    <>
      <Menu
        id={`${videoId}-video-card-menu`}
        anchorEl={anchorEl}
        open={!!anchorEl}
        onClose={handleClose}
        MenuListProps={{
          'aria-labelledby': `${videoId}-video-card-menu-button`,
        }}
      >
        {!hideItems.includes(videoMenuItems.toggleSelected) &&
          <MenuItem onClick={handleSelectedClick}>
            <ListItemIcon>
              {selected && <CheckBoxIcon fontSize='small' />}
              {!selected && <CheckBoxOutlineBlankIcon fontSize="small" />}
            </ListItemIcon>
            <ListItemText>Select</ListItemText>
          </MenuItem>
        }
        {!hideItems.includes(videoMenuItems.favourite) && (
          <MenuItem onClick={handleFavourite}>
            <ListItemIcon>
              {!loadingFavourite &&
                (favourite ? (
                  <FavoriteIcon fontSize="small" />
                ) : (
                  <FavoriteBorderIcon fontSize="small" />
                ))}
              {loadingFavourite && <CircularProgress sx={{ mr: 1 }} />}
            </ListItemIcon>
            <ListItemText>Favourite</ListItemText>
          </MenuItem>
        )}
        {!hideItems.includes(videoMenuItems.addToPlaylist) && (
          <MenuItem component={Link} to={`/add-video-to-playlist/${videoId}`} state={{ title }}>
            <ListItemIcon>
              <PlaylistAddIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText>Add {selectedVideos?.length ? 'multiple ' : ''} to playlist</ListItemText>
          </MenuItem>
        )}
        {!hideItems.includes(videoMenuItems.removeFromPlaylist) && params.playlistId && (
          <MenuItem onClick={handleRemoveFromPlaylist}>
            <ListItemIcon>
              <PlaylistRemoveIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText>Remove from playlist</ListItemText>
          </MenuItem>
        )}

        <Divider />

        {progress !== undefined && !hideItems.includes(videoMenuItems.chooseThumbnail) && (
          <MenuItem onClick={handleChooseThumbnail}>
            <ListItemIcon>
              {!loadingChooseThumbnail && <ImageIcon fontSize="small" />}
              {loadingChooseThumbnail && <CircularProgress sx={{ mr: 1 }} />}
            </ListItemIcon>
            <ListItemText>Set thumbnail</ListItemText>
          </MenuItem>
        )}
        {!hideItems.includes(videoMenuItems.generateChapters) && (
          <MenuItem onClick={handleGenerateChapters}>
            <ListItemIcon>
              {!loadingGenerateChapter && <BurstModeIcon fontSize="small" />}
              {loadingGenerateChapter && <CircularProgress sx={{ mr: 1 }} />}
            </ListItemIcon>
            <ListItemText>Generate Chapters</ListItemText>
          </MenuItem>
        )}
        {!hideItems.includes(videoMenuItems.copyStreamUrl) && (
          <MenuItem onClick={handleCopyStreamUrl}>
            <ListItemIcon>
              <OfflineShareIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText>Copy stream URL</ListItemText>
          </MenuItem>
        )}

        <Divider />

        {!hideItems.includes(videoMenuItems.sync) && (
          <MenuItem onClick={handleSync}>
            <ListItemIcon>
              {!loadingSync && <SyncIcon fontSize="small" />}
              {loadingSync && <CircularProgress sx={{ mr: 1 }} />}
            </ListItemIcon>
            <ListItemText>Sync metadata</ListItemText>
          </MenuItem>
        )}
        {!hideItems.includes(videoMenuItems.syncNfo) && (
          <MenuItem onClick={handleSyncFromNfo} disabled={true}>
            <ListItemIcon>
              {!loadingSyncNfo && <SyncAltIcon fontSize="small" />}
              {loadingSyncNfo && <CircularProgress sx={{ mr: 1 }} />}
            </ListItemIcon>
            <ListItemText>Sync from NFO</ListItemText>
          </MenuItem>
        )}
        {!hideItems.includes(videoMenuItems.convert) && (
          <MenuItem
            component={Link}
            to={`/convert/${videoId}`}>
            <ListItemIcon>
              {!loadingSyncNfo && <CompareIcon fontSize="small" />}
              {loadingSyncNfo && <CircularProgress sx={{ mr: 1 }} />}
            </ListItemIcon>
            <ListItemText>{selectedVideos?.length ? "Bulk convert videos" : "Convert Video"}</ListItemText>
          </MenuItem>
        )}
        {!hideItems.includes(videoMenuItems.edit) && (
          <MenuItem onClick={handleEditMenuClick}>
            <ListItemIcon>
              <EditIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText>{editing ? "Finished Edit" : "Edit"}</ListItemText>
          </MenuItem>
        )}
        {!hideItems.includes(videoMenuItems.delete) && (
          <MenuItem onClick={handleDeleteMenuClick}>
            <ListItemIcon>
              <DeleteForeverIcon fontSize="small" />
            </ListItemIcon>
            <ListItemText>Delete permanently</ListItemText>
          </MenuItem>
        )}
      </Menu>
      <DeleteConfirmationModal
        open={deleteModalOpen}
        onClose={handleDeleteModalClose}
        title={title}
        loadingConfirm={loadingDelete}
        onConfirm={handleDelete}
      />
    </>
  )
}

VideoMenu.propTypes = {
  anchorEl: PropTypes.any,
  handleClose: PropTypes.func.isRequired,
  videoId: PropTypes.number.isRequired,
  removeVideo: PropTypes.func,
  setVideo: PropTypes.func.isRequired,
  title: PropTypes.string.isRequired,
  source: PropTypes.string.isRequired,
  favourite: PropTypes.bool.isRequired,
  progress: PropTypes.number,
  hideItems: PropTypes.arrayOf(PropTypes.string),
  editing: PropTypes.bool,
  setEditing: PropTypes.func,
  toggleSelected: PropTypes.func,
  selected: PropTypes.bool
}
