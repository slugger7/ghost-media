import React from 'react'
import { useAsync } from 'react-async-hook'
import axios from 'axios'

import { ButtonLink } from './ButtonLink.jsx';
import { LibraryCard } from './LibraryCard.jsx'

const fetchLibraries = async () => (await axios.get("library")).data

export const Libraries = () => {
  const librariesPage = useAsync(fetchLibraries, [])

  return (<>
    {librariesPage.loading && <span>loading ...</span>}
    {!librariesPage.loading && librariesPage.result?.content?.map(library => (<LibraryCard key={library._id} library={library} />))}
    <ButtonLink to="/libraries/add" variant="contained">Add Library</ButtonLink>
  </>)
} 