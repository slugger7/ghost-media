import React, { useEffect, useRef, useState } from 'react'
import PropTypes from 'prop-types'
import {
  Stack,
  Chip,
  Typography,
  Button,
  Autocomplete,
  TextField,
  Box,
} from '@mui/material'
import { Link } from 'react-router-dom'
import { prop } from 'ramda'
import LoadingButton from '@mui/lab/LoadingButton'
import SaveIcon from '@mui/icons-material/Save'

import { fetchGenres } from '../services/genre.service'
import usePromise from '../services/use-promise'
import { EditIconButton } from './EditIconButton'

export const VideoGenres = ({ genres, updateGenres, loseFocus }) => {
  const [editing, setEditing] = useState(false)
  const [allGenres, , loadingGenres] = usePromise(() => fetchGenres())
  const [selectedGenres, setSelectedGenres] = useState([
    ...genres.map(prop('name')),
  ])
  const [submitting, setSubmitting] = useState(false)
  const autocompleteRef = useRef()

  useEffect(() => {
    if (editing) {
      autocompleteRef.current.focus()
    }
  }, [editing])

  const handleCancel = () => {
    setEditing(false)
  }

  const handleSubmit = async () => {
    setSubmitting(true)
    await updateGenres({ genres: selectedGenres })
    setSubmitting(false)
    setEditing(false)
  }

  const handleKeyUp = (event) => {
    if (loseFocus && event.code === 'Escape') {
      loseFocus(() => autocompleteRef.current.focus())
    }
  }

  return (
    <Box sx={{ width: '100%' }}>
      <Box
        sx={{
          display: 'flex',
          alignItems: 'center',
        }}
      >
        <Typography variant="h5" component="h5">
          Genres
        </Typography>
        {!editing && <EditIconButton onClick={() => setEditing(true)} />}
      </Box>
      <Stack direction="column" spacing={1}>
        <Box>
          {!editing &&
            genres.map(({ name, videoCount }, index) => (
              <Chip
                sx={{ m: 0.5 }}
                variant="outlined"
                color="primary"
                key={index}
                label={`${name} ${videoCount}`}
                component={Link}
                to={`/genres/${encodeURIComponent(name.toLowerCase())}`}
                clickable
              />
            ))}
          {!editing && genres.length === 0 && (
            <Chip variant="outlined" label="None"></Chip>
          )}
        </Box>
        {editing && (
          <>
            <Autocomplete
              multiple
              freeSolo
              onChange={(e, newGenres) => setSelectedGenres(newGenres)}
              options={allGenres?.map(prop('name')) || []}
              defaultValue={selectedGenres}
              loading={loadingGenres}
              renderInput={(params) => (
                <TextField
                  onKeyUp={handleKeyUp}
                  inputRef={autocompleteRef}
                  {...params}
                />
              )}
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
    </Box>
  )
}

VideoGenres.propTypes = {
  genres: PropTypes.arrayOf(
    PropTypes.shape({
      name: PropTypes.string.isRequired,
      videoCount: PropTypes.number.isRequired,
    }),
  ).isRequired,
  updateGenres: PropTypes.func.isRequired,
  loseFocus: PropTypes.func,
}
