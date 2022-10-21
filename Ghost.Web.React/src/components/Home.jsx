import React from 'react'

import { fetchVideos } from '../services/video.service'
import { VideoView } from './VideoView.jsx'

export const Home = () => {
  return <VideoView fetchFn={fetchVideos} />
}
