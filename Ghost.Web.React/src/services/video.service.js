import axios from 'axios'

export const fetchVideos = async (
  page,
  limit,
  search,
  sortBy,
  ascending,
  watchState,
) => {
  const videosResult = await axios.get(
    `media?${constructVideoParams({
      page,
      limit,
      search,
      sortBy,
      ascending,
      watchState,
    })}`,
  )

  return videosResult.data
}

export const constructVideoParams = ({
  page,
  limit,
  search,
  sortBy,
  ascending,
  watchState,
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
    params.push(`watchState=${watchState.value}`)
  }
  return params.join('&')
}

export const generateVideoUrl = (id) => `${axios.defaults.baseURL}/media/${id}`

export const toggleFavourite = async (videoId) =>
  (await axios.put(`/user/${localStorage.getItem('userId')}/video/${videoId}`))
    .data
export const resetProgress = async (videoId) =>
  (await axios.put(`/media/${videoId}/reset-progress`)).data
