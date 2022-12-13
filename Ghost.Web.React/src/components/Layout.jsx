import React, { useContext } from 'react';
import { ThemeProvider, Container, CssBaseline } from '@mui/material'
import { createTheme } from '@mui/material/styles'
import PropTypes from 'prop-types'

import { NavMenu } from './NavMenu.jsx';
import AuthenticationContext from '../context/authentication.context.js';

const darkTheme = createTheme({
  palette: {
    mode: 'dark'
  }
})

export const Layout = ({ children }) => {
  const {authenticated} = useContext(AuthenticationContext);
  return <ThemeProvider theme={darkTheme}>
    <CssBaseline />
    <Container maxWidth={false}>
      {authenticated && <NavMenu />}
      {children}
    </Container>
  </ThemeProvider>
}

Layout.propTypes = {
  children: PropTypes.node
}