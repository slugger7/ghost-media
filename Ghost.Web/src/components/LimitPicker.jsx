import React from 'react'
import PropTypes from 'prop-types'
import { Box, FormControl, InputLabel, MenuItem, Select } from '@mui/material'

export const LimitPicker = ({ limit, setLimit }) => (
  <Box sx={{ mr: 1, mb: 1 }}>
    <FormControl sx={{ minWidth: '100px' }} size="small">
      <InputLabel id="limit-picker-label">Page Size</InputLabel>
      <Select
        labelId="limit-picker-label"
        label="Page Size"
        value={limit}
        onChange={(event) => setLimit(event.target.value)}
      >
        <MenuItem value={12}>12</MenuItem>
        <MenuItem value={24}>24</MenuItem>
        <MenuItem value={36}>36</MenuItem>
        <MenuItem value={48}>48</MenuItem>
        <MenuItem value={60}>60</MenuItem>
      </Select>
    </FormControl>
  </Box>
)

LimitPicker.propTypes = {
  limit: PropTypes.number.isRequired,
  setLimit: PropTypes.func.isRequired,
}
