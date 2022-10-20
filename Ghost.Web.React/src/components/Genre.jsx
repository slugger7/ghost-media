import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { mergeDeepLeft } from 'ramda'
import { constructVideoParams } from '../services/video.service.js'
import { TextEdit } from './TextEdit.jsx'
import usePromise from '../services/use-promise.js'
import { VideoView } from './VideoView.jsx'

const fetchGenre = async (name) =>
  (await axios.get(`/genre/${encodeURIComponent(name)}`)).data
const fetchVideos =
  (genre) =>
  async ({ page, limit, search, sortBy, ascending, watchState }) => {
    const videosResult = await axios.get(
      `/media/genre/${encodeURIComponent(genre)}?${constructVideoParams({
        page,
        limit,
        search,
        sortBy,
        ascending,
        watchState,
      })}`,
    )

    return videosResult.data
  }
const updateGenreName = async (id, name) =>
  (await axios.put(`/genre/${id}`, { name })).data

export const Genre = () => {
  const params = useParams()
  const [genre, , loadingGenre, setGenre] = usePromise(
    () => fetchGenre(params.name),
    [params.name],
  )

  const handleGenreUpdate = async (genreName) => {
    const newGenre = await updateGenreName(genre.id, genreName)
    setGenre(mergeDeepLeft({ name: newGenre.name }))
  }

  return (
    <>
      {!loadingGenre && (
        <TextEdit text={genre.name} update={handleGenreUpdate} />
      )}
      <VideoView fetchFn={fetchVideos(params.name)} />
    </>
  )
}
