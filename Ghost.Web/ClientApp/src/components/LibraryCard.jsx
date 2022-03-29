import React, { useState } from 'react';
import PropTypes from 'prop-types'
import { Avatar, Card, CardHeader, IconButton, ListItemIcon, ListItemText, Menu, MenuItem } from '@mui/material'
import VideoLibraryIcon from '@mui/icons-material/VideoLibrary';
import MoreVertIcon from '@mui/icons-material/MoreVert'
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

export const LibraryCard = ({ library }) => {
  const [anchorEl, setAnchorEl] = useState(null);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  return <>
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
            onClick={handleMenuClick}
            aria-controls={!!anchorEl ? 'library-menu' : undefined}
            aria-haspopup={true}
            aria-expanded={!!anchorEl}>
            <MoreVertIcon />
          </IconButton>
        }
        title={library.name}
      />
    </Card>
    <Menu
      id={`${library._id}-menu`}
      anchorEl={anchorEl}
      open={!!anchorEl}
      onClose={handleMenuClose}
      MenuListProps={{
        'aria-labelledby': `${library._id}-menu-button`
      }}>
      <MenuItem onClick={handleMenuClose}>
        <ListItemIcon><EditIcon fontSize="small" /></ListItemIcon>
        <ListItemText>Edit</ListItemText>
      </MenuItem>
      <MenuItem onClick={handleMenuClose}>
        <ListItemIcon><DeleteIcon fontSize="small" /></ListItemIcon>
        <ListItemText>Delete</ListItemText>
      </MenuItem>
    </Menu>
  </>
}

LibraryCard.propTypes = {
  library: PropTypes.shape({
    _id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired
  }).isRequired
}
