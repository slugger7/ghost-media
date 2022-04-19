import { Grid, Pagination, Typography } from '@mui/material';
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook';
import { useParams, useSearchParams } from 'react-router-dom';
import { VideoCard } from './VideoCard.jsx';
import { VideoCardSkeleton } from './VideoCardSkeleton.jsx';

const fetchGenre = async (name) => (await axios.get(`/genre/${name}`)).data;
const fetchVideos = async (genre, page, limit) => (await axios.get(`/media/genre/${genre}`)).data

export const Genre = () => {
  const params = useParams()
  const genreResult = useAsync(fetchGenre, [params.name])
  const [page, setPage] = useState(1)
  const [limit, setLimit] = useState(1)
  const [total, setTotal] = useState(0)
  const videosPage = useAsync(fetchVideos, [params.name, page, limit])
  const [searchParams, setSearchParams] = useSearchParams()

  useEffect(() => {
    if (!videosPage.loading && !videosPage.error) {
      setTotal(videosPage.result.total)
    }
  }, [videosPage])

  useEffect(() => {
    setLimit(parseInt(searchParams.get("limit")) || 48)
    setPage(parseInt(searchParams.get("page")) || 1)
  }, [searchParams])

  const paginationComponent = <Pagination
    color="primary"
    page={page}
    defaultPage={1}
    count={Math.ceil(total / limit) || 1}
    showFirstButton showLastButton
    onChange={(e, newPage) => setSearchParams({ page: newPage, limit })}
  />

  return <>
    {genreResult.loading}
    {!genreResult.loading && <Typography variant="h4" component="h4">{genreResult.result.name}</Typography>}
    {paginationComponent}
    <Grid container spacing={2}>
      {videosPage.loading && <Grid item xs={12} sm={6} md={4} lg={3} xl={2}><VideoCardSkeleton /></Grid>}
      {!videosPage.loading && videosPage.result?.content?.length === 0 && <span>nothing here</span>}

      {!videosPage.loading && videosPage.result?.content?.map(video => <Grid key={video._id} item xs={12} sm={6} md={4} lg={3} xl={2}>
        <VideoCard id={video._id} title={video.title} />
      </Grid>)}
    </Grid>
    {paginationComponent}
  </>
}