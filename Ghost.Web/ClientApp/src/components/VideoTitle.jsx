import React, { useState } from 'react'
import { IconButton, Typography, Paper, TextField, Stack, Button } from "@mui/material"
import EditIcon from '@mui/icons-material/Edit';
import PropTypes from 'prop-types'
import LoadingButton from '@mui/lab/LoadingButton';
import SaveIcon from '@mui/icons-material/Save';

export const VideoTitle = ({ video, updateTitle }) => {
  const [editing, setEditing] = useState(false);
  const [title, setTitle] = useState(video.title)
  const [submitting, setSubmitting] = useState(false);

  const handleSubmit = async () => {
    setSubmitting(true);
    await updateTitle(title)
    setSubmitting(false)
    setEditing(false)
  }
  const handleCancel = () => {
    setEditing(false);
    setTitle(video.title);
  }

  return <Paper sx={{ p: 2 }}>
    <Stack directior="column">
      {!editing && <Typography variant="h3" gutterBottom component="h3">{video.title} <IconButton color="primary" onClick={() => { setEditing(true) }}>
        <EditIcon />
      </IconButton>
      </Typography>}
      {editing && <>
        <TextField
          sx={{ mb: 1 }}
          id="ghost-edit-title-field"
          label="Title"
          variant="outlined"
          value={title}
          onChange={(event) => setTitle(event.target.value)} />
        <Stack direction="row" spacing={2}>
          <LoadingButton
            onClick={handleSubmit}
            variant="contained"
            loading={submitting}
            disabled={submitting}
            loadingPosition="start"
            startIcon={<SaveIcon />}>Done</LoadingButton>
          <Button onClick={handleCancel}>Cancel</Button>
        </Stack>
      </>}
    </Stack>
  </Paper>
}

VideoTitle.propTypes = {
  video: PropTypes.shape({
    _id: PropTypes.string.isRequired,
    title: PropTypes.string.isRequired
  }).isRequired,
  updateTitle: PropTypes.func.isRequired
}