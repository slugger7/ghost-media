import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'
import axios from 'axios'
import { Card, CardActionArea, CardHeader, CardMedia, Typography, IconButton, Tooltip, Skeleton } from '@mui/material'
import MoreVertIcon from '@mui/icons-material/MoreVert'
import { VideoMenu } from './VideoMenu.jsx'

const generateThumbnail = async ({ videoId, setVideoThumbnail }) => {
  const videoThumbnail = await axios.put(`/image/video/${videoId}`);
  setVideoThumbnail(videoThumbnail.data);
}

export const VideoCard = ({ video, remove }) => {
  const [localVideo, setLocalVideo] = useState(video);
  const [anchorEl, setAnchorEl] = useState(null)
  const [videoThumbnail, setVideoThumbnail] = useState();

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
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
    <VideoMenu
      videoId={localVideo._id}
      anchorEl={anchorEl}
      handleClose={handleMenuClose}
      removeVideo={remove}
      setVideo={setLocalVideo}
      title={localVideo.title}
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