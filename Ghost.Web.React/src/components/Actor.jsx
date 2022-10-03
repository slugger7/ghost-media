import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { useParams, useSearchParams } from 'react-router-dom'
import { VideoGrid } from './VideoGrid.jsx'
import { Sort } from './Sort.jsx'
import {
  updateSearchParamsService,
  getSearchParamsObject,
} from '../services/searchParam.service.js'
import { constructVideoParams } from '../services/video.service.js'
import { TextEdit } from './TextEdit.jsx'
import { mergeDeepLeft, mergeDeepRight } from 'ramda'
import { Stack } from '@mui/material'
import { FavouriteIconButton } from './FavouriteIconButton.jsx'
import usePromise from '../services/use-promise.js'

const fetchActor = async (name) =>
  (await axios.get(`/actor/${encodeURIComponent(name)}`)).data
const fetchVideos = async (id, page, limit, search, sortBy, ascending) => {
  const videosResult = await axios.get(
    `/media/actor/${encodeURIComponent(id)}?${constructVideoParams({
      page,
      limit,
      search,
      sortBy,
      ascending,
    })}`,
  )
  return videosResult.data
}
const updateActorName = async (id, name) =>
  (await axios.put(`/actor/${id}`, { name })).data

export const Actor = () => {
  const params = useParams()
  const [actorsPage, , loadingActor, setActor] = usePromise(
    () => fetchActor(params.name),
    [params.name],
  )
  const [page, setPage] = useState()
  const [limit, setLimit] = useState()
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const [sortAscending, setSortAscending] = useState()
  const [sortBy, setSortBy] = useState('title')
  const userId = localStorage.getItem('userId')
  const [videosPage, fetchVideoError, loadingVideos] = usePromise(
    () => fetchVideos(params.id, page, limit, search, sortBy, sortAscending),
    [params.id, page, limit, search, sortBy, sortAscending],
  )
  const [searchParams, setSearchParams] = useSearchParams()

  useEffect(() => {
    if (!loadingVideos && !fetchVideoError) {
      setTotal(videosPage.total)
    }
  }, [videosPage])

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

  const handleToggleFavourite = async () =>
    (
      await axios.put(
        `/user/${localStorage.getItem('userId')}/actor/${actorsPage.id}`,
      )
    ).data

  const updateActor = (val) => setActor(mergeDeepLeft(val))

  const handleUpdateActorName = async (name) => {
    const newActor = await updateActorName(actorsPage.id, name)
    updateActor({ name: newActor.name })
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
      {!loadingActor && actorsPage && (
        <Stack direction="row" spacing={1}>
          <FavouriteIconButton
            state={actorsPage.favourite}
            toggleFn={handleToggleFavourite}
            update={(favourite) => updateActor({ favourite })}
            id={actorsPage.id}
          />
          <TextEdit text={actorsPage.name} update={handleUpdateActorName} />
        </Stack>
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
