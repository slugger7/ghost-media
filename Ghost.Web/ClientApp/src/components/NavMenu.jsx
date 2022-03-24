import React, { useState } from 'react';
import { Box, Tabs } from '@mui/material'

import { LinkTab } from './LinkTab.jsx'

export const NavMenu = () => {
  const [value, setValue] = useState(0);

  const handleTabChange = (_, newValue) => {
    setValue(newValue)
  }

  return (
    <header>
      <Box sx={{ width: '100%' }}>
        <Tabs value={value} onChange={handleTabChange} aria-label="nav tabs">
          <LinkTab to="/" label="Home" />
          <LinkTab to="/library" label="Library"/>
          <LinkTab to="/favourites" label="Favourites" />
        </Tabs>
      </Box>
    </header>
  );
}
