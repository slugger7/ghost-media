import React from 'react';
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { VideoCard } from './VideoCard.jsx';
import { Grid } from '@mui/material';

const fetchVideos = async () => (await axios.get("media")).data

export const Home = () => {
  const videosPage = useAsync(fetchVideos, [])

  return (<Grid container spacing={2}>
    {videosPage.loading && <span>loading ...</span>}
    {!videosPage.loading && videosPage.result?.content?.map(video => <Grid key={video._id} item xs={12} sm={6} md={4} lg={3} xl={2}>
      <VideoCard id={video._id} title={video.title} />
    </Grid>)}
  </Grid>)
}
