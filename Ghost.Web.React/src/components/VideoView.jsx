import React, { useEffect, useState } from 'react'
import PropTypes from 'prop-types'
import { VideoGrid } from './VideoGrid'

import usePromise from '../services/use-promise.js'
import watchStates from '../constants/watch-states.js'

export const VideoView = ({ fetchFn }) => {
  const [page, setPage] = useState(1)
  const [limit] = useState(48)
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const [sortBy, setSortBy] = useState('date-added')
  const [sortAscending, setSortAscending] = useState(false)
  const [watchState, setWatchState] = useState(watchStates.unwatched)
  const [videosPage, error, loading] = usePromise(
    () =>
      fetchFn({
        page,
        limit,
        search,
        sortBy,
        ascending: sortAscending,
        watchState,
      }),
    [page, limit, search, sortBy, sortAscending, watchState],
  )

  useEffect(() => {
    if (!loading && !error) {
      setTotal(videosPage.total)
    }
  }, [videosPage, error, loading])

  return (
    <VideoGrid
      videos={videosPage?.content}
      loading={loading}
      onPageChange={(e, newPage) => setPage(newPage)}
      page={page}
      count={Math.ceil(total / limit) || 1}
      search={search}
      setSearch={setSearch}
      watchState={watchState}
      setWatchState={setWatchState}
      sortBy={sortBy}
      setSortBy={setSortBy}
      sortDirection={sortAscending}
      setSortDirection={setSortAscending}
    />
  )
}

VideoView.propTypes = {
  fetchFn: PropTypes.func.isRequired,
}
