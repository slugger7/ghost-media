import React from 'react'
import PropTypes from 'prop-types'
import { IconButton } from '@mui/material'
import ContentCopyIcon from '@mui/icons-material/ContentCopy';

export const CopyIconButton = ({ onClick = () => { } }) => (
  <IconButton sx={{ ml: 1 }} color="primary" onClick={onClick}>
    <ContentCopyIcon />
  </IconButton>
)

CopyIconButton.propTypes = {
  onClick: PropTypes.func,
}
