import React, { useState } from 'react';
import { Box, Tabs } from '@mui/material'
import HomeIcon from '@mui/icons-material/Home';
import TheaterComedyIcon from '@mui/icons-material/TheaterComedy';
import PeopleIcon from '@mui/icons-material/People';
import SettingsIcon from '@mui/icons-material/Settings';
import FavoriteIcon from '@mui/icons-material/Favorite';

import { LinkTab } from './LinkTab.jsx'

export const NavMenu = () => {
  const [value, setValue] = useState(0);

  const handleTabChange = (_, newValue) => {
    setValue(newValue)
  }

  return (
    <header>
      <Box>
        <Tabs
          value={value}
          onChange={handleTabChange}
          aria-label="nav tabs scrollable auto"
          variant="scrollable"
          scrollButtons="auto">
          <LinkTab to="/" label="Home" icon={<HomeIcon />} iconPosition="start" />
          <LinkTab to="/genres" label="Genres" icon={<TheaterComedyIcon />} iconPosition="start" />
          <LinkTab to="/actors" label="Actors" icon={<PeopleIcon />} iconPosition="start" />
          <LinkTab to="/favourites" label="Favourites" icon={<FavoriteIcon />} iconPosition="start" />
          <LinkTab to="/settings" label="Settings" icon={<SettingsIcon />} iconPosition="start" />
        </Tabs>
      </Box>
    </header>
  );
}
