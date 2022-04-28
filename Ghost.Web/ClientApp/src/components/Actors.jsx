import { Chip, Stack } from '@mui/material'
import React from 'react'
import { useAsync } from 'react-async-hook'
import { Link } from 'react-router-dom'
import { fetchActors } from '../services/actor.service'

export const Actors = () => {
  const actorsResult = useAsync(fetchActors, [])

  return <>
    {!actorsResult.loading && actorsResult.result.map(actor => <Chip
      sx={{ m: 0.5 }}
      key={actor._id}
      label={`${actor.name} ${actor.videoCount}`}
      variant="outlined"
      color="primary"
      component={Link}
      to={`/actors/${actor._id}/${encodeURIComponent(actor.name.toLowerCase())}`}
      clickable
    />)}
  </>
}