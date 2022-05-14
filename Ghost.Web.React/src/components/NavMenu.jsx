import React, { useState } from 'react';
import { Box, Tabs } from '@mui/material'
import HomeIcon from '@mui/icons-material/Home';
import VideoLibraryIcon from '@mui/icons-material/VideoLibrary';
import TheaterComedyIcon from '@mui/icons-material/TheaterComedy';
import PeopleIcon from '@mui/icons-material/People';

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
          <LinkTab to="/" label="Home" icon={<HomeIcon />} iconPosition="start" />
          <LinkTab to="/libraries" label="Libraries" icon={<VideoLibraryIcon />} iconPosition="start" />
          <LinkTab to="/genres" label="Genres" icon={<TheaterComedyIcon />} iconPosition="start" />
          <LinkTab to="/actors" label="Actors" icon={<PeopleIcon />} iconPosition="start" />
        </Tabs>
      </Box>
    </header>
  );
}
