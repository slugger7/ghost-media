import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'
import axios from 'axios'
import { Card, CardActionArea, CardHeader, CardMedia, Stack, Typography, IconButton, Tooltip, Skeleton, CardActions, Chip } from '@mui/material'
import MoreVertIcon from '@mui/icons-material/MoreVert'
import { items, VideoMenu } from './VideoMenu.jsx'
import { generateVideoUrl, toggleFavourite } from '../services/video.service.js'
import { mergeDeepLeft } from 'ramda'
import { VideoProgress } from './VideoProgress.jsx'
import { FavouriteIconButton } from './FavouriteIconButton.jsx'

export const VideoCard = ({ video, remove, onClick, overrideLeftAction, selected = false, disableActions = false, disabled = false }) => {
  const [localVideo, setLocalVideo] = useState(video);
  const [anchorEl, setAnchorEl] = useState(null);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const onClickOverride = (e) => {
    if (disabled) {
      e.preventDefault()
    } else {
      if (onClick) {
        e.preventDefault()
        onClick(video)
      }
    }
  }

  const actors = localVideo.actors.length > 0 ? localVideo.actors.map(a => <Chip
    size='small'
    key={a.id}
    label={a.name}
    variant="outlined"
    color={a.favourite ? "success" : "primary"}
    component={Link}
    to={`/actors/${a.id}/${encodeURIComponent(a.name.toLowerCase())}`}
    disabled={disableActions || disabled}
    onClick={onClickOverride}
    clickable />) : <Chip variant="outlined" label="Unknown" size='small' disabled={disableActions || disabled}></Chip>

  const urlToMedia = `/media/${localVideo.id}/${localVideo.title}`;
  return (<Card sx={{
    maxHeight: '400px',
    height: '100%',
    display: 'flex',
    justifyContent: 'space-between',
    flexDirection: 'column'
  }}
    raised={selected}
    disabled={disabled}>
    <CardHeader
      sx={{ cursor: onClick && !disabled ? "pointer" : "auto" }}
      onClick={onClickOverride}
      className="ghost-video-card-header"
      title={<Tooltip title={localVideo.title}><Typography variant="h6" noWrap={true}>
        <Typography
          sx={{ cursor: disabled ? "default" : "pointer" }}
          component={Link}
          to={urlToMedia}
          color="white"
          onClick={onClickOverride}
          disabled={disabled}>
          {localVideo.title}
        </Typography>
      </Typography></Tooltip>}
      subheader={<Stack direction="row" spacing={0.5}>{actors}</Stack >}
      disableTypography={true}
      disabled={disabled}
    />
    <CardActionArea
      LinkComponent={Link}
      to={urlToMedia}
      onClick={onClickOverride}
      disabled={disabled}
    >
      {localVideo.thumbnail && <CardMedia sx={{ height: "200px" }}
        component="img"
        image={`${axios.defaults.baseURL}/image/${localVideo.thumbnail.id}/${localVideo.thumbnail.name}`}
        alt={localVideo.title}
      />}
      {!localVideo.thumbnail && <Skeleton animation={false} variant="rectangle" height="150px" />}
      <VideoProgress duration={localVideo.runtime} current={localVideo.progress} />
    </CardActionArea>
    <CardActions
      sx={{ cursor: onClick && !disabled ? "pointer" : "auto" }}
      onClick={onClickOverride}
      disableSpacing
    >
      {!overrideLeftAction && <FavouriteIconButton
        id={localVideo.id}
        state={localVideo.favourite}
        toggleFn={toggleFavourite}
        update={favourite => setLocalVideo(mergeDeepLeft({ favourite }))}
        disabled={disableActions}
      />}
      {overrideLeftAction}
      <IconButton
        sx={{ marginLeft: "auto" }}
        onClick={handleMenuClick}
        id={`${localVideo.id}-video-card-menu-button`}
        aria-controls={!!anchorEl ? 'video-card-menu' : undefined}
        aria-haspopup={true}
        aria-expanded={!!anchorEl}
        disabled={disableActions}
      >
        <MoreVertIcon />
      </IconButton>
    </CardActions>
    <VideoMenu
      source={generateVideoUrl(localVideo.id)}
      videoId={localVideo.id}
      anchorEl={anchorEl}
      handleClose={handleMenuClose}
      removeVideo={remove || null}
      setVideo={video => setLocalVideo(mergeDeepLeft(video))}
      title={localVideo.title}
      favourite={!!localVideo.favourite}
      progress={localVideo.progress}
      hideItems={[items.favourite, items.chooseThumbnail, items.edit]}
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
  remove: PropTypes.func,
  onClick: PropTypes.func,
  selected: PropTypes.bool,
  disableActions: PropTypes.bool,
  disabled: PropTypes.bool,
  overrideLeftAction: PropTypes.node
} 