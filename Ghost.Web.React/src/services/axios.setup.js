import axios from 'axios'
import { head } from 'ramda'

export const setupAxios = (baseUrl) => {
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
      if (userId) {
        config.headers['User-Id'] = userId
      }

      return config
    },
    (error) => Promise.reject(error),
  )
}
