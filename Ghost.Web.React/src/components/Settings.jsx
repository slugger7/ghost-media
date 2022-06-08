import React from 'react'
import { Container } from '@mui/material'
import { Libraries } from './Libraries';
import { Users } from './Users'

export const Settings = () => {
  return <Container>
    <Libraries />
    <Users />
  </Container>
}