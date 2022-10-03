import React, { useState, useEffect } from 'react'
import axios from 'axios'

import { VideoGrid } from './VideoGrid.jsx'
import { Sort } from './Sort.jsx'
import { constructVideoParams } from '../services/video.service'
import usePromise from '../services/use-promise.js'

const fetchFavouriteVideos = async (page, limit, search, sortBy, ascending) =>
  (
    await await axios.get(
      `/media/favourites?${constructVideoParams({
        page,
        limit,
        search,
        sortBy,
        ascending,
      })}`,
    )
  ).data

export const Favourites = () => {
  const [page, setPage] = useState(1)
  const [limit, setLimit] = useState(48)
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const [sortAscending, setSortAscending] = useState(false)
  const [sortBy, setSortBy] = useState('date-added')
  const [videosPage, error, loading] = usePromise(
    () => fetchFavouriteVideos(page, limit, search, sortBy, sortAscending),
    [page, limit, search, sortBy, sortAscending],
  )

  useEffect(() => {
    if (!loading && !error) {
      setTotal(videosPage.total)
    }
  }, [videosPage, error, loading])

  const sortComponent = (
    <Sort
      sortBy={sortBy}
      setSortBy={setSortBy}
      sortDirection={sortAscending}
      setSortDirection={setSortAscending}
    />
  )

  return (
    <VideoGrid
      videos={videosPage?.content}
      loading={loading}
      onPageChange={(e, newPage) => setPage(newPage)}
      page={page}
      count={Math.ceil(total / limit) || 1}
      search={search}
      setSearch={setSearch}
      sortComponent={sortComponent}
    />
  )
}
