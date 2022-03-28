import React, { useState } from 'react';
import { Avatar, Card, CardHeader, IconButton, Menu, MenuItem } from '@mui/material'
import VideoLibraryIcon from '@mui/icons-material/VideoLibrary';
import MoreVertIcon from '@mui/icons-material/MoreVert'
import PropTypes from 'prop-types'

export const LibraryCard = ({ library }) => {
  const [anchorEl, setAnchorEl] = useState(null);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  return <>
    <Menu
      id={`${library._id}-menu`}
      anchorEl={anchorEl}
      open={!!anchorEl}>
      <MenuItem onClick={handleMenuClose}>Profile</MenuItem>
    </Menu>
    <Card>
      <CardHeader
        avatar={
          <Avatar>
            <VideoLibraryIcon />
          </Avatar>
        }
        action={
          <IconButton
            id={`${library._id}-menu-button`}
            onClick={handleMenuClick}>
            <MoreVertIcon />
          </IconButton>
        }
        title={library.name}
      />
    </Card>
  </>
}

LibraryCard.propTypes = {
  library: PropTypes.shape({
    _id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired
  }).isRequired
}
