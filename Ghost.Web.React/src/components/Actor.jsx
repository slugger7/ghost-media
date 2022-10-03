import axios from 'axios'
import React, { useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'
import { VideoGrid } from './VideoGrid.jsx'
import { Sort } from './Sort.jsx'
import { constructVideoParams } from '../services/video.service.js'
import { TextEdit } from './TextEdit.jsx'
import { mergeDeepLeft } from 'ramda'
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
  const [page, setPage] = useState(1)
  const [limit, setLimit] = useState(2)
  const [search, setSearch] = useState('')
  const [total, setTotal] = useState(0)
  const [sortAscending, setSortAscending] = useState(false)
  const [sortBy, setSortBy] = useState('date-added')
  const [videosPage, fetchVideoError, loadingVideos] = usePromise(
    () => fetchVideos(params.id, page, limit, search, sortBy, sortAscending),
    [params.id, page, limit, search, sortBy, sortAscending],
  )

  useEffect(() => {
    if (!loadingVideos && !fetchVideoError) {
      setTotal(videosPage.total)
    }
  }, [videosPage])

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
      setSortBy={setSortBy}
      sortDirection={sortAscending}
      setSortDirection={setSortAscending}
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
