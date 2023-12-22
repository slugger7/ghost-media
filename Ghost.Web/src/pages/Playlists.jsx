import { Box, Button, Grid, IconButton, Menu, MenuItem, ListItemIcon, ListItemText } from "@mui/material";
import React, {useState} from "react";
import { Search } from "../components/Search";
import usePromise from "../services/use-promise";
import { fetchPlaylists } from "../services/playlists.service";
import { Link } from "react-router-dom";
import { PlaylistItem } from "../components/PlaylistItem";
import MoreVertIcon from '@mui/icons-material/MoreVert'
import DeleteIcon from '@mui/icons-material/Delete';
import { deletePlaylist } from "../services/playlists.service";

export const Playlists = () => {
  const [search, setSearch] = useState('');
  const [anchorEl, setAnchorEl] = useState(null)
  const [menuPlaylistId, setMenuPlaylistId] = useState(null)
  const [loadingDelete, setLoadingDelete] = useState(false)

  const [playlists, , loadingPlaylists, setPlaylist] = usePromise(fetchPlaylists, []);

  const handleDeleteVideo = (id) => {
    
  }

  const handleMenuClick = (id) => (event) => {
    setMenuPlaylistId(id)
    setAnchorEl(event.currentTarget)
  }

  const handleMenuClose = () => {
    setAnchorEl(null)
    setMenuPlaylistId(null)
  }

  const handleDeleteClick = async () => {
    setLoadingDelete(true)
    try {
      await deletePlaylist(menuPlaylistId)

      setPlaylist(playlists.filter(p => p.id !== menuPlaylistId))
    } finally {
      setLoadingDelete(false)
      handleMenuClose()
    }
  }

  return <Box>
    <Grid container spacing={2}>
      <Grid item xs={12}>
        <Search 
          search={search}
          setSearch={(...args) => { setSearch(...args, )}} />
        <Button component={Link} to="/new-playlist">New playlist</Button>
      </Grid>
        {!loadingPlaylists && 
          playlists?.map(playlist => <Grid item xs={12} sm={6} md={4} lg={3} key={playlist.id}>
            <PlaylistItem playlist={playlist} action={<IconButton 
              id={`playlist-${playlist.id}`}
              aria-controls={!!anchorEl ? 'playlist-menu' : undefined}
              aria-haspopup={true}
              aria-expanded={!!anchorEl}
              onClick={handleMenuClick(playlist.id)}>
                <MoreVertIcon />
              </IconButton>}/>
          </Grid>)}
    </Grid>
    <Menu
        id={"playlist-menu"}
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
  </Box>
}