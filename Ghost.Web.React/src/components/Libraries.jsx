import React from 'react'
import axios from 'axios'

import { ButtonLink } from './ButtonLink.jsx'
import { LibraryCard } from './LibraryCard.jsx'
import { Stack, Skeleton, Typography, Box } from '@mui/material'
import { NothingHere } from './NothingHere.jsx'
import usePromise from '../services/use-promise.js'

const fetchLibraries = async () => (await axios.get('library')).data

export const Libraries = () => {
  const [libraries, , loadingLibraries] = usePromise(() => fetchLibraries())

  return (
    <Box>
      <Typography variant="h4" component="h4">
        Libraries
      </Typography>
      {loadingLibraries && <Skeleton height="90px" />}
      {!loadingLibraries && (
        <Stack direction="column" spacing={1} sx={{ mb: 1 }}>
          {libraries?.content?.map((library) => (
            <LibraryCard
              key={library.id}
              library={library}
              updateLibraries={() => {}}
            />
          ))}
          {libraries?.content.length === 0 && (
            <NothingHere>Nothing here, try adding a library.</NothingHere>
          )}
        </Stack>
      )}
      <ButtonLink to="/libraries/add" variant="contained">
        Add Library
      </ButtonLink>
    </Box>
  )
}
