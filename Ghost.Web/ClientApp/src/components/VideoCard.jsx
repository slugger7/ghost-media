import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Link } from 'react-router-dom'
import axios from 'axios'
import { Card, CardActionArea, CardHeader, CardMedia, Typography, IconButton, Tooltip, Menu, MenuItem, ListItemIcon, ListItemText } from '@mui/material'
import MoreVertIcon from '@mui/icons-material/MoreVert';

export const VideoCard = ({ id, title }) => {
  const [anchorEl, setAnchorEl] = useState(null)

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }
  return (<Card sx={{ maxHeight: '400px' }}>
    <CardActionArea LinkComponent={Link} to={`/media/${id}`}>
      <CardMedia
        component="img"
        image={`${axios.defaults.baseURL}/media/${id}/thumbnail`}
        alt={title}
      />
    </CardActionArea>
    <CardHeader
      className="ghost-video-card-header"
      title={<Tooltip title={title}><Typography variant="h6" component="h6" noWrap={true}>{title}</Typography></Tooltip>}
      disableTypography={true}
      action={<IconButton
        onClick={handleMenuClick}
        id={`${id}-video-card-menu-button`}
        aria-controls={!!anchorEl ? 'video-card-menu' : undefined}
        aria-haspopup={true}
        aria-expanded={!!anchorEl}
      >
        <MoreVertIcon />
      </IconButton>}
    />
    <Menu
      id={`${id}-video-card-menu`}
      anchorEl={anchorEl}
      open={!!anchorEl}
      onClose={handleMenuClose}
      MenuListProps={{ 'aria-labelledby': `${id}-video-card-menu-button` }}
    >
      <MenuItem>
        <ListItemIcon><MoreVertIcon fontSize="small" /></ListItemIcon>
        <ListItemText>This is a test</ListItemText>
      </MenuItem>
    </Menu>
  </Card>)
}

VideoCard.propTypes = {
  id: PropTypes.string.isRequired,
  title: PropTypes.string.isRequired
} 