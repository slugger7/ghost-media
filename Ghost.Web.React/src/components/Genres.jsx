import { Chip } from '@mui/material'
import React from 'react'
import { useAsync } from 'react-async-hook'
import { fetchGenres } from '../services/genre.service'
import { Link } from 'react-router-dom'
import { ChipSkeleton } from './ChipSkeleton.jsx'
import { NothingHere } from './NothingHere.jsx'

export const Genres = () => {
  const genresResult = useAsync(fetchGenres, [])

  return <>
    {genresResult.loading && <ChipSkeleton />}
    {!genresResult.loading && <>
      {genresResult.result.map(genre => <Chip
        sx={{ m: 0.5 }}
        key={genre.id}
        label={`${genre.name} ${genre.videoCount}`}
        variant="outlined"
        color="primary"
        component={Link}
        to={`/genres/${encodeURIComponent(genre.name)}`}
        clickable />)
      }
      {genresResult.result.length === 0 && <NothingHere>Nothing here. Add some genres to videos.</NothingHere>}
    </>}
  </>
}