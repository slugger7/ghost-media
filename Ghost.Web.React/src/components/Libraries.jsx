import React from 'react'
import { useAsync } from 'react-async-hook'
import axios from 'axios'

import { ButtonLink } from './ButtonLink.jsx';
import { LibraryCard } from './LibraryCard.jsx'
import { Container, Stack, Skeleton, Paper } from '@mui/material';
import { NothingHere } from './NothingHere.jsx';

const fetchLibraries = async () => (await axios.get("library")).data

export const Libraries = () => {
  const librariesPage = useAsync(fetchLibraries, [])

  return (<Container>
    {librariesPage.loading && <Skeleton height="90px" />}
    {!librariesPage.loading && <Stack direction="column" spacing={1} sx={{ mb: 1 }}>
      {librariesPage.result?.content?.map(library => (<LibraryCard key={library._id} library={library} refresh={librariesPage.execute} />))}
      {librariesPage.result?.content.length === 0 && <NothingHere>Nothing here, try adding a library.</NothingHere>}
    </Stack>}
    <ButtonLink to="/libraries/add" variant="contained">Add Library</ButtonLink>
  </Container >)
} 