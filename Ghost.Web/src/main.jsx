import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App.jsx'
import './styles.scss'
import { setupAxios } from './services/axios.setup.js'

setupAxios();

ReactDOM.createRoot(document.getElementById('root')).render(
  <React.StrictMode>
    <App baseUrl={'/'} />
  </React.StrictMode>,
)
