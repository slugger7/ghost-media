import React, { useState, useEffect } from 'react'
import { useSearchParams } from 'react-router-dom'
import axios from 'axios';

import { VideoGrid } from './VideoGrid.jsx';
import { Sort } from './Sort.jsx'
import { constructVideoParams } from '../services/video.service';
import { updateSearchParamsService, getSearchParamsObject } from '../services/searchParam.service.js';
import usePromise from '../services/use-promise.js';

const fetchFavouriteVideos = async (page, limit, search, sortBy, ascending) =>
  (await (await axios.get(`/media/favourites?${constructVideoParams({ page, limit, search, sortBy, ascending })}`))).data

export const Favourites = () => {
  const [page, setPage] = useState()
  const [limit, setLimit] = useState()
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const [sortAscending, setSortAscending] = useState();
  const [sortBy, setSortBy] = useState('title')
  const [videosPage, fetchVideosError, loadingVideos] = usePromise(
    () => fetchFavouriteVideos(page, limit, search, sortBy, sortAscending), 
    [page, limit, search, sortBy, sortAscending]
  );
  const [searchParams, setSearchParams] = useSearchParams()

  useEffect(() => {
    if (!loadingVideos && !fetchVideosError) {
      setTotal(videosPage.total)
    }
  }, [videosPage, loadingVideos, fetchVideosError])

  useEffect(() => {
    const params = getSearchParamsObject(searchParams);
    setLimit(params.limit || limit || 48)
    setPage(params.page || page || 1)
    setSearch(params.search || search || '')
    setSortBy(params.sortBy || sortBy)
    setSortAscending(params.ascending)
  }, [searchParams])

  const updateSearchParams = updateSearchParamsService(setSearchParams, { page, limit, search, sortBy, ascending: sortAscending })

  const handleSearchChange = (searchValue) => {
    updateSearchParams({
      search: encodeURIComponent(searchValue),
      page: 0
    })
  }

  const sortComponent = <Sort
    sortBy={sortBy}
    setSortBy={(sortByValue) => updateSearchParams({ sortBy: sortByValue })}
    sortDirection={sortAscending}
    setSortDirection={(sortAscendingValue) => updateSearchParams({ ascending: sortAscendingValue })} />

  return <>
    <VideoGrid
      videos={videosPage?.content}
      loading={loadingVideos}
      onPageChange={(e, newPage) => updateSearchParams({ page: newPage })}
      page={page}
      count={Math.ceil(total / limit) || 1}
      search={search}
      setSearch={handleSearchChange}
      sortComponent={sortComponent}
    />
  </>
}