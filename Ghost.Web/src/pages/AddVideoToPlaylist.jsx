import { Grid, Typography, Button } from '@mui/material';
import React, {useState} from 'react'
import { useLocation, useParams } from 'react-router-dom'
import usePromise from '../services/use-promise'
import { addVideosToPlaylist, fetchPlaylists } from '../services/playlists.service'
import { PlaylistItem } from '../components/PlaylistItem';
import { useNavigate } from 'react-router-dom';

export const AddVideoToPlaylist = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const params = useParams();

  const [playlists, , playlistsLoading] = usePromise(() => fetchPlaylists(), [])
  const [loading, setLoading] = useState(false)
  const [selectedPlaylists, setSelectedPlaylists] = useState([])

  const handleSave = async () => {
    setLoading(true)
    try {
      const promises = selectedPlaylists.map(p => addVideosToPlaylist(p, [params.videoId]))

      await Promise.all(promises);

      navigate(-1);
    } finally {
      setLoading(false)
    }
  }

  const toggleSelected = (id) => () => {
    if (selectedPlaylists.includes(id)) {
      setSelectedPlaylists(selectedPlaylists.filter(p => p !== id))
    } else {
      setSelectedPlaylists([...selectedPlaylists, id])
    }
  }

  return (
    <Grid container spacing={1}>
      <Grid item xs={12}>
        <Typography variant="h3">Add {location.state.title} to</Typography>
      </Grid>
      {!playlistsLoading && playlists.map(playlist => (
        <Grid item xs={12} sm={6} md={4} lg={3} key={playlist.id}>
          <PlaylistItem 
            playlist={playlist} 
            onClick={toggleSelected(playlist.id)} 
            selected={selectedPlaylists.includes(playlist.id)}/>
        </Grid>
      ))}
      <Grid item xs={12} sx={{display: 'flex', justifyContent: 'end', gap: 1}}>
        <Button variant="outlined" onClick={() => navigate(-1)}>Cancel</Button>
        <Button variant="contained" onClick={handleSave} disabled={loading}>Add to playlist{selectedPlaylists.length > 1 && 's'}</Button>
      </Grid>
    </Grid>
  )
}