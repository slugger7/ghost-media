import React, { useState, useRef, useEffect } from 'react'
import PropTypes from 'prop-types'
import { Stack, Chip, Typography, Button, IconButton, Autocomplete, TextField, Box } from '@mui/material'
import { Link } from 'react-router-dom'
import EditIcon from '@mui/icons-material/Edit';
import { prop } from 'ramda'
import LoadingButton from '@mui/lab/LoadingButton';
import SaveIcon from '@mui/icons-material/Save';

import { fetchActors } from '../services/actor.service'
import usePromise from '../services/use-promise';

export const VideoActors = ({ actors, videoId, updateActors }) => {
  const [editing, setEditing] = useState(false);
  const allActors = usePromise(() => fetchActors());
  const [selectedActors, setSelectedActors] = useState([...actors]);
  const [submitting, setSubmitting] = useState(false)
  const autocompleteRef = useRef()

  useEffect(() => {
    if (editing) {
      autocompleteRef.current.focus();
    }
  }, [editing])

  const handleCancel = () => { setEditing(false) }
  const handleSubmit = async () => {
    setSubmitting(true)
    updateActors({ actors: selectedActors });
    setSubmitting(false)
    setEditing(false)
  }

  return <>
    <Typography variant="h5" component="h5">Actors {!editing && <IconButton color="primary" onClick={() => setEditing(true)}><EditIcon /></IconButton>}</Typography>
    <Stack direction="column" spacing={1}>
      <Box>
        {!editing && actors.map(({ name, videoCount, id, favourite }, index) => <Chip
          sx={{ m: 0.5 }}
          variant="outlined"
          color={favourite ? "success" : "primary"}
          key={index}
          label={`${name} ${videoCount}`}
          component={Link}
          to={`/actors/${id}/${encodeURIComponent(name.toLowerCase())}`}
          clickable
        />)}
        {!editing && actors.length === 0 && <Chip variant="outlined" label="Unknown"></Chip>}
      </Box>
      {editing && <>
        <Autocomplete
          multiple
          freeSolo
          onChange={(e, newActors) => setSelectedActors(newActors)}
          options={allActors?.result?.map(prop('name')) || []}
          defaultValue={actors.map(prop('name'))}
          loading={allActors.loading}
          renderInput={(params) => <TextField
            inputRef={autocompleteRef}
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

VideoActors.propTypes = {
  actors: PropTypes.arrayOf(PropTypes.shape({
    id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired
  })).isRequired,
  videoId: PropTypes.string.isRequired,
  updateActors: PropTypes.func.isRequired
}