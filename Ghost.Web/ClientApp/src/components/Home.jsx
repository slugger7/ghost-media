import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { VideoCard } from './VideoCard.jsx';
import { VideoCardSkeleton } from './VideoCardSkeleton.jsx';
import { Grid, Pagination } from '@mui/material';
import { useSearchParams } from 'react-router-dom';

const fetchVideos = async (page, limit) => (await axios.get(`media?page=${page - 1}&limit=${limit}`)).data

export const Home = () => {
  const [page, setPage] = useState(1)
  const [limit, setLimit] = useState(0)
  const videosPage = useAsync(fetchVideos, [page, limit])
  const [total, setTotal] = useState(0);
  const [searchParams, setSearchParams] = useSearchParams()

  useEffect(() => {
    if (!videosPage.loading && !videosPage.error) {
      setTotal(videosPage.result.total)
    }
  }, [videosPage])

  useEffect(() => {
    setLimit(searchParams.get("limit") || 48)
    setPage(searchParams.get("page") || 1)
  }, [searchParams])

  const paginationComponent = <Pagination
    color="primary"
    page={page}
    defaultPage={1}
    count={Math.ceil(total / limit)}
    showFirstButton showLastButton
    onChange={(e, newPage) => setSearchParams({ page: newPage, limit })}
  />

  return (<>
    {paginationComponent}

    <Grid container spacing={2}>
      {videosPage.loading && <Grid item xs={12} sm={6} md={4} lg={3} xl={2}><VideoCardSkeleton /></Grid>}
      {!videosPage.loading && videosPage.result?.content?.length === 0 && <span>nothing here</span>}

      {!videosPage.loading && videosPage.result?.content?.map(video => <Grid key={video._id} item xs={12} sm={6} md={4} lg={3} xl={2}>
        <VideoCard id={video._id} title={video.title} />
      </Grid>)}
    </Grid>
    {paginationComponent}
  </>)
}
