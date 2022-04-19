import React from 'react'
import { useParams } from 'react-router-dom'
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { Container, IconButton, Typography } from '@mui/material'
import EditIcon from '@mui/icons-material/Edit';
import { mergeDeepRight, prop } from 'ramda'

import { Video } from './Video.jsx'
import { VideoGenres } from './VideoGenres.jsx'
import { VideoActors } from './VideoActors.jsx'

const fetchMedia = async (id) => (await axios.get(`/media/${id}/info`)).data
const updateGenres = async (id, genres) => (await axios.put(`/media/${id}/genres`, genres)).data
const updateActors = async (id, actors) => (await axios.put(`/media/${id}/actors`, actors)).data

export const Media = () => {
  const params = useParams()
  const media = useAsync(fetchMedia, [params.id])

  return <Container>
    {media.loading && <span>...loading</span>}
    {!media.loading && <>
      <Typography variant="h3" gutterBottom component="h3">{media.result.title} <IconButton>
        <EditIcon />
      </IconButton>
      </Typography>

      <Video
        source={`${axios.defaults.baseURL}/media/${params.id}`}
        type={media.result.type}
        poster={`${axios.defaults.baseURL}/media/${params.id}/thumbnail`}
      />
      <VideoGenres genres={media.result.genres.map(prop('name'))} videoId={params.id}
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
    </>}
  </Container>
}