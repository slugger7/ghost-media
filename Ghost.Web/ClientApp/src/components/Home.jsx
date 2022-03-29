import React, { useState } from 'react';
import { useAsync } from 'react-async-hook'
import axios from 'axios'

import { Video } from './Video.jsx'

const fetchVideos = async () => (await axios.get("media")).data

export const Home = () => {
  const videosPage = useAsync(fetchVideos, [])
  const [currentVideo, setCurrentVideo] = useState()

  return (<>
    {currentVideo && <Video
      source={`${axios.defaults.baseURL}/media/${currentVideo._id}`}
      type={currentVideo.type}
    />}
    {videosPage.loading && <span>loading ...</span>}
    {!videosPage.loading && videosPage.result?.content?.map(video =>
      <button key={video._id}
        onClick={() => setCurrentVideo(video)}
      >{video.title}</button>)}
  </>)
}
