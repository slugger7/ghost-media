import React, { useRef, useState, useEffect } from 'react'
import {
  IconButton,
  Typography,
  TextField,
  Stack,
  Button,
  Box,
} from '@mui/material'
import EditIcon from '@mui/icons-material/Edit'
import PropTypes from 'prop-types'
import LoadingButton from '@mui/lab/LoadingButton'
import SaveIcon from '@mui/icons-material/Save'

export const TextEdit = ({ text, update }) => {
  const [editing, setEditing] = useState(false)
  const [localText, setText] = useState(text)
  const [submitting, setSubmitting] = useState(false)
  const inputRef = useRef()

  useEffect(() => {
    if (editing) {
      inputRef.current.focus()
    }
  }, [editing])

  const handleSubmit = async () => {
    setSubmitting(true)
    await update(localText)
    setSubmitting(false)
    setEditing(false)
  }
  const handleCancel = () => {
    setEditing(false)
    setText(text)
  }

  return (
    <>
      <Stack direction="column">
        {!editing && (
          <Box
            sx={{
              display: 'flex',
              alignItems: 'flex-start',
              justifyContent: 'space-between',
            }}
          >
            <Typography
              variant="h4"
              gutterBottom
              component="h4"
              overflow="hidden"
              textOverflow="ellipsis"
            >
              {localText}
            </Typography>
            <IconButton
              color="primary"
              onClick={() => {
                setEditing(true)
              }}
            >
              <EditIcon />
            </IconButton>
          </Box>
        )}
        {editing && (
          <>
            <TextField
              inputRef={inputRef}
              sx={{ mb: 1 }}
              id="ghost-edit-field"
              variant="outlined"
              value={localText}
              onChange={(event) => setText(event.target.value)}
            />
            <Stack direction="row" spacing={2}>
              <LoadingButton
                onClick={handleSubmit}
                variant="contained"
                loading={submitting}
                disabled={submitting}
                loadingPosition="start"
                startIcon={<SaveIcon />}
              >
                Done
              </LoadingButton>
              <Button onClick={handleCancel}>Cancel</Button>
            </Stack>
          </>
        )}
      </Stack>
    </>
  )
}

TextEdit.propTypes = {
  text: PropTypes.string.isRequired,
  update: PropTypes.func.isRequired,
}
