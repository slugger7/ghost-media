import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Stack, Chip, Typography, Paper, Button, IconButton, Autocomplete, TextField, Box } from '@mui/material'
import { Link } from 'react-router-dom'
import EditIcon from '@mui/icons-material/Edit';
import { useAsync } from 'react-async-hook';
import { prop } from 'ramda'
import LoadingButton from '@mui/lab/LoadingButton';
import SaveIcon from '@mui/icons-material/Save';

import { fetchGenres } from '../services/genre.service'

export const VideoGenres = ({ genres, videoId, updateGenres }) => {
  const [editing, setEditing] = useState(false);
  const allGenres = useAsync(fetchGenres, []);
  const [selectedGenres, setSelectedGenres] = useState([...genres.map(prop("name"))]);
  const [submitting, setSubmitting] = useState(false)

  const handleCancel = () => { setEditing(false) }
  const handleSubmit = async () => {
    setSubmitting(true)
    await updateGenres({ genres: selectedGenres });
    setSubmitting(false)
    setEditing(false)
  }

  return <>
    <Typography variant="h5" component="h5">Genres {!editing && <IconButton color="primary" onClick={() => setEditing(true)}><EditIcon /></IconButton>}</Typography>
    <Stack direction="column" spacing={1}>
      <Box>
        {!editing && genres.map(({ name, videoCount }, index) => <Chip
          sx={{ m: 0.5 }}
          variant="outlined"
          color="primary"
          key={index}
          label={`${name} ${videoCount}`}
          component={Link}
          to={`/genres/${encodeURIComponent(name.toLowerCase())}`}
          clickable
        />)}
        {!editing && genres.length === 0 && <Chip variant="outlined" label="None"></Chip>}
      </Box>
      {editing && <>
        <Autocomplete
          multiple
          freeSolo
          onChange={(e, newGenres) => setSelectedGenres(newGenres)}
          options={allGenres?.result?.map(prop('name')) || []}
          defaultValue={selectedGenres}
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
  </>
}

VideoGenres.propTypes = {
  genres: PropTypes.arrayOf(PropTypes.shape({
    name: PropTypes.string.isRequired,
    videoCount: PropTypes.number.isRequired
  })).isRequired,
  videoId: PropTypes.string.isRequired,
  updateGenres: PropTypes.func.isRequired
}