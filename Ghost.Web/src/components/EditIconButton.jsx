import React from 'react'
import PropTypes from 'prop-types'
import { IconButton } from '@mui/material'
import EditIcon from '@mui/icons-material/Edit'

export const EditIconButton = ({ onClick = () => {} }) => (
  <IconButton sx={{ ml: 1 }} color="primary" onClick={onClick}>
    <EditIcon />
  </IconButton>
)

EditIconButton.propTypes = {
  onClick: PropTypes.func,
}
