import React from 'react'
import { useAsync } from 'react-async-hook'
import axios from 'axios'

import { ButtonLink } from './ButtonLink.jsx';
import { LibraryCard } from './LibraryCard.jsx'
import { Container, Stack } from '@mui/material';

const fetchLibraries = async () => (await axios.get("library")).data

export const Libraries = () => {
  const librariesPage = useAsync(fetchLibraries, [])

  // TODO: Make skeleton
  return (<Container>
    <Stack direction="column" spacing={1} sx={{ mb: 1 }}>
      {librariesPage.loading && <span>loading ...</span>}
      {!librariesPage.loading && librariesPage.result?.content?.map(library => (<LibraryCard key={library._id} library={library} refresh={librariesPage.execute} />))}
    </Stack>
    <ButtonLink to="/libraries/add" variant="contained">Add Library</ButtonLink>
  </Container>)
} 