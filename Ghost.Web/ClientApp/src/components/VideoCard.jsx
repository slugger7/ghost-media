import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'
import axios from 'axios'
import { Card, CardActionArea, CardHeader, CardMedia, Typography, IconButton, Tooltip, Menu, MenuItem, ListItemIcon, ListItemText, Skeleton } from '@mui/material'
import MoreVertIcon from '@mui/icons-material/MoreVert'
import SyncAltIcon from '@mui/icons-material/SyncAlt';

const generateThumbnail = async ({ videoId, setVideoThumbnail }) => {
  const videoThumbnail = await axios.put(`/image/video/${videoId}`);
  setVideoThumbnail(videoThumbnail.data);
}

const syncFromNfo = async ({ videoId, setVideo }) => {
  const video = await axios.put(`/media/${videoId}/nfo`)
  setVideo(video.data);
}

export const VideoCard = ({ video }) => {
  const [localVideo, setLocalVideo] = useState(video);
  const [anchorEl, setAnchorEl] = useState(null)
  const [videoThumbnail, setVideoThumbnail] = useState();

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const handleSyncFromNfo = () => {
    syncFromNfo({ videoId: localVideo._id, setVideo: setLocalVideo });
    handleMenuClose()
  }

  useEffect(() => {
    if (localVideo.thumbnail) {
      setVideoThumbnail(video.thumbnail);
    } else {
      generateThumbnail({ videoId: localVideo._id, setVideoThumbnail });
    }
  }, [video]);

  return (<Card sx={{ maxHeight: '400px' }}>
    <CardActionArea LinkComponent={Link} to={`/media/${localVideo._id}`}>
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
        <Link to={`/media/${video._id}`}>{localVideo.title}</Link>
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
        <ListItemIcon><SyncAltIcon fontSize="small" /></ListItemIcon>
        <ListItemText>Sync from NFO</ListItemText>
      </MenuItem>
    </Menu>
  </Card>)
}

VideoCard.propTypes = {
  video: PropTypes.shape({
    _id: PropTypes.number.isRequired,
    title: PropTypes.string.isRequired,
    thumbnail: PropTypes.shape({
      id: PropTypes.number.isRequired
    })
  }).isRequired
} 