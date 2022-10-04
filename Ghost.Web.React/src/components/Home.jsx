import React, { useEffect, useState } from 'react'

import { VideoGrid } from './VideoGrid.jsx'
import { Sort } from './Sort.jsx'
import { fetchVideos } from '../services/video.service'
import usePromise from '../services/use-promise.js'
import watchStates from '../constants/watch-states.js'

export const Home = () => {
  const [page, setPage] = useState(1)
  const [limit] = useState(48)
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const [sortBy, setSortBy] = useState('date-added')
  const [sortAscending, setSortAscending] = useState(false)
  const [watchState, setWatchState] = useState(watchStates.unwatched)
  const [videosPage, error, loading] = usePromise(
    () => fetchVideos(page, limit, search, sortBy, sortAscending, watchState),
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
