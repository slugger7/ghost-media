import React from 'react';
import { ThemeProvider, Container, CssBaseline } from '@mui/material'
import { createTheme } from '@mui/material/styles'
import { NavMenu } from './NavMenu.jsx';

const darkTheme = createTheme({
  palette: {
    mode: 'dark'
  }
})

export const Layout = ({ children }) => (<ThemeProvider theme={darkTheme}>
  <Container component="main">
    <CssBaseline />
    <NavMenu />
    {children}
  </Container>
</ThemeProvider>)
