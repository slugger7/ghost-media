import React, { useState } from 'react';
import PropTypes from 'prop-types'
import { Avatar, Card, CardHeader, IconButton, ListItemIcon, ListItemText, Menu, MenuItem } from '@mui/material'
import VideoLibraryIcon from '@mui/icons-material/VideoLibrary';
import MoreVertIcon from '@mui/icons-material/MoreVert'
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SyncIcon from '@mui/icons-material/Sync';
import SyncAltIcon from '@mui/icons-material/SyncAlt';
import axios from 'axios';

export const LibraryCard = ({ library, refresh }) => {
  const [anchorEl, setAnchorEl] = useState(null);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const sync = async () => {
    await axios.put(`/library/${library._id}/sync`)
    handleMenuClose()
  }

  const deleteLibrary = async () => {
    await axios.delete(`/library/${library._id}`)
    refresh()
    handleMenuClose();
  }

  const syncNfo = () => {
    axios.put(`/library/${library._id}/sync-nfo`);
    handleMenuClose();
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
        subheader={`${library.paths.length} path${library.paths.length === 1 ? '' : 's'}`}
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
      <MenuItem onClick={sync}>
        <ListItemIcon><SyncIcon fontSize="small" /></ListItemIcon>
        <ListItemText>Sync</ListItemText>
      </MenuItem>
      <MenuItem onClick={syncNfo}>
        <ListItemIcon><SyncAltIcon fontSize="small" /></ListItemIcon>
        <ListItemText>Sync all NFOs</ListItemText>
      </MenuItem>
      <MenuItem onClick={handleMenuClose}>
        <ListItemIcon><EditIcon fontSize="small" /></ListItemIcon>
        <ListItemText>Edit</ListItemText>
      </MenuItem>
      <MenuItem onClick={deleteLibrary}>
        <ListItemIcon><DeleteIcon fontSize="small" /></ListItemIcon>
        <ListItemText>Delete</ListItemText>
      </MenuItem>
    </Menu>
  </>
}

LibraryCard.propTypes = {
  library: PropTypes.shape({
    _id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    paths: PropTypes.arrayOf(PropTypes.shape({
      _id: PropTypes.string.isRequired,
      path: PropTypes.string.isRequired
    }))
  }).isRequired,
  refresh: PropTypes.func.isRequired
}
