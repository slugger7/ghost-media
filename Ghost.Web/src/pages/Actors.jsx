import { Box, Chip } from '@mui/material'
import { includes } from 'ramda'
import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { fetchActors } from '../services/actor.service'
import usePromise from '../services/use-promise'
import { ActorChip } from '../components/ActorChip'
import { ChipSkeleton } from '../components/ChipSkeleton'
import { NothingHere } from '../components/NothingHere'
import { Search } from '../components/Search'

export const Actors = () => {
  const [actorsResult, , loading] = usePromise(() => fetchActors())
  const [filteredActors, setFilteredActors] = useState([])
  const [search, setSearch] = useState('')

  useEffect(() => {
    if (actorsResult) {
      setFilteredActors(
        actorsResult.filter((actor) =>
          includes(search.trim().toLowerCase(), actor.name.toLowerCase()),
        ),
      )
    }
  }, [search, actorsResult])

  return (
    <Box sx={{ my: 1 }}>
      <Box sx={{ mb: 1 }}>
        <Search search={search} setSearch={setSearch} />
      </Box>
      {loading && <ChipSkeleton />}
      {!loading && (
        <>
          {filteredActors.map((actor) => (
            <ActorChip actor={actor} key={actor.id} />
          ))}
          {filteredActors.length === 0 && (
            <NothingHere>Nothing here. Add some actors to videos.</NothingHere>
          )}
        </>
      )}
    </Box>
  )
}
