import React, { useRef } from 'react'
import PropTypes from 'prop-types'
import { Autocomplete, TextField } from '@mui/material'
import { prop } from 'ramda'
import usePromise from '../services/use-promise'
import { fetchGenres } from '../services/genre.service'

export const GenreFilter = ({ setSelectedGenres, selectedGenres }) => {
  const [allGenres, , loadingAllGenres] = usePromise(() => fetchGenres())
  const autocompleteRef = useRef()

  return (
    <Autocomplete
      sx={{ mb: 1 }}
      multiple
      size="small"
      onChange={(e, newGenres) => setSelectedGenres(newGenres)}
      options={allGenres?.map(prop('name')) || []}
      loading={loadingAllGenres}
      defaultValue={selectedGenres}
      renderInput={(params) => (
        <TextField
          size="small"
          sx={{ minWidth: '100px' }}
          placeholder="Genres"
          id="genre-filter-text-box"
          inputRef={autocompleteRef}
          label="Genres"
          {...params}
        />
      )}
    />
  )
}

GenreFilter.propTypes = {
  setSelectedGenres: PropTypes.func.isRequired,
  selectedGenres: PropTypes.arrayOf(PropTypes.string).isRequired,
}
