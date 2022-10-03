import { Chip } from '@mui/material'
import React, { useState, useEffect } from 'react'
import { fetchGenres } from '../services/genre.service'
import { Link } from 'react-router-dom'
import { ChipSkeleton } from './ChipSkeleton.jsx'
import { NothingHere } from './NothingHere.jsx'
import { Search } from './Search'
import { Box } from '@mui/system'
import { includes } from 'ramda'
import usePromise from '../services/use-promise'

export const Genres = () => {
  const [genres,, loadingGenres] = usePromise(() => fetchGenres())
  const [filteredGenres, setFilteredGenres] = useState([])
  const [search, setSearch] = useState('')

  useEffect(() => {
    if (genres) {
      setFilteredGenres(genres.filter(
        genre => includes(search.trim().toLowerCase(), genre.name.toLowerCase()))
      );
    }
  }, [genres, search])

  return <Box sx={{ my: 1 }}>
    <Box sx={{ mb: 1 }}>
      <Search search={search} setSearch={setSearch} />
    </Box>
    {loadingGenres && <ChipSkeleton />}
    {!loadingGenres && <>
      {filteredGenres.map(genre => <Chip
        sx={{ m: 0.5 }}
        key={genre.id}
        label={`${genre.name} ${genre.videoCount}`}
        variant="outlined"
        color="primary"
        component={Link}
        to={`/genres/${encodeURIComponent(genre.name)}`}
        clickable />)
      }
      {filteredGenres.length === 0 && <NothingHere>Nothing here. Add some genres to videos.</NothingHere>}
    </>}
  </Box>
}