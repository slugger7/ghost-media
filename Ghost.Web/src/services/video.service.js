import axios from 'axios'

export const fetchVideos = async ({
  page,
  limit,
  search,
  sortBy,
  ascending,
  watchState,
  genres,
}) => {
  const videosResult = await axios.get(
    `media?${constructVideoParams({
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

export const relateVideos = async ({ id, relateToId }) => {
  const videosResult = await axios.put(`media/${id}/relations/${relateToId}`)

  return videosResult.data
}

export const fetchRandomMedia = async (params) => (await axios.get(`/media/random?${constructVideoParams(params)}`)).data

export const constructVideoParams = ({
  page,
  limit,
  search,
  sortBy,
  ascending,
  watchState,
  genres,
}) => {
  const params = []
  if (page) {
    params.push(`page=${page - 1}`)
  }
  if (limit) {
    params.push(`limit=${limit}`)
  }
  if (search) {
    params.push(`search=${encodeURIComponent(search)}`)
  }
  if (sortBy) {
    params.push(`sortBy=${encodeURIComponent(sortBy)}`)
  }
  if (ascending !== undefined) {
    params.push(`ascending=${ascending}`)
  }
  if (watchState !== undefined && watchState !== null) {
    params.push(`watchState=${watchState}`)
  }
  if (genres !== undefined && genres !== null && genres.length > 0) {
    params.push(
      genres.map((genre) => `genres=${encodeURIComponent(genre)}`).join('&'),
    )
  }
  return params.join('&')
}

export const generateVideoUrl = (id) => `${axios.defaults.baseURL}/media/${id}`

export const toggleFavourite = async (videoId) =>
  (await axios.put(`/user/${localStorage.getItem('userId')}/video/${videoId}`))
    .data
