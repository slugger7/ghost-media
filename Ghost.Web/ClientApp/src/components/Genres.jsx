import { Chip, Stack } from '@mui/material'
import React from 'react'
import { useAsync } from 'react-async-hook'
import { fetchGenres } from '../services/genre.service'
import { Link } from 'react-router-dom'

export const Genres = () => {
  const genresResult = useAsync(fetchGenres, [])

  return <>
    <Stack direction="row" spacing={1}>
      {!genresResult.loading && genresResult.result.map(genre => <Chip
        key={genre._id}
        label={genre.name}
        variant="outlined"
        color="primary"
        component={Link}
        to={`/genres/${encodeURIComponent(genre.name)}`}
        clickable />)}
    </Stack>
  </>
}