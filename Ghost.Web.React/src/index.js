import React from 'react'
import { createRoot } from 'react-dom/client'
import { App } from './App'
import axios from 'axios'
import { head } from 'ramda'

const container = document.getElementById('root')
const root = createRoot(container)

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')
const serverUrl = [
  window.location.protocol,
  '//',
  head(window.location.host.split(':')),
  ':8080',
  '/api',
].join('')
axios.defaults.baseURL = serverUrl

root.render(<App baseUrl={baseUrl} />)
