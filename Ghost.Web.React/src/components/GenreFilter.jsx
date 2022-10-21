import React, { useRef } from 'react'
import PropTypes from 'prop-types'
import { Autocomplete, TextField } from '@mui/material'
import { prop } from 'ramda'
import usePromise from '../services/use-promise'
import { fetchGenres } from '../services/genre.service'

export const GenreFilter = ({ setSelectedGenres }) => {
  const [allGenres, , loadingAllGenres] = usePromise(() => fetchGenres())
  const autocompleteRef = useRef()

  return (
    <Autocomplete
      multiple
      onChange={(e, newGenres) => setSelectedGenres(newGenres)}
      options={allGenres?.map(prop('name')) || []}
      loading={loadingAllGenres}
      renderInput={(params) => (
        <TextField
          sx={{ mr: 1 }}
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