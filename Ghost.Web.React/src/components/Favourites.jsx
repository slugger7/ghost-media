import React from 'react'
import axios from 'axios'
import { constructVideoParams } from '../services/video.service'
import { VideoView } from './VideoView.jsx'

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
  return <VideoView fetchFn={fetchFavouriteVideos} />
}
