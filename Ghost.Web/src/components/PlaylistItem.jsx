import React, { useState } from "react";
import PropTypes from 'prop-types'
import { Card, CardHeader, IconButton, MenuItem, ListItemText, ListItemIcon, Menu } from "@mui/material";
import MoreVertIcon from '@mui/icons-material/MoreVert'
import DeleteIcon from '@mui/icons-material/Delete';
import { deletePlaylist } from "../services/playlists.service";

export const PlaylistItem = ({ playlist, onDelete }) => {
  const [anchorEl, setAnchorEl] = useState(null)
  const [loadingDelete, setLoadingDelete] = useState(false)

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const handleDeleteClick = async () => {
    setLoadingDelete(true)
    try {
      await deletePlaylist(playlist.id)

      if (onDelete) {
        onDelete(playlist.id)
      }
    } finally {
      setLoadingDelete(false)
      handleMenuClose()
    }
  }

  return <>
    <Card>
      <CardHeader 
        title={playlist.name} 
        action={<IconButton 
          id={`playlist-${playlist.id}`}
          aria-controls={!!anchorEl ? 'playlist-menu' : undefined}
          aria-haspopup={true}
          aria-expanded={!!anchorEl}
          onClick={handleMenuClick}>
            <MoreVertIcon />
          </IconButton>}
      />
    </Card>

    <Menu
      id={`${playlist.id}-playlist-menu`}
      anchorEl={anchorEl}
      open={!!anchorEl}
      onClose={handleMenuClose}
    >
      <MenuItem onClick={handleDeleteClick} disabled={loadingDelete}>
        <ListItemIcon>
          <DeleteIcon />
        </ListItemIcon>
        <ListItemText>Delete</ListItemText>
      </MenuItem>
    </Menu>
  </>
}

PlaylistItem.propTypes = {
  playlist: PropTypes.shape({
    id: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    createdAt: PropTypes.string.isRequired,
    playlistVideos: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.number.isRequired,
    })).isRequired
  }).isRequired,
  onDelete: PropTypes.func
}