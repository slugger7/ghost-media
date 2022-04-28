import { FormControl, InputAdornment, OutlinedInput, IconButton } from '@mui/material';
import React, { useEffect, useCallback, useState } from 'react';
import PropTypes from 'prop-types'
import SearchIcon from '@mui/icons-material/Search';
import SearchOffIcon from '@mui/icons-material/SearchOff';
import throttle from 'lodash.throttle';

export const Search = ({ search, setSearch }) => {
  const [searchValue, setSearchValue] = useState(search);
  const throttledSearch = useCallback(throttle(setSearch, 400, { trailing: true, leading: false }), []);

  useEffect(() => {
    if (searchValue) {
      throttledSearch(searchValue);
    }
  }, [searchValue]);

  useEffect(() => {
    setSearchValue(search)
  }, [search])

  return <FormControl variant="standard">
    <OutlinedInput
      id='ghost-search-box'
      type='text'
      value={searchValue}
      onChange={(event) => setSearchValue(event.target.value)}
      endAdornment={
        <InputAdornment position="end">
          {search.length === 0 && <SearchIcon />}
          {search.length > 0 && <IconButton
            aria-label="clear search"
            onClick={() => setSearch('')}
            edge="end"
          >
            <SearchOffIcon />
          </IconButton>}
        </InputAdornment>
      } />
  </FormControl>
}

Search.propTypes = {
  search: PropTypes.string.isRequired,
  setSearch: PropTypes.func.isRequired
};
