import axios from 'axios'

export const fetchVideos = async (page, limit, search, sortBy, ascending) => {
  const videosResult = await axios.get(
    `media?${constructVideoParams({ page, limit, search, sortBy, ascending })}`,
  )

  return videosResult.data
}

export const constructVideoParams = ({
  page,
  limit,
  search,
  sortBy,
  ascending,
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
  return params.join('&')
}
