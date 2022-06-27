import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'
import axios from 'axios'
import { Card, CardActionArea, CardHeader, CardMedia, Typography, IconButton, Tooltip, Skeleton, CardActions } from '@mui/material'
import MoreVertIcon from '@mui/icons-material/MoreVert'
import { items, VideoMenu } from './VideoMenu.jsx'
import { generateVideoUrl, toggleFavourite } from '../services/video.service.js'
import { mergeDeepLeft } from 'ramda'
import FavoriteIcon from '@mui/icons-material/Favorite';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';

export const VideoCard = ({ video, remove }) => {
  const [localVideo, setLocalVideo] = useState(video);
  const [anchorEl, setAnchorEl] = useState(null);
  const [loadingFavourite, setLoadingFavourite] = useState(false);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const handleFavourite = async () => {
    if (loadingFavourite) return;
    setLoadingFavourite(true);
    try {
      const favourite = await toggleFavourite(localVideo.id)
      setLocalVideo(mergeDeepLeft({ favourite }));
    } finally {
      setLoadingFavourite(false);
    }
  }

  const urlToMedia = `/media/${localVideo.id}/${localVideo.title}`;
  return (<Card sx={{ maxHeight: '400px' }}>
    <CardHeader
      className="ghost-video-card-header"
      title={<Tooltip title={localVideo.title}><Typography variant="h6" noWrap={true}>
        <Typography component={Link} to={urlToMedia} color="white">{localVideo.title}</Typography>
      </Typography></Tooltip>}
      subheader={JSON.stringify(localVideo.actors)}
      disableTypography={true}
    />
    <CardActionArea LinkComponent={Link} to={urlToMedia}>
      {localVideo.thumbnail && <CardMedia
        component="img"
        image={`${axios.defaults.baseURL}/image/${localVideo.thumbnail.id}/${localVideo.thumbnail.name}`}
        alt={localVideo.title}
      />}
      {!localVideo.thumbnail && <Skeleton animation={false} variant="rectangle" height="150px" />}
    </CardActionArea>
    <CardActions disableSpacing>
      <IconButton aria-label="add to favourites" onClick={handleFavourite} disabled={loadingFavourite}>
        {localVideo.favourite ? <FavoriteIcon /> : <FavoriteBorderIcon />}
      </IconButton>
      <IconButton
        sx={{ marginLeft: "auto" }}
        onClick={handleMenuClick}
        id={`${localVideo.id}-video-card-menu-button`}
        aria-controls={!!anchorEl ? 'video-card-menu' : undefined}
        aria-haspopup={true}
        aria-expanded={!!anchorEl}
      >
        <MoreVertIcon />
      </IconButton>
    </CardActions>
    <VideoMenu
      source={generateVideoUrl(localVideo.id)}
      videoId={localVideo.id}
      anchorEl={anchorEl}
      handleClose={handleMenuClose}
      removeVideo={remove}
      setVideo={video => setLocalVideo(mergeDeepLeft(video))}
      title={localVideo.title}
      favourite={!!localVideo.favourite}
      hideItems={[items.favourite]}
    />
  </Card>)
}

VideoCard.propTypes = {
  video: PropTypes.shape({
    id: PropTypes.number.isRequired,
    title: PropTypes.string.isRequired,
    thumbnail: PropTypes.shape({
      id: PropTypes.number.isRequired
    })
  }).isRequired,
  remove: PropTypes.func.isRequired
} 