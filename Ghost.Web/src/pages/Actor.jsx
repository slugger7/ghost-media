import axios from 'axios'
import React from 'react'
import { useParams } from 'react-router-dom'
import { constructVideoParams } from '../services/video.service.js'
import { TextEdit } from '../components/TextEdit.jsx'
import { mergeDeepLeft } from 'ramda'
import { Stack } from '@mui/material'
import { FavouriteIconButton } from '../components/FavouriteIconButton.jsx'
import usePromise from '../services/use-promise.js'
import { VideoView } from '../components/VideoView.jsx'

const fetchActor = async (name) =>
  (await axios.get(`/actor/${encodeURIComponent(name)}`)).data
const fetchVideos =
  (id) =>
    async ({ page, limit, search, sortBy, ascending, watchState, genres }) => {
      const videosResult = await axios.get(
        `/media/actor/${encodeURIComponent(id)}?${constructVideoParams({
          page,
          limit,
          search,
          sortBy,
          ascending,
          watchState,
          genres,
        })}`,
      )
      return videosResult.data
    }

const fetchRandomVideo = (id) => async (params) => {
  const videoResult = await axios.get(`/media/actor/${encodeURIComponent(id)}/random?${constructVideoParams(params)}`)

  return videoResult.data;
}
const updateActorName = async (id, name) =>
  (await axios.put(`/actor/${id}`, { name })).data

export const Actor = () => {
  const params = useParams()
  const [actorsPage, , loadingActor, setActor] = usePromise(
    () => fetchActor(params.name),
    [params.name],
  )

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
      <VideoView fetchFn={fetchVideos(params.id)} fetchRandomVideoFn={fetchRandomVideo(params.id)} />
    </>
  )
}
