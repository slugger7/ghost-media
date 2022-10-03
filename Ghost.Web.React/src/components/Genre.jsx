import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { mergeDeepLeft } from 'ramda'
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
  const [page, setPage] = useState(1)
  const [limit, setLimit] = useState(48)
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const [sortBy, setSortBy] = useState('date-added')
  const [sortAscending, setSortAscending] = useState(false)
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

  useEffect(() => {
    if (!loadingVideos && !fetchVideosError) {
      setTotal(videosPage.total)
    }
  }, [videosPage, loadingVideos, fetchVideosError])

  const handleGenreUpdate = async (genreName) => {
    const newGenre = await updateGenreName(genre.id, genreName)
    setGenre(mergeDeepLeft({ name: newGenre.name }))
  }

  const sortComponent = (
    <Sort
      sortBy={sortBy}
      setSortBy={setSortBy}
      sortDirection={sortAscending}
      setSortDirection={setSortAscending}
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
        onPageChange={(e, newPage) => setPage(newPage)}
        page={page}
        count={Math.ceil(total / limit) || 1}
        search={search}
        setSearch={setSearch}
        sortComponent={sortComponent}
      />
    </>
  )
}
