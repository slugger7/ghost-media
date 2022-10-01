import React, { useEffect, useState } from 'react';
import { useSearchParams } from 'react-router-dom';

import { VideoGrid } from './VideoGrid.jsx';
import { Sort } from './Sort.jsx'
import { fetchVideos } from '../services/video.service'
import { updateSearchParamsService, getSearchParamsObject } from '../services/searchParam.service.js';
import usePromise from '../services/use-promise.js';


export const Home = () => {
  const [page, setPage] = useState()
  const [limit, setLimit] = useState()
  const [search, setSearch] = useState('');
  const [total, setTotal] = useState(0);
  const [searchParams, setSearchParams] = useSearchParams()
  const [sortBy, setSortBy] = useState('date-added');
  const [sortAscending, setSortAscending] = useState(true);
  const {result: videos, error, loading} = usePromise(
    () => fetchVideos(page, limit, search, sortBy, sortAscending),
    [page, limit, search, sortBy, sortAscending]
  );

  useEffect(() => {
    if (!loading && !error) {
      setTotal(videos.total)
    }
  }, [videos, error, loading])

  useEffect(() => {
    const params = getSearchParamsObject(searchParams);
    setLimit(params.limit || limit || 48)
    setPage(params.page || page || 1)
    setSearch(params.search || search)
    setSortBy(params.sortBy || sortBy)
    setSortAscending(params.ascending === undefined ? sortBy : params.ascending)
  }, [searchParams])

  const updateSearchParams = updateSearchParamsService(
    setSearchParams,
    { page, limit, search, sortBy, ascending: sortAscending }
  );

  const handleSearchChange = (searchValue) => {
    updateSearchParams({
      search: encodeURIComponent(searchValue),
      page: 0,
    })
  }

  const sortComponent = <Sort
    sortBy={sortBy}
    setSortBy={(sortByValue) => updateSearchParams({ sortBy: sortByValue })}
    sortDirection={sortAscending}
    setSortDirection={(sortAscendingValue) => updateSearchParams({ ascending: sortAscendingValue })} />

  return (<>
    <VideoGrid
      videosPage={{result: videos, error, loading}}
      onPageChange={(e, newPage) => updateSearchParams({ page: newPage })}
      page={page}
      count={Math.ceil(total / limit) || 1}
      search={search}
      setSearch={handleSearchChange}
      sortComponent={sortComponent}
    />
  </>)
}
