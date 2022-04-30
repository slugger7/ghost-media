import React from 'react'
import { Paper } from '@mui/material'
import PropTypes from 'prop-types'

export const NothingHere = ({ children }) => <Paper elevation={1} sx={{ my: 1, p: 2 }}>{children}</Paper>

NothingHere.propTypes = {
  children: PropTypes.string.isRequired
}
