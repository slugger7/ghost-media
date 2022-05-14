import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook'
import axios from 'axios'
import { useSearchParams } from 'react-router-dom';

import { VideoGrid } from './VideoGrid.jsx';
import { Sort } from './Sort.jsx'
import { ascend, mergeDeepRight } from 'ramda';

const fetchVideos = async (page, limit, search, sortBy, ascending) => {
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
  if (sortBy) {
    params.push(`sortBy=${encodeURIComponent(sortBy)}`)
  }
  if (ascending !== undefined) {
    params.push(`ascending=${ascending}`)
  }
  const videosResult = await axios.get(`media?${params.join('&')}`)

  return videosResult.data;
}

export const Home = () => {
  const [page, setPage] = useState()
  const [limit, setLimit] = useState()
  const [search, setSearch] = useState('');
  const [total, setTotal] = useState(0);
  const [searchParams, setSearchParams] = useSearchParams()
  const [sortBy, setSortBy] = useState('title');
  const [sortAscending, setSortAscending] = useState();
  const videosPage = useAsync(fetchVideos, [page, limit, search, sortBy, sortAscending])

  useEffect(() => {
    if (!videosPage.loading && !videosPage.error) {
      setTotal(videosPage.result.total)
    }
  }, [videosPage])

  useEffect(() => {
    setLimit(parseInt(searchParams.get("limit")) || limit || 48)
    setPage(parseInt(searchParams.get("page")) || page || 1)
    setSearch(decodeURIComponent(searchParams.get("search") || search || ''))
    setSortBy(decodeURIComponent(searchParams.get("sortBy") || sortBy))
    const ascending = searchParams.get("ascending")
    setSortAscending((ascending !== "false" && ascending !== "true") || ascending === "true")
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

  const updateSearchParams = (newSearchParams) => setSearchParams(
    mergeDeepRight(
      { page, limit, search, sortBy, ascending: sortAscending },
      newSearchParams
    ))

  const sortComponent = <Sort
    sortBy={sortBy}
    setSortBy={(sortByValue) => updateSearchParams({ sortBy: sortByValue })}
    sortDirection={sortAscending}
    setSortDirection={(sortAscendingValue) => updateSearchParams({ ascending: sortAscendingValue })} />

  return (<>
    <VideoGrid
      videosPage={videosPage}
      onPageChange={(e, newPage) => updateSearchParams({ page: newPage })}
      page={page}
      count={Math.ceil(total / limit) || 1}
      search={search}
      setSearch={handleSearchChange}
      sortComponent={sortComponent}
    />
  </>)
}
