import { Typography } from '@mui/material';
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook';
import { useParams, useSearchParams } from 'react-router-dom';
import { VideoGrid } from './VideoGrid.jsx';

const fetchActor = async (name) => (await axios.get(`/actor/${encodeURIComponent(name)}`)).data
const fetchVideos = async (id, page, limit, search) => {
  const params = []
  if (page) {
    params.push(`page=${page - 1}`);
  }
  if (limit) {
    params.push(`limit=${limit}`)
  }
  if (search) {
    params.push(`search=${encodeURIComponent(search)}`)
  }

  const videosResult = await axios.get(`/media/actor/${encodeURIComponent(id)}?${params.join('&')}`)
  return videosResult.data;
}

export const Actor = () => {
  const params = useParams()
  const actorResult = useAsync(fetchActor, [params.name])
  const [page, setPage] = useState()
  const [limit, setLimit] = useState()
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const videosPage = useAsync(fetchVideos, [params.id, page, limit, search])
  const [searchParams, setSearchParams] = useSearchParams()

  useEffect(() => {
    if (!videosPage.loading && !videosPage.error) {
      setTotal(videosPage.result.total)
    }
  }, [videosPage])

  useEffect(() => {
    setLimit(parseInt(searchParams.get("limit")) || 48)
    setPage(parseInt(searchParams.get("page")) || 1)
    setSearch(decodeURIComponent(searchParams.get("search") || ''))
  }, [searchParams])

  const handleSearchChange = (searchValue) => setSearchParams({
    search: encodeURIComponent(searchValue),
    page: 0,
    limit: limit || 48
  })

  return <>
    {!actorResult.loading && <Typography variant="h4" component="h4">{actorResult.result.name}</Typography>}
    <VideoGrid
      videosPage={videosPage}
      onPageChange={(e, newPage) => setSearchParams({ page: newPage, limit, search })}
      page={page}
      count={Math.ceil(total / limit) || 1}
      search={search}
      setSearch={handleSearchChange}
    />
  </>
}