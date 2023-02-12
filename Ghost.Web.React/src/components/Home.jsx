import React from 'react'

import { fetchVideos, fetchRandomMedia } from '../services/video.service'
import { VideoView } from './VideoView.jsx'

export const Home = () => {
  return <VideoView fetchFn={fetchVideos} fetchRandomVideoFn={fetchRandomMedia} />
}
