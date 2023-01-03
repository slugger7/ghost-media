import axios from 'axios'
import { head } from 'ramda'

export const setupAxios = () => {
  const serverUrl = [
    process.env.REACT_APP_SERVER_URL ||
    [
      window.location.protocol,
      '//',
      head(window.location.host.split(':')),
      ':5120',
    ].join(''),
    '/api',
  ].join('')
  axios.defaults.baseURL = serverUrl

  axios.interceptors.request.use(
    (config) => {
      const userId = localStorage.getItem('userId')
      const token = localStorage.getItem('token')
      if (userId) {
        config.headers['User-Id'] = userId //replace this with only JWT being sent back
      }
      if (token) {
        config.headers['Authorization'] = `Bearer ${token}`
      }

      return config
    },
    (error) => Promise.reject(error),
  )

  axios.interceptors.response.use(res => {
    console.log({ stauts: res.status })
    if (+res.status === 401) {
      console.log('clearing localstorage')
      localStorage.clear();
    }
    return res;
  }, (error, ...args) => {
    console.log("Response error", { error, args })
    return Promise.reject(error)
  })
}
