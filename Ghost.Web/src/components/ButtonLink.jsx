import React from 'react'
import { Button } from '@mui/material'
import { Link } from 'react-router-dom'
import PropTypes from 'prop-types'

export const ButtonLink = ({ children, ...props }) => (<Button
  LinkComponent={Link}
  {...props}>
  {children}
</Button>)

ButtonLink.propTypes = {
  children: PropTypes.node
}