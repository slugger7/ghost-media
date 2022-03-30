import React from 'react';
import { useAsync } from 'react-async-hook'
import { ButtonLink } from './ButtonLink.jsx'
import axios from 'axios'

const fetchVideos = async () => (await axios.get("media")).data

export const Home = () => {
  const videosPage = useAsync(fetchVideos, [])

  return (<>
    {videosPage.loading && <span>loading ...</span>}
    {!videosPage.loading && videosPage.result?.content?.map(video =>
      <ButtonLink key={video._id} variant="contained" to={`/media/${video._id}`}
      >{video.title}</ButtonLink>)}
  </>)
}
