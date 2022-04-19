import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Stack, Chip, Typography, Paper, Button, IconButton, Autocomplete, TextField } from '@mui/material'
import { Link } from 'react-router-dom'
import EditIcon from '@mui/icons-material/Edit';

export const Genres = ({ genres }) => {
  const [editing, setEditing] = useState(false);

  const handleCancel = () => { setEditing(false) }
  const handleSubmit = () => { }
  return <Paper sx={{ p: 2 }}>
    <Typography variant="h5" component="h5">Genres {!editing && <IconButton onClick={() => setEditing(true)}><EditIcon /></IconButton>}</Typography>
    <Stack direction="column" spacing={1}>
      <Stack direction="row" spacing={1}>
        {!editing && genres.map(genre => <Chip
          key={genre._id}
          label={genre.name}
          component={Link}
          to={`/genre/${encodeURIComponent(genre.name)}`}
          clickable
        />)}
      </Stack >
      {editing && <>
        <Autocomplete
          multiple
          options={genres}
          getOptionLabel={(option) => option.name}
          defaultValue={genres}
          renderInput={(params) => <TextField {...params}
            variant="standard"
            placeholder='Genres' />}
        />
        <Stack direction="row" spacing={2}>
          <Button onClick={handleSubmit} variant="contained">Done</Button>
          <Button onClick={handleCancel}>Cancel</Button>
        </Stack>
      </>}
    </Stack>
  </Paper>
}

Genres.propTypes = {
  genres: PropTypes.arrayOf(PropTypes.shape({
    _id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired
  })).isRequired
}