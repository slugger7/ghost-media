import axios from 'axios';
import React, { useEffect, useState } from 'react';
import { useAsync } from 'react-async-hook';
import { useParams, useSearchParams } from 'react-router-dom';
import { mergeDeepRight } from 'ramda';
import { updateSearchParamsService, getSearchParamsObject } from '../services/searchParam.service.js';
import { VideoGrid } from './VideoGrid.jsx';
import { constructVideoParams } from '../services/video.service.js'
import { Sort } from './Sort.jsx'
import { TextEdit } from './TextEdit.jsx'

const fetchGenre = async (name) => (await axios.get(`/genre/${encodeURIComponent(name)}`)).data
const fetchVideos = async (genre, page, limit, search, sortBy, ascending) => {
  const videosResult = await axios.get(`/media/genre/${encodeURIComponent(genre)}?${constructVideoParams({ page, limit, search, sortBy, ascending })}`)

  return videosResult.data;
}
const updateGenreName = async (id, name) => (await axios.put(`/genre/${id}`, { name })).data

export const Genre = () => {
  const params = useParams()
  const genreResult = useAsync(fetchGenre, [params.name])
  const [page, setPage] = useState()
  const [limit, setLimit] = useState()
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const [sortBy, setSortBy] = useState('title')
  const [sortAscending, setSortAscending] = useState()
  const videosPage = useAsync(fetchVideos, [params.name, page, limit, search, sortBy, sortAscending])
  const [searchParams, setSearchParams] = useSearchParams()

  useEffect(() => {
    if (!videosPage.loading && !videosPage.error) {
      setTotal(videosPage.result.total)
    }
  }, [videosPage])

  useEffect(() => {
    const params = getSearchParamsObject(searchParams);
    setLimit(params.limit || limit || 48)
    setPage(params.page || page || 1)
    setSearch(params.search || search)
    setSortBy(params.sortBy || sortBy)
    setSortAscending(params.ascending)
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

  const handleGenreUpdate = async (genre) => {
    const newGenre = await updateGenreName(genreResult.result._id, genre)
    genreResult.set(mergeDeepRight(genreResult, { result: { name: newGenre.name } }))
  }

  const sortComponent = <Sort
    sortBy={sortBy}
    setSortBy={(sortByValue) => updateSearchParams({ sortBy: sortByValue })}
    sortDirection={sortAscending}
    setSortDirection={(sortAscendingValue) => updateSearchParams({ ascending: sortAscendingValue })} />

  return <>
    {!genreResult.loading && <TextEdit text={genreResult.result.name} update={handleGenreUpdate} />}
    <VideoGrid
      videosPage={videosPage}
      onPageChange={(e, newPage) => updateSearchParams({ page: newPage })}
      page={page}
      count={Math.ceil(total / limit) || 1}
      search={search}
      setSearch={handleSearchChange}
      sortComponent={sortComponent}
    />
  </>
}