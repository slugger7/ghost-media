import React, { useRef, useState, useEffect } from 'react'
import { IconButton, Typography, TextField, Stack, Button } from "@mui/material"
import EditIcon from '@mui/icons-material/Edit';
import PropTypes from 'prop-types'
import LoadingButton from '@mui/lab/LoadingButton';
import SaveIcon from '@mui/icons-material/Save';

export const VideoTitle = ({ video, updateTitle }) => {
  const [editing, setEditing] = useState(false);
  const [title, setTitle] = useState(video.title)
  const [submitting, setSubmitting] = useState(false);
  const inputRef = useRef()

  useEffect(() => {
    if (editing) {
      inputRef.current.focus()
    }
  }, [editing])

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

  return <>
    <Stack directior="column">
      {!editing && <Typography variant="h4" gutterBottom component="h4">{video.title} <IconButton color="primary" onClick={() => { setEditing(true) }}>
        <EditIcon />
      </IconButton>
      </Typography>}
      {editing && <>
        <TextField
          inputRef={inputRef}
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
  </>
}

VideoTitle.propTypes = {
  video: PropTypes.shape({
    _id: PropTypes.number.isRequired,
    title: PropTypes.string.isRequired
  }).isRequired,
  updateTitle: PropTypes.func.isRequired
}