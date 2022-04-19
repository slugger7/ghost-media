import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Stack, Chip, Typography, Paper, Button, IconButton, Autocomplete, TextField } from '@mui/material'
import { Link } from 'react-router-dom'
import EditIcon from '@mui/icons-material/Edit';
import { useAsync } from 'react-async-hook';
import { prop } from 'ramda'
import LoadingButton from '@mui/lab/LoadingButton';
import SaveIcon from '@mui/icons-material/Save';

import { fetchGenres } from '../services/genre.service'

export const Genres = ({ genres, videoId, updateGenres }) => {
  const [editing, setEditing] = useState(false);
  const allGenres = useAsync(fetchGenres, []);
  const [selectedGenres, setSelectedGenres] = useState([...genres]);
  const [submitting, setSubmitting] = useState(false)

  const handleCancel = () => { setEditing(false) }
  const handleSubmit = async () => {
    setSubmitting(true)
    updateGenres({ genres: selectedGenres });
    setSubmitting(false)
    setEditing(false)
  }

  return <Paper sx={{ p: 2 }}>
    <Typography variant="h5" component="h5">Genres {!editing && <IconButton onClick={() => setEditing(true)}><EditIcon /></IconButton>}</Typography>
    <Stack direction="column" spacing={1}>
      <Stack direction="row" spacing={1}>
        {!editing && genres.map((genre, index) => <Chip
          variant="outlined"
          color="primary"
          key={index}
          label={genre}
          component={Link}
          to={`/genre/${encodeURIComponent(genre.toLowerCase())}`}
          clickable
        />)}
        {!editing && genres.length === 0 && <Chip variant="outlined" label="None"></Chip>}
      </Stack >
      {editing && <>
        <Autocomplete
          multiple
          freeSolo
          onChange={(e, newGenres) => setSelectedGenres(newGenres)}
          options={allGenres?.result?.map(prop('name')) || []}
          defaultValue={genres}
          loading={allGenres.loading}
          renderInput={(params) => <TextField
            {...params}
          />}
        />
        <Stack direction="row" spacing={2}>
          <LoadingButton
            onClick={handleSubmit}
            variant="contained"
            loading={submitting}
            disabled={submitting}
            loadingPosition="start"
            startIcon={<SaveIcon />}>
            Done
          </LoadingButton>
          <Button onClick={handleCancel}>Cancel</Button>
        </Stack>
      </>}
    </Stack>
  </Paper>
}

Genres.propTypes = {
  genres: PropTypes.arrayOf(PropTypes.string).isRequired,
  videoId: PropTypes.string.isRequired,
  updateGenres: PropTypes.func.isRequired
}