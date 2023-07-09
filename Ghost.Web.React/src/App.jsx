import React from 'react';
import { BrowserRouter, Routes, Route, } from 'react-router-dom';
import PropTypes from 'prop-types';

import { Layout } from './components/Layout.jsx';
import { Home } from './components/Home.jsx';
import { Settings } from './components/Settings.jsx'
import { AddLibrary } from './components/AddLibrary.jsx';
import { Media } from './components/Media.jsx'
import { Genre } from './components/Genre.jsx'
import { Genres } from './components/Genres.jsx'
import { Actor } from './components/Actor.jsx'
import { Actors } from './components/Actors.jsx'
import { Favourites } from './components/Favourites.jsx';

import './styles.scss'
import { Login } from './components/Login.jsx';
import { AuthenticationProvider } from './context/Authentication.provider.jsx';
import { AuthenticatedRoute } from './components/AuthenticatedRoute.jsx';
import { Jobs } from './components/Jobs.jsx';

export const App = ({ baseUrl }) => {
  return (
    <BrowserRouter basename={baseUrl}>
      <AuthenticationProvider>
        <Layout>
          <Routes>
            <Route exact path='/' element={<AuthenticatedRoute><Home /></AuthenticatedRoute>} />
            <Route path='/settings' element={<AuthenticatedRoute><Settings /></AuthenticatedRoute>} />
            <Route path='/libraries/add' element={<AuthenticatedRoute><AddLibrary /></AuthenticatedRoute>} />
            <Route path='/media/:id/:title' element={<AuthenticatedRoute><Media /></AuthenticatedRoute>} />
            <Route path='/genres' element={<AuthenticatedRoute><Genres /></AuthenticatedRoute>} />
            <Route path='/genres/:name' element={<AuthenticatedRoute><Genre /></AuthenticatedRoute>} />
            <Route path='/actors/:id/:name' element={<AuthenticatedRoute><Actor /></AuthenticatedRoute>} />
            <Route path='/actors' element={<AuthenticatedRoute><Actors /></AuthenticatedRoute>} />
            <Route path='/favourites' element={<AuthenticatedRoute><Favourites /></AuthenticatedRoute>} />
            <Route path='/jobs' element={<AuthenticatedRoute><Jobs /></AuthenticatedRoute>} />
            <Route path='/login' element={<Login />} />
          </Routes>
        </Layout>
      </AuthenticationProvider>
    </BrowserRouter>
  );
}

App.propTypes = {
  baseUrl: PropTypes.string.isRequired
}
