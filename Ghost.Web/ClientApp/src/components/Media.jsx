import React from 'react'
import { useParams } from 'react-router-dom'
import axios from 'axios'

import { Video } from './Video.jsx'

export const Media = () => {
  const params = useParams()

  return <>
    <Video
      source={`${axios.defaults.baseURL}/media/${params.id}`}
      type="video/mp4"
    />
  </>
}