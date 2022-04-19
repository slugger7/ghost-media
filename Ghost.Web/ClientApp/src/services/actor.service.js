import axios from 'axios'

export const fetchActors = async () => (await axios.get(`/actor`)).data
