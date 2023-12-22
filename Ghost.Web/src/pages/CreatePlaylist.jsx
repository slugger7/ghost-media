import React, {useState} from "react";
import { createPlaylist } from "../services/playlists.service";
import { useNavigate } from "react-router";
import { Typography, Grid, TextField, Button } from "@mui/material";

export const CreatePlaylist = () => {
  const [name, setName] = useState('');
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate()

  const handleSubmit = async (e) => {
    setLoading(true)

    try {
      await createPlaylist({ name })

      navigate(-1)
    } finally {
      setLoading(false)
    }
  }
  return <>
    <Grid container spacing={1}>
      <Grid item xs={12}><Typography variant="h3">New Playlist</Typography></Grid>
      <Grid item xs={12}>
        <TextField label="Name" fullWidth value={name} onChange={e => setName(e.target.value)} />
      </Grid>
      <Grid item xs={12} sx={{display: 'flex', justifyContent: 'end', gap: 1}}>
        <Button variant="outlined" onClick={() => navigate(-1)}>Cancel</Button>
        <Button variant="contained" onClick={handleSubmit} disabled={loading}>Create</Button>
      </Grid>
    </Grid>
  </>
}