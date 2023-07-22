import axios from 'axios'

export const fetchMedia = async (id) => (await axios.get(`/media/${id}/info`)).data
export const convertVideo = async (id, convertRequest) => (await axios.post(`/media/${id}/convert`, convertRequest))