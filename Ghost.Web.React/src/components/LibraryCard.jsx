import React, { useState } from 'react';
import PropTypes from 'prop-types'
import { Avatar, Card, CardHeader, CircularProgress, IconButton, ListItemIcon, ListItemText, Menu, MenuItem } from '@mui/material'
import VideoLibraryIcon from '@mui/icons-material/VideoLibrary';
import MoreVertIcon from '@mui/icons-material/MoreVert'
import DeleteIcon from '@mui/icons-material/Delete';
import SyncIcon from '@mui/icons-material/Sync';
import SyncAltIcon from '@mui/icons-material/SyncAlt';
import ImageSearchIcon from '@mui/icons-material/ImageSearch';
import BurstModeIcon from '@mui/icons-material/BurstMode';
import axios from 'axios';

export const LibraryCard = ({ library, refresh }) => {
  const [anchorEl, setAnchorEl] = useState(null);
  const [loadingSync, setLoadingSync] = useState(false);
  const [loadingDelete, setLoadingDelete] = useState(false);
  const [loadingSyncNfo, setLoadingSyncNfo] = useState(false);
  const [loadingGenerateAllThumbnails, setLoadingGenerateAllThumbnails] = useState(false);
  const [loadingChapters, setLoadingChapters] = useState(false);

  const handleMenuClick = (event) => {
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
  }

  const sync = async () => {
    if (loadingSync) return;
    setLoadingSync(true);
    try {
      await axios.put(`/library/${library.id}/sync`)
    } finally {
      setLoadingSync(false)
      handleMenuClose()
    }
  }

  const deleteLibrary = async () => {
    if (loadingDelete) return;
    setLoadingDelete(true);
    try {
      await axios.delete(`/library/${library.id}`)
    } finally {
      refresh()
      setLoadingDelete(false);
      handleMenuClose();
    }
  }

  const syncNfo = async () => {
    if (loadingSyncNfo) return;
    setLoadingSyncNfo(true)
    try {
      await axios.put(`/library/${library.id}/sync-nfo`);
    } finally {
      setLoadingSyncNfo(false)
      handleMenuClose()
    }
  }

  const generateAllThumbnails = async () => {
    if (loadingGenerateAllThumbnails) return;
    setLoadingGenerateAllThumbnails(true);
    try {
      await axios.put(`/library/${library.id}/generate-thumbnails`);
    } finally {
      setLoadingGenerateAllThumbnails(false);
      handleMenuClose();
    }
  }

  const generateChapters = async () => {
    if (loadingChapters) return;
    setLoadingChapters(true);
    try {
      await axios.put(`/library/${library.id}/generate-chapters`);
    } finally {
      setLoadingChapters(false);
      handleMenuClose();
    }
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
            id={`${library.id}-menu-button`}
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
      id={`${library.id}-menu`}
      anchorEl={anchorEl}
      open={!!anchorEl}
      onClose={handleMenuClose}
      MenuListProps={{
        'aria-labelledby': `${library.id}-menu-button`
      }}>
      <MenuItem onClick={sync}>
        <ListItemIcon>
          {loadingSync && <CircularProgress sx={{ mr: 1 }} fontSize="small" />}
          {!loadingSync && <SyncIcon fontSize="small" />}
        </ListItemIcon>
        <ListItemText>Sync</ListItemText>
      </MenuItem>
      <MenuItem onClick={syncNfo}>
        <ListItemIcon>
          {loadingSyncNfo && <CircularProgress sx={{ mr: 1 }} fontSize="small" />}
          {!loadingSyncNfo && <SyncAltIcon fontSize="small" />}
        </ListItemIcon>
        <ListItemText>Sync all NFOs</ListItemText>
      </MenuItem>
      <MenuItem onClick={generateAllThumbnails}>
        <ListItemIcon>
          {loadingGenerateAllThumbnails && <CircularProgress sx={{ mr: 1 }} fontSize="small" />}
          {!loadingGenerateAllThumbnails && <ImageSearchIcon fontSize="small" />}
        </ListItemIcon>
        <ListItemText>Generate all thumbnails</ListItemText>
      </MenuItem>
      <MenuItem onClick={generateChapters}>
        <ListItemIcon>
          {loadingChapters && <CircularProgress sx={{ mr: 1 }} fontSize="small" />}
          {!loadingChapters && <BurstModeIcon fontSize="small" />}
        </ListItemIcon>
        <ListItemText>Generate all chapters</ListItemText>
      </MenuItem>
      <MenuItem onClick={deleteLibrary}>
        <ListItemIcon>
          {loadingDelete && <CircularProgress sx={{ mr: 1 }} fontSize="small" />}
          {!loadingDelete && <DeleteIcon fontSize="small" />}
        </ListItemIcon>
        <ListItemText>Delete</ListItemText>
      </MenuItem>
    </Menu>
  </>
}

LibraryCard.propTypes = {
  library: PropTypes.shape({
    id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    paths: PropTypes.arrayOf(PropTypes.shape({
      id: PropTypes.string.isRequired,
      path: PropTypes.string.isRequired
    }))
  }).isRequired,
  refresh: PropTypes.func.isRequired
}
