import React from 'react'
import { Button } from '@mui/material'
import { Link } from 'react-router-dom'

export const ButtonLink = ({ children, ...props}) => (<Button 
  LinkComponent={Link} 
  {...props}>
    {children}
</Button>)