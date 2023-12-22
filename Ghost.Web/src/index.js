import React from 'react'
import { createRoot } from 'react-dom/client'
import { App } from './App'
import { setupAxios } from './services/axios.setup'

const container = document.getElementById('root')
const root = createRoot(container)

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href')
setupAxios(baseUrl)

root.render(<App baseUrl={baseUrl} />)
