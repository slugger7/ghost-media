import { Typography } from '@mui/material';
import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook';
import { useParams, useSearchParams } from 'react-router-dom';
import { VideoGrid } from './VideoGrid.jsx';
import { Sort } from './Sort.jsx'
import { updateSearchParamsService, getSearchParamsObject } from '../services/searchParam.service.js';
import { constructVideoParams } from '../services/video.service.js';

const fetchActor = async (name) => (await axios.get(`/actor/${encodeURIComponent(name)}`)).data
const fetchVideos = async (id, page, limit, search, sortBy, ascending) => {
  const videosResult = await axios.get(`/media/actor/${encodeURIComponent(id)}?${constructVideoParams({ page, limit, search, sortBy, ascending })}`)
  return videosResult.data;
}
export const Actor = () => {
  const params = useParams()
  const actorResult = useAsync(fetchActor, [params.name])
  const [page, setPage] = useState()
  const [limit, setLimit] = useState()
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const [sortAscending, setSortAscending] = useState();
  const [sortBy, setSortBy] = useState('title');
  const videosPage = useAsync(fetchVideos, [params.id, page, limit, search, sortBy, sortAscending])
  const [searchParams, setSearchParams] = useSearchParams()

  useEffect(() => {
    if (!videosPage.loading && !videosPage.error) {
      setTotal(videosPage.result.total)
    }
  }, [videosPage])

  useEffect(() => {
    const params = getSearchParamsObject(searchParams);
    setLimit(params.limit || limit || 48)
    setPage(params.page || page || 1)
    setSearch(params.search || search)
    setSortBy(params.sortBy || sortBy)
    setSortAscending(params.ascending)
  }, [searchParams])

  const handleSearchChange = (searchValue) => setSearchParams({
    search: encodeURIComponent(searchValue),
    page: 0,
    limit: limit || 48
  })

  const updateSearchParams = updateSearchParamsService(setSearchParams, { page, limit, search, sortBy, ascending: sortAscending })

  const sortComponent = <Sort
    sortBy={sortBy}
    setSortBy={(sortByValue) => updateSearchParams({ sortBy: sortByValue })}
    sortDirection={sortAscending}
    setSortDirection={(sortAscendingValue) => updateSearchParams({ ascending: sortAscendingValue })} />

  return <>
    {!actorResult.loading && <Typography variant="h4" component="h4">{actorResult.result.name}</Typography>}
    <VideoGrid
      videosPage={videosPage}
      onPageChange={(e, newPage) => updateSearchParams({ page: newPage })}
      page={page}
      count={Math.ceil(total / limit) || 1}
      search={search}
      setSearch={handleSearchChange}
      sortComponent={sortComponent}
    />
  </>
}