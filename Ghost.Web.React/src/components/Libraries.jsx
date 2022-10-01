import React from 'react'
import axios from 'axios'

import { ButtonLink } from './ButtonLink.jsx';
import { LibraryCard } from './LibraryCard.jsx'
import { Stack, Skeleton, Typography, Box } from '@mui/material';
import { NothingHere } from './NothingHere.jsx';
import usePromise from '../services/use-promise.js';

const fetchLibraries = async () => (await axios.get("library")).data

export const Libraries = () => {
  const librariesPage = usePromise(() => fetchLibraries());

  return (<Box>
    <Typography variant="h4" component="h4">Libraries</Typography>
    {librariesPage.loading && <Skeleton height="90px" />}
    {!librariesPage.loading && <Stack direction="column" spacing={1} sx={{ mb: 1 }}>
      {librariesPage.result?.content?.map(library => (<LibraryCard key={library.id} library={library} refresh={librariesPage.execute} />))}
      {librariesPage.result?.content.length === 0 && <NothingHere>Nothing here, try adding a library.</NothingHere>}
    </Stack>}
    <ButtonLink to="/libraries/add" variant="contained">Add Library</ButtonLink>
  </Box>
  )
} 