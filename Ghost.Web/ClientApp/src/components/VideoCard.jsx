import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'
import axios from 'axios'
import { Card, CardActionArea, CardHeader, CardMedia, Typography, IconButton, Tooltip, Menu, MenuItem, ListItemIcon, ListItemText, Skeleton, CircularProgress } from '@mui/material'
import MoreVertIcon from '@mui/icons-material/MoreVert'
import SyncAltIcon from '@mui/icons-material/SyncAlt';
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';
import { DeleteConfirmationModal } from './DeleteConfirmationModal.jsx'

const generateThumbnail = async ({ videoId, setVideoThumbnail }) => {
  const videoThumbnail = await axios.put(`/image/video/${videoId}`);
  setVideoThumbnail(videoThumbnail.data);
}

const syncFromNfo = async ({ videoId, setVideo }) => {
  const video = await axios.put(`/media/${videoId}/nfo`)
  setVideo(video.data);
}

const deleteVideo = async ({ videoId, remove }) => {
  await axios.delete(`/media/${videoId}`)
  remove();
}

export const VideoCard = ({ video, remove }) => {
  const [localVideo, setLocalVideo] = useState(video);
  const [anchorEl, setAnchorEl] = useState(null)
  const [videoThumbnail, setVideoThumbnail] = useState();
  const [loadingSyncNfo, setLoadingSyncNfo] = useState(false)
  const [loadingDelete, setLoadingDelete] = useState(false)
  const [deleteModalOpen, setDeletModalOpen] = useState(false);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const handleModalClose = () => {
    if (!loadingDelete) {
      setDeletModalOpen(false)
    }
  }

  const handleSyncFromNfo = async () => {
    if (loadingSyncNfo) return;
    setLoadingSyncNfo(true)
    try {
      await syncFromNfo({ videoId: localVideo._id, setVideo: setLocalVideo });
    } finally {
      setLoadingSyncNfo(false)
      handleMenuClose()
    }
  }

  const handleDeleteMenuClick = () => {
    setDeletModalOpen(true)
    handleMenuClose();
  }

  const handleDelete = async () => {
    if (loadingDelete) return;
    setLoadingDelete(true)
    try {
      await deleteVideo({ videoId: localVideo._id, remove });
    } finally {
      setLoadingDelete(false)
      handleModalClose()
      handleMenuClose()
    }
  }

  useEffect(() => {
    if (localVideo.thumbnail) {
      setVideoThumbnail(video.thumbnail);
    } else {
      generateThumbnail({ videoId: localVideo._id, setVideoThumbnail });
    }
  }, [video]);

  const urlToMedia = `/media/${localVideo._id}/${localVideo.title}`;
  return (<Card sx={{ maxHeight: '400px' }}>
    <CardActionArea LinkComponent={Link} to={urlToMedia}>
      {videoThumbnail && <CardMedia
        component="img"
        image={`${axios.defaults.baseURL}/image/${videoThumbnail.id}`}
        alt={localVideo.title}
      />}
      {!videoThumbnail && <Skeleton variant="rectangle" height="150px" />}
    </CardActionArea>
    <CardHeader
      className="ghost-video-card-header"
      title={<Tooltip title={localVideo.title}><Typography variant="h6" noWrap={true}>
        <Link to={urlToMedia}>{localVideo.title}</Link>
      </Typography></Tooltip>}
      disableTypography={true}
      action={<IconButton
        onClick={handleMenuClick}
        id={`${localVideo._id}-video-card-menu-button`}
        aria-controls={!!anchorEl ? 'video-card-menu' : undefined}
        aria-haspopup={true}
        aria-expanded={!!anchorEl}
      >
        <MoreVertIcon />
      </IconButton>}
    />
    <Menu
      id={`${localVideo._id}-video-card-menu`}
      anchorEl={anchorEl}
      open={!!anchorEl}
      onClose={handleMenuClose}
      MenuListProps={{ 'aria-labelledby': `${localVideo._id}-video-card-menu-button` }}
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
        <ListItemText>Delete</ListItemText>
      </MenuItem>
    </Menu>
    <DeleteConfirmationModal
      text="Hello world"
      open={deleteModalOpen}
      onClose={handleModalClose}
      title={localVideo.title}
      loadingConfirm={loadingDelete}
      onConfirm={handleDelete}
    />
  </Card>)
}

VideoCard.propTypes = {
  video: PropTypes.shape({
    _id: PropTypes.number.isRequired,
    title: PropTypes.string.isRequired,
    thumbnail: PropTypes.shape({
      id: PropTypes.number.isRequired
    })
  }).isRequired,
  remove: PropTypes.func.isRequired
} 