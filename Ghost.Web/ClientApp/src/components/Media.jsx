import React from 'react'
import { useParams } from 'react-router-dom'
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { IconButton, Typography } from '@mui/material'
import EditIcon from '@mui/icons-material/Edit';

import { Video } from './Video.jsx'

const fetchMedia = async (id) => (await axios.get(`/media/${id}/info`)).data

export const Media = () => {
  const params = useParams()
  const media = useAsync(fetchMedia, [params.id])

  return <>
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
    </>}
  </>
}