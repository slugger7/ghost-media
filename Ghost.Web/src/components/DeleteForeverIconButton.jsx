import React from 'react'
import { IconButton, Tooltip } from '@mui/material'
import PropTypes from 'prop-types'
import DeleteForeverIcon from '@mui/icons-material/DeleteForever';

export const DeleteForeverIconButton = ({ onClick }) => {

  return <Tooltip title={"Delete"}>
    <IconButton aria-label="add to favourites" onClick={onClick}>
      <DeleteForeverIcon />
    </IconButton>
  </Tooltip>
}

DeleteForeverIconButton.propTypes = {
  onClick: PropTypes.func
}