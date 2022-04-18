import React from 'react';
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { VideoCard } from './VideoCard.jsx';

const fetchVideos = async () => (await axios.get("media")).data

export const Home = () => {
  const videosPage = useAsync(fetchVideos, [])

  return (<>
    {videosPage.loading && <span>loading ...</span>}
    {!videosPage.loading && videosPage.result?.content?.map(video => <VideoCard key={video._id} id={video._id} title={video.title} />)}
  </>)
}
