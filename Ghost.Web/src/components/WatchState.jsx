import React from 'react'
import PropTypes from 'prop-types'
import { FormControl, Select, MenuItem, InputLabel } from '@mui/material'
import watchStates from '../constants/watch-states.js'

export const WatchState = ({ watchState, setWatchState }) => {
  return (
    <FormControl size="small" sx={{ mr: 1, mb: 1 }}>
      <InputLabel id="watch-state-label">Watch state</InputLabel>
      <Select
        sx={{ minWidth: '150px' }}
        labelId={'watch-state-label'}
        id="watch-state-select"
        value={watchState}
        label={'Watch state'}
        onChange={(e) => setWatchState(e.target.value)}
        defaultValue={watchState.value}
      >
        <MenuItem value={watchStates.inProgress.value}>
          {watchStates.inProgress.name}
        </MenuItem>
        <MenuItem value={watchStates.watched.value}>
          {watchStates.watched.name}
        </MenuItem>
        <MenuItem value={watchStates.unwatched.value}>
          {watchStates.unwatched.name}
        </MenuItem>
        <MenuItem value={watchStates.all.value}>
          {watchStates.all.name}
        </MenuItem>
      </Select>
    </FormControl>
  )
}

WatchState.propTypes = {
  watchState: PropTypes.string.isRequired,
  setWatchState: PropTypes.func.isRequired,
}
