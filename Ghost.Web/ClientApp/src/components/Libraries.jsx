import React from 'react'
import { useAsync } from 'react-async-hook'
import axios from 'axios'

import { ButtonLink } from './ButtonLink.jsx';
import { LibraryCard } from './LibraryCard.jsx'
import { Container, Stack, Skeleton, Paper } from '@mui/material';

const fetchLibraries = async () => (await axios.get("library")).data

export const Libraries = () => {
  const librariesPage = useAsync(fetchLibraries, [])

  return (<Container>
    {librariesPage.loading && <Skeleton height="90px" />}
    {!librariesPage.loading && <Stack direction="column" spacing={1} sx={{ mb: 1 }}>
      {librariesPage.result?.content?.map(library => (<LibraryCard key={library._id} library={library} refresh={librariesPage.execute} />))}
      {librariesPage.result?.content.length === 0 && <Paper elevation={1} sx={{ my: 1, p: 2 }}>Nothing here, try adding a library.</Paper>}
    </Stack>}
    <ButtonLink to="/libraries/add" variant="contained">Add Library</ButtonLink>
  </Container >)
} 