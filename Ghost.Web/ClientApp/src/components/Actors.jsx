import { Chip, Stack } from '@mui/material'
import React from 'react'
import { useAsync } from 'react-async-hook'
import { Link } from 'react-router-dom'
import { fetchActors } from '../services/actor.service'

export const Actors = () => {
  const actorsResult = useAsync(fetchActors, [])

  return <>
    <Stack direction="row" spacing={1}>
      {!actorsResult.loading && actorsResult.result.map(actor => <Chip
        key={actor._id}
        label={actor.name}
        variant="outlined"
        color="primary"
        component={Link}
        to={`/actors/${actor._id}/${encodeURIComponent(actor.name.toLowerCase())}`}
        clickable
      />)}
    </Stack>
  </>
}