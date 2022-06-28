import { FormControl, InputAdornment, OutlinedInput, IconButton, Stack, Button } from '@mui/material';
import React, { useEffect, useCallback, useState } from 'react';
import PropTypes from 'prop-types'
import SearchIcon from '@mui/icons-material/Search';
import SearchOffIcon from '@mui/icons-material/SearchOff';

export const Search = ({ search, setSearch }) => {
  const [searchValue, setSearchValue] = useState(search);

  useEffect(() => {
    setSearchValue(search)
  }, [search])

  const handleKeyPress = (event) => {
    if (event.key === "Enter") {
      setSearch(searchValue);
    }
  }

  return <Stack direction="row" spacing={1}><FormControl variant="standard">
    <OutlinedInput
      onKeyUp={handleKeyPress}
      id='ghost-search-box'
      type='text'
      value={searchValue}
      onChange={(event) => setSearchValue(event.target.value)}
      endAdornment={
        <InputAdornment position="end">
          {searchValue.length === 0 && <SearchIcon />}
          {searchValue.length > 0 && <IconButton
            tabIndex={-1}
            aria-label="clear search"
            onClick={() => {
              setSearch('')
              setSearchValue('')
            }}
            edge="end"
          >
            <SearchOffIcon />
          </IconButton>}
        </InputAdornment>
      } />
  </FormControl>
    <Button variant="outlined" onClick={() => setSearch(searchValue)}><SearchIcon /> Search</Button>
  </Stack>
}

Search.propTypes = {
  search: PropTypes.string.isRequired,
  setSearch: PropTypes.func.isRequired
};
