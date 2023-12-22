import { Box, Button, Grid } from "@mui/material";
import React, {useState} from "react";
import { Search } from "../components/Search";
import usePromise from "../services/use-promise";
import { fetchPlaylists } from "../services/playlists.service";
import { Link } from "react-router-dom";
import { PlaylistItem } from "../components/PlaylistItem";

export const Playlists = () => {
  const [search, setSearch] = useState('');
  const [playlists, , loadingPlaylists, setPlaylist] = usePromise(fetchPlaylists, []);

  const handleDeleteVideo = (id) => {
    setPlaylist(playlists.filter(p => p.id !== id))
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
            <PlaylistItem playlist={playlist} onDelete={handleDeleteVideo}/>
          </Grid>)}
    </Grid>
  </Box>
}