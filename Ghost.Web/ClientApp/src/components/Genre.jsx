import axios from 'axios';
import React from 'react';
import { useAsync } from 'react-async-hook';
import { useParams } from 'react-router-dom';

const fetchGenre = async (name) => (await axios.get(`/genre/${name}`)).data;

export const Genre = () => {
  const params = useParams()
  const genre = useAsync(fetchGenre, [params.name])

  return <>
    {genre.loading}
    {!genre.loading && <span>{genre.result.name}</span>}
  </>
}