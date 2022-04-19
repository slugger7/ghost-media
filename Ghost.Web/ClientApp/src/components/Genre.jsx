import { Typography } from '@mui/material';
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook';
import { useParams, useSearchParams } from 'react-router-dom';
import { VideoGrid } from './VideoGrid.jsx';

const fetchGenre = async (name) => (await axios.get(`/genre/${encodeURIComponent(name)}`)).data
const fetchVideos = async (genre, page, limit) => (await axios.get(`/media/genre/${encodeURIComponent(genre)}?page=${page - 1}&limit=${limit}`)).data

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

  return <>
    {!genreResult.loading && <Typography variant="h4" component="h4">{genreResult.result.name}</Typography>}
    <VideoGrid
      videosPage={videosPage}
      onPageChange={(e, newPage) => setSearchParams({ page: newPage, limit })}
      page={page}
      count={Math.ceil(total / limit) || 1}
    />
  </>
}