import React, { useState } from 'react'
import PropTypes from 'prop-types'
import { Stack, Chip, Typography, Paper, Button, IconButton, Autocomplete, TextField } from '@mui/material'
import { Link } from 'react-router-dom'
import EditIcon from '@mui/icons-material/Edit';
import { useAsync } from 'react-async-hook';
import { prop } from 'ramda'
import LoadingButton from '@mui/lab/LoadingButton';
import SaveIcon from '@mui/icons-material/Save';

import { fetchActors } from '../services/actor.service'

export const VideoActors = ({ actors, videoId, updateActors }) => {
  const [editing, setEditing] = useState(false);
  const allActors = useAsync(fetchActors, []);
  const [selectedActors, setSelectedActors] = useState([...actors]);
  const [submitting, setSubmitting] = useState(false)

  const handleCancel = () => { setEditing(false) }
  const handleSubmit = async () => {
    setSubmitting(true)
    updateActors({ actors: selectedActors });
    setSubmitting(false)
    setEditing(false)
  }

  return <Paper sx={{ p: 2 }}>
    <Typography variant="h5" component="h5">Actors {!editing && <IconButton onClick={() => setEditing(true)}><EditIcon /></IconButton>}</Typography>
    <Stack direction="column" spacing={1}>
      <Stack direction="row" spacing={1}>
        {!editing && actors.map((actor, index) => <Chip
          variant="outlined"
          color="primary"
          key={index}
          label={actor}
          component={Link}
          to={`/actors/${encodeURIComponent(actor.toLowerCase())}`}
          clickable
        />)}
        {!editing && actors.length === 0 && <Chip variant="outlined" label="None"></Chip>}
      </Stack >
      {editing && <>
        <Autocomplete
          multiple
          freeSolo
          onChange={(e, newActors) => setSelectedActors(newActors)}
          options={allActors?.result?.map(prop('name')) || []}
          defaultValue={actors}
          loading={allActors.loading}
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

VideoActors.propTypes = {
  actors: PropTypes.arrayOf(PropTypes.string).isRequired,
  videoId: PropTypes.string.isRequired,
  updateActors: PropTypes.func.isRequired
}