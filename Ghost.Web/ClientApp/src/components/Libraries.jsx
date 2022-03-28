import React from 'react'
import { useAsync } from 'react-async-hook'
import axios from 'axios'

import { ButtonLink } from './ButtonLink.jsx';

const fetchLibraries = async () => (await axios.get("library")).data

export const Libraries = () => {
  const librariesPage = useAsync(fetchLibraries, [])

  return (<>

    {librariesPage.loading && <span>loading ...</span>}
    {!librariesPage.loading && librariesPage.result?.content?.map(library => (<pre key={library._id}>{JSON.stringify(library, null , 2)}</pre>))}
    <ButtonLink to="/libraries/add">Add Library</ButtonLink>
  </>)
}