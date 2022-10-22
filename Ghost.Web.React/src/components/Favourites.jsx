import React from 'react'
import axios from 'axios'
import { Box } from '@mui/material'

import { constructVideoParams } from '../services/video.service'
import { VideoView } from './VideoView.jsx'
import { ChipSkeleton } from './ChipSkeleton.jsx'
import usePromise from '../services/use-promise'
import { ActorChip } from './ActorChip'

const fetchFavouriteVideos = async ({
  page,
  limit,
  search,
  sortBy,
  ascending,
  watchState,
  genres,
}) =>
  (
    await axios.get(
      `/media/favourites?${constructVideoParams({
        page,
        limit,
        search,
        sortBy,
        ascending,
        watchState,
        genres,
      })}`,
    )
  ).data

const fetchFavouriteActors = async (userId) =>
  (await axios.get('/actor/favourites')).data

export const Favourites = () => {
  const [favouriteActors, , loadingFavouriteActors] =
    usePromise(fetchFavouriteActors)

  return (
    <VideoView fetchFn={fetchFavouriteVideos}>
      <Box sx={{ mb: 1 }}>
        {loadingFavouriteActors && <ChipSkeleton />}
        {!loadingFavouriteActors &&
          favouriteActors.map((actor) => (
            <ActorChip actor={actor} key={actor.id} />
          ))}
      </Box>
    </VideoView>
  )
}
