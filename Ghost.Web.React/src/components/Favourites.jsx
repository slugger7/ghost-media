import React, { useState, useEffect } from 'react'
import axios from 'axios'

import { VideoGrid } from './VideoGrid.jsx'
import { Sort } from './Sort.jsx'
import { constructVideoParams } from '../services/video.service'
import usePromise from '../services/use-promise.js'
import watchStates from '../constants/watch-states.js'

const fetchFavouriteVideos = async (
  page,
  limit,
  search,
  sortBy,
  ascending,
  watchState,
) =>
  (
    await await axios.get(
      `/media/favourites?${constructVideoParams({
        page,
        limit,
        search,
        sortBy,
        ascending,
        watchState,
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
  const [watchState, setWatchState] = useState(watchStates.all)
  const [videosPage, error, loading] = usePromise(
    () =>
      fetchFavouriteVideos(
        page,
        limit,
        search,
        sortBy,
        sortAscending,
        watchState,
      ),
    [page, limit, search, sortBy, sortAscending, watchState],
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
      watchState={watchState}
      setWatchState={setWatchState}
    />
  )
}
