import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { useParams, useSearchParams } from 'react-router-dom'
import { mergeDeepLeft } from 'ramda'
import {
  updateSearchParamsService,
  getSearchParamsObject,
} from '../services/searchParam.service.js'
import { VideoGrid } from './VideoGrid.jsx'
import { constructVideoParams } from '../services/video.service.js'
import { Sort } from './Sort.jsx'
import { TextEdit } from './TextEdit.jsx'
import usePromise from '../services/use-promise.js'

const fetchGenre = async (name) =>
  (await axios.get(`/genre/${encodeURIComponent(name)}`)).data
const fetchVideos = async (genre, page, limit, search, sortBy, ascending) => {
  const videosResult = await axios.get(
    `/media/genre/${encodeURIComponent(genre)}?${constructVideoParams({
      page,
      limit,
      search,
      sortBy,
      ascending,
    })}`,
  )

  return videosResult.data
}
const updateGenreName = async (id, name) =>
  (await axios.put(`/genre/${id}`, { name })).data

export const Genre = () => {
  const params = useParams()
  const [genre, , loadingGenre, setGenre] = usePromise(
    () => fetchGenre(params.name),
    [params.name],
  )
  const [page, setPage] = useState()
  const [limit, setLimit] = useState()
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const [sortBy, setSortBy] = useState('title')
  const [sortAscending, setSortAscending] = useState()
  const userId = localStorage.getItem('userId')
  const [videosPage, fetchVideosError, loadingVideos] = usePromise(
    () =>
      fetchVideos(
        params.name,
        page,
        limit,
        search,
        sortBy,
        sortAscending,
        userId,
      ),
    [params.name, page, limit, search, sortBy, sortAscending, userId],
  )
  const [searchParams, setSearchParams] = useSearchParams()

  useEffect(() => {
    if (!loadingVideos && !fetchVideosError) {
      setTotal(videosPage.total)
    }
  }, [videosPage, loadingVideos, fetchVideosError])

  useEffect(() => {
    const params = getSearchParamsObject(searchParams)
    setLimit(params.limit || limit || 48)
    setPage(params.page || page || 1)
    setSearch(params.search || search)
    setSortBy(params.sortBy || sortBy)
    setSortAscending(params.ascending)
  }, [searchParams])

  const updateSearchParams = updateSearchParamsService(setSearchParams, {
    page,
    limit,
    search,
    sortBy,
    ascending: sortAscending,
  })

  const handleSearchChange = (searchValue) => {
    updateSearchParams({
      search: encodeURIComponent(searchValue),
      page: 0,
    })
  }

  const handleGenreUpdate = async (genreName) => {
    const newGenre = await updateGenreName(genre.id, genreName)
    setGenre(mergeDeepLeft({ name: newGenre.name }))
  }

  const sortComponent = (
    <Sort
      sortBy={sortBy}
      setSortBy={(sortByValue) => updateSearchParams({ sortBy: sortByValue })}
      sortDirection={sortAscending}
      setSortDirection={(sortAscendingValue) =>
        updateSearchParams({ ascending: sortAscendingValue })
      }
    />
  )

  return (
    <>
      {!loadingGenre && (
        <TextEdit text={genre.name} update={handleGenreUpdate} />
      )}
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
  )
}
