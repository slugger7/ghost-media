import { Chip, } from '@mui/material'
import React from 'react'
import { useAsync } from 'react-async-hook'
import { Link } from 'react-router-dom'
import { fetchActors } from '../services/actor.service'
import { ChipSkeleton } from './ChipSkeleton'
import { NothingHere } from './NothingHere'

export const Actors = () => {
  const actorsResult = useAsync(fetchActors, [])

  return <>
    {actorsResult.loading && <ChipSkeleton />}
    {!actorsResult.loading && <>
      {actorsResult.result.map(actor => <Chip
        sx={{ m: 0.5 }}
        key={actor._id}
        label={`${actor.name} ${actor.videoCount}`}
        variant="outlined"
        color="primary"
        component={Link}
        to={`/actors/${actor._id}/${encodeURIComponent(actor.name.toLowerCase())}`}
        clickable
      />)}
      {actorsResult.result.length === 0 && <NothingHere>Nothing here. Add some actors to videos.</NothingHere>}
    </>}
  </>
}