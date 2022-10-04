import React from 'react'
import {
  Box,
  FormControl,
  IconButton,
  InputLabel,
  MenuItem,
  Select,
} from '@mui/material'
import PropTypes from 'prop-types'
import ArrowDownwardIcon from '@mui/icons-material/ArrowDownward'
import ArrowUpwardIcon from '@mui/icons-material/ArrowUpward'

export const Sort = ({
  sortBy,
  setSortBy,
  sortDirection,
  setSortDirection,
}) => (
  <Box sx={{ mr: 1, mb: 1 }}>
    <FormControl sx={{ minWidth: '150px' }} size="small">
      <InputLabel id="sort-label">Sort By</InputLabel>
      <Select
        labelId="sort-label"
        label="Sort By"
        value={sortBy}
        onChange={(event) => setSortBy(event.target.value)}
      >
        <MenuItem value="title">Title</MenuItem>
        <MenuItem value="date-added">Date Added</MenuItem>
        <MenuItem value="date-created">Date Created</MenuItem>
        <MenuItem value="size">Size</MenuItem>
        <MenuItem value="runtime">Runtime</MenuItem>
      </Select>
    </FormControl>
    {sortDirection !== undefined && (
      <IconButton
        onClick={() => {
          setSortDirection(!sortDirection)
        }}
      >
        {sortDirection ? <ArrowDownwardIcon /> : <ArrowUpwardIcon />}
      </IconButton>
    )}
  </Box>
)

Sort.propTypes = {
  sortBy: PropTypes.string.isRequired,
  setSortBy: PropTypes.func.isRequired,
  sortDirection: PropTypes.bool,
  setSortDirection: PropTypes.func,
}
