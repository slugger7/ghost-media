import React from 'react'
import { Paper } from '@mui/material'
import PropTypes from 'prop-types'
import { mergeDeepRight } from 'ramda'

export const MediaSection = ({ children, sx = {} }) => (
  <Paper
    sx={mergeDeepRight(
      {
        py: 1,
        px: 2,
        my: 1,
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
      },
      sx,
    )}
  >
    {children}
  </Paper>
)

MediaSection.propTypes = {
  children: PropTypes.node,
  sx: PropTypes.object,
}
