import React from 'react'
import { createRoot } from 'react-dom/client'
import { App } from './App'
// import * as serviceWorkerRegistration from './serviceWorkerRegistration'
// import reportWebVitals from './reportWebVitals'
import axios from 'axios'
import { head } from 'ramda'

const container = document.getElementById('root')
const root = createRoot(container)

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')
const serverUrl = [
  window.location.protocol,
  '//',
  head(window.location.host.split(':')),
  ':7110',
  '/api',
].join('')
axios.defaults.baseURL = serverUrl

root.render(<App baseUrl={baseUrl} />)

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
//serviceWorkerRegistration.unregister()

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
//reportWebVitals()
