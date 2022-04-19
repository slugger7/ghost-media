import { Typography } from '@mui/material';
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook';
import { useParams, useSearchParams } from 'react-router-dom';
import { VideoGrid } from './VideoGrid.jsx';

const fetchActor = async (name) => (await axios.get(`/actor/${encodeURIComponent(name)}`)).data
const fetchVideos = async (id, page, limit) => (await axios.get(`/media/actor/${encodeURIComponent(id)}?page=${page - 1}&limit=${limit}`)).data

export const Actor = () => {
  const params = useParams()
  const actorResult = useAsync(fetchActor, [params.name])
  const [page, setPage] = useState(1)
  const [limit, setLimit] = useState(1)
  const [total, setTotal] = useState(0)
  const videosPage = useAsync(fetchVideos, [params.id, page, limit])
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

  return <>
    {!actorResult.loading && <Typography variant="h4" component="h4">{actorResult.result.name}</Typography>}
    <VideoGrid
      videosPage={videosPage}
      onPageChange={(e, newPage) => setSearchParams({ page: newPage, limit })}
      page={page}
      count={Math.ceil(total / limit) || 1}
    />
  </>
}