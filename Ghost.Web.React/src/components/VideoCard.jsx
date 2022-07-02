import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'
import axios from 'axios'
import { Card, CardActionArea, CardHeader, CardMedia, Stack, Typography, IconButton, Tooltip, Skeleton, CardActions, Chip, LinearProgress } from '@mui/material'
import MoreVertIcon from '@mui/icons-material/MoreVert'
import { items, VideoMenu } from './VideoMenu.jsx'
import { generateVideoUrl, toggleFavourite } from '../services/video.service.js'
import { mergeDeepLeft } from 'ramda'
import { VideoProgress } from './VideoProgress.jsx'
import { FavouriteIconButton } from './FavouriteIconButton.jsx'

export const VideoCard = ({ video, remove }) => {
  const [localVideo, setLocalVideo] = useState(video);
  const [anchorEl, setAnchorEl] = useState(null);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const actors = localVideo.actors.length > 0 ? localVideo.actors.map(a => <Chip
    size='small'
    key={a.id}
    label={a.name}
    variant="outlined"
    color={a.favourite ? "success" : "primary"}
    component={Link}
    to={`/actors/${a.id}/${encodeURIComponent(a.name.toLowerCase())}`}
    clickable />) : <Chip variant="outlined" label="Unknown" size='small'></Chip>

  const urlToMedia = `/media/${localVideo.id}/${localVideo.title}`;
  return (<Card sx={{
    maxHeight: '400px',
    height: '100%',
    display: 'flex',
    justifyContent: 'space-between',
    flexDirection: 'column'
  }}>
    <CardHeader
      className="ghost-video-card-header"
      title={<Tooltip title={localVideo.title}><Typography variant="h6" noWrap={true}>
        <Typography component={Link} to={urlToMedia} color="white">{localVideo.title}</Typography>
      </Typography></Tooltip>}
      subheader={<Stack direction="row" spacing={0.5}>{actors}</Stack >}
      disableTypography={true}
    />
    <CardActionArea
      LinkComponent={Link}
      to={urlToMedia}
    >
      {localVideo.thumbnail && <CardMedia sx={{ height: "200px" }}
        component="img"
        image={`${axios.defaults.baseURL}/image/${localVideo.thumbnail.id}/${localVideo.thumbnail.name}`}
        alt={localVideo.title}
      />}
      {!localVideo.thumbnail && <Skeleton animation={false} variant="rectangle" height="150px" />}
      <VideoProgress duration={localVideo.runtime} current={localVideo.progress} />
    </CardActionArea>
    <CardActions disableSpacing>
      <FavouriteIconButton
        id={localVideo.id}
        state={localVideo.favourite}
        toggleFn={toggleFavourite}
        update={favourite => setLocalVideo(mergeDeepLeft({ favourite }))} />
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
      progress={localVideo.progress}
      hideItems={[items.favourite, items.chooseThumbnail]}
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