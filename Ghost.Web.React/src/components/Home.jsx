import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { useSearchParams } from 'react-router-dom';

import { VideoGrid } from './VideoGrid.jsx';
import { Sort } from './Sort.jsx'

const fetchVideos = async (page, limit, search) => {
  const params = [];
  if (page) {
    params.push(`page=${page - 1}`)
  }
  if (limit) {
    params.push(`limit=${limit}`)
  }
  if (search) {
    params.push(`search=${encodeURIComponent(search)}`)
  }
  const videosResult = await axios.get(`media?${params.join('&')}`)

  return videosResult.data;
}

export const Home = () => {
  const [page, setPage] = useState()
  const [limit, setLimit] = useState()
  const [search, setSearch] = useState('');
  const videosPage = useAsync(fetchVideos, [page, limit, search])
  const [total, setTotal] = useState(0);
  const [searchParams, setSearchParams] = useSearchParams()
  const [sortBy, setSortBy] = useState('title');
  const [sortDirection, setSortDirection] = useState(true);

  useEffect(() => {
    if (!videosPage.loading && !videosPage.error) {
      setTotal(videosPage.result.total)
    }
  }, [videosPage])

  useEffect(() => {
    setLimit(parseInt(searchParams.get("limit")) || limit || 48)
    setPage(parseInt(searchParams.get("page")) || page || 1)
    setSearch(decodeURIComponent(searchParams.get("search") || search || ''))
  }, [searchParams])

  const handleSearchChange = (searchValue) => {
    setSearchParams({
      search: encodeURIComponent(searchValue),
      page: 0,
      limit: limit || 48
    })
    setSearch(searchValue);
    setPage(0);
    setLimit(limit || 48);
  }

  const sortComponent = <Sort
    sortBy={sortBy}
    setSortBy={setSortBy}
    sortDirection={sortDirection}
    setSortDirection={setSortDirection} />

  return (<>
    <VideoGrid
      videosPage={videosPage}
      onPageChange={(e, newPage) => setSearchParams({ page: newPage, limit, search })}
      page={page}
      count={Math.ceil(total / limit) || 1}
      search={search}
      setSearch={handleSearchChange}
      sortComponent={sortComponent}
    />
  </>)
}
