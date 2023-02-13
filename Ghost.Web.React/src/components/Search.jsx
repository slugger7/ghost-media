import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import {
  FormControl,
  InputAdornment,
  OutlinedInput,
  IconButton,
  Button,
  Box,
} from '@mui/material'
import SearchIcon from '@mui/icons-material/Search'
import SearchOffIcon from '@mui/icons-material/SearchOff'

export const Search = ({ search, setSearch }) => {
  const [searchValue, setSearchValue] = useState(search)

  useEffect(() => {
    setSearchValue(search)
  }, [search])

  const handleKeyPress = (event) => {
    if (event.key === 'Enter') {
      setSearch(searchValue)
    }
  }

  return (
    <Box sx={{ display: 'flex' }}>
      <FormControl variant="standard" size="small" sx={{ mr: 1, mb: 1 }}>
        <OutlinedInput
          onKeyUp={handleKeyPress}
          id="ghost-search-box"
          type="text"
          value={searchValue}
          onChange={(event) => setSearchValue(event.target.value)}
          endAdornment={
            <InputAdornment position="end">
              {searchValue.length === 0 && <SearchIcon />}
              {searchValue.length > 0 && (
                <IconButton
                  tabIndex={-1}
                  aria-label="clear search"
                  onClick={() => {
                    setSearch('')
                    setSearchValue('')
                  }}
                  edge="end"
                >
                  <SearchOffIcon />
                </IconButton>
              )}
            </InputAdornment>
          }
        />
      </FormControl>
      <Button
        sx={{ mr: 1, mb: 1, p: 1 }}
        size="small"
        variant="outlined"
        onClick={() => setSearch(searchValue)}
      >
        <SearchIcon /> Search
      </Button>
    </Box>
  )
}

Search.propTypes = {
  search: PropTypes.string.isRequired,
  setSearch: PropTypes.func.isRequired,
}
