import React, { useRef, useState, useEffect } from 'react'
import {
  Typography,
  TextField,
  Stack,
  Button,
  Box,
} from '@mui/material'
import PropTypes from 'prop-types'
import LoadingButton from '@mui/lab/LoadingButton'
import SaveIcon from '@mui/icons-material/Save'
import { EditIconButton } from './EditIconButton'

export const TextEdit = ({ text, update, loseFocus }) => {
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

  const handleKeyUp = (event) => {
    if (loseFocus && event.code === 'Escape') {
      loseFocus(() => inputRef.current.focus())
    }
  }

  return (
    <Box sx={{ width: '100%' }}>
      {!editing && (
        <Box
          sx={{
            display: 'flex',
            alignItems: 'flex-start',
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
          <EditIconButton
            onClick={() => {
              setEditing(true)
            }}
          />
        </Box>
      )}
      {editing && (
        <>
          <TextField
            onKeyUp={handleKeyUp}
            inputRef={inputRef}
            sx={{ mb: 1, width: '100%' }}
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
    </Box>
  )
}

TextEdit.propTypes = {
  text: PropTypes.string.isRequired,
  update: PropTypes.func.isRequired,
  loseFocus: PropTypes.func,
}
