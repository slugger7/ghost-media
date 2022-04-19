import axios from 'axios'

export const fetchGenres = async () => (await axios.get(`/genre`)).data
