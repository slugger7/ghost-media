import axios from 'axios'
import { head } from 'ramda'

export const setupAxios = () => {
  const serverUrl = [
    import.meta.env.VITE_APP_SERVER_URL ||
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
      const token = localStorage.getItem('token')
      if (token) {
        config.headers['Authorization'] = `Bearer ${token}`
      }

      return config
    },
    (error) => Promise.reject(error),
  )

  axios.interceptors.response.use(res => {
    return res;
  }, (error) => {
    if (+error?.response?.status === 401) {
      localStorage.clear();
      //window.location.reload();
    }
  })
}
