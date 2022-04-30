import React from 'react'
import { useParams } from 'react-router-dom'
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { Container } from '@mui/material'
import { mergeDeepRight } from 'ramda'

import { Video } from './Video.jsx'
import { VideoGenres } from './VideoGenres.jsx'
import { VideoActors } from './VideoActors.jsx'
import { VideoTitle } from './VideoTitle.jsx'

const fetchMedia = async (id) => (await axios.get(`/media/${id}/info`)).data
const updateGenres = async (id, genres) => (await axios.put(`/media/${id}/genres`, genres)).data
const updateActors = async (id, actors) => (await axios.put(`/media/${id}/actors`, actors)).data
const updateTitle = async (id, title) => (await axios.put(`/media/${id}/title`, { title })).data

export const Media = () => {
  const params = useParams()
  const media = useAsync(fetchMedia, [params.id])

  return <>
    {media.loading && <span>...loading</span>}
    {!media.loading && <>
      <Video
        source={`${axios.defaults.baseURL}/media/${params.id}`}
        type={media.result.type}
        poster={`${axios.defaults.baseURL}/image/video/${params.id}`}
      />
      <Container>

        <VideoTitle video={media.result} updateTitle={async (title) => {
          const video = await updateTitle(params.id, title)
          media.set(mergeDeepRight(media, { result: video }))
        }} />
        <VideoGenres genres={media.result.genres} videoId={params.id}
          updateGenres={async (genres) => {
            const video = await updateGenres(params.id, genres)
            media.set(mergeDeepRight(media, { result: video }))
          }}
        />
        <VideoActors
          actors={media.result.actors}
          videoId={params.id}
          updateActors={async (actors) => {
            const video = await updateActors(params.id, actors)
            media.set(mergeDeepRight(media, { result: video }))
          }}
        />
      </Container>
    </>}
  </>
}