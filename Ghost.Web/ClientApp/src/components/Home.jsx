import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { VideoCard } from './VideoCard.jsx';
import { Grid, Pagination } from '@mui/material';
import { useSearchParams } from 'react-router-dom';

const fetchVideos = async (page, limit) => (await axios.get(`media?page=${page}&limit=${limit}`)).data

export const Home = () => {
  const [page, setPage] = useState(0)
  const [limit, setLimit] = useState(12)
  const videosPage = useAsync(fetchVideos, [page, limit])
  const [total, setTotal] = useState(0);
  const [searchParams, setSearchParams] = useSearchParams()

  useEffect(() => {
    if (!videosPage.loading && !videosPage.error) {
      setTotal(videosPage.result.total)
    }
  }, [videosPage])

  useEffect(() => {
    setLimit(searchParams.get("limit") || 12)
    setPage(searchParams.get("page") || 0)
  }, [searchParams])

  return (<>
    <Pagination count={Math.ceil(total / limit)} showFirstButton showLastButton onChange={(e, newPage) => setSearchParams({ page: newPage - 1, limit })} />
    <Grid container spacing={2}>
      {videosPage.loading && <span>loading ...</span>}
      {!videosPage.loading && videosPage.result?.content?.map(video => <Grid key={video._id} item xs={12} sm={6} md={4} lg={3} xl={2}>
        <VideoCard id={video._id} title={video.title} />
      </Grid>)}
    </Grid>
    <Pagination count={Math.ceil(total / limit)} showFirstButton showLastButton onChange={(e, newPage) => setSearchParams({ page: newPage - 1, limit })} />
  </>)
}
