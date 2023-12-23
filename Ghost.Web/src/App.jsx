import React from 'react';
import { BrowserRouter, Routes, Route, } from 'react-router-dom';
import PropTypes from 'prop-types';

import { Layout } from './components/Layout.jsx';
import { Home } from './pages/Home.jsx';
import { Settings } from './pages/Settings.jsx'
import { AddLibrary } from './pages/AddLibrary.jsx';
import { Media } from './pages/Media.jsx'
import { Genre } from './pages/Genre.jsx'
import { Genres } from './pages/Genres.jsx'
import { Actor } from './pages/Actor.jsx'
import { Actors } from './pages/Actors.jsx'
import { Favourites } from './pages/Favourites.jsx';
import { ConvertVideo } from './pages/ConvertVideo.jsx'
import { Jobs } from './pages/Jobs.jsx';

import './styles.scss'
import { Login } from './pages/Login.jsx';
import { AuthenticationProvider } from './context/Authentication.provider.jsx';
import { SelectedVideosProvider } from './context/SelectedVideos.provider.jsx';
import { AuthenticatedRoute } from './components/AuthenticatedRoute.jsx';
import { Playlists } from './pages/Playlists.jsx';
import { CreatePlaylist } from './pages/CreatePlaylist.jsx';
import { AddVideoToPlaylist } from './pages/AddVideoToPlaylist.jsx';
import { Playlist } from './pages/Playlist.jsx';

export const App = ({ baseUrl }) => {
  return (
    <BrowserRouter basename={baseUrl}>
      <AuthenticationProvider>
        <SelectedVideosProvider>
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
              <Route path='/convert/:id?' element={<AuthenticatedRoute><ConvertVideo /></AuthenticatedRoute>} />
              <Route path='/playlists' element={<AuthenticatedRoute><Playlists /></AuthenticatedRoute>} />
              <Route path='/new-playlist' element={<AuthenticatedRoute><CreatePlaylist /></AuthenticatedRoute>} />
              <Route path='/add-video-to-playlist/:videoId' element={<AuthenticatedRoute><AddVideoToPlaylist /></AuthenticatedRoute>} />
              <Route path='playlist/:playlistId' element={<AuthenticatedRoute><Playlist /></AuthenticatedRoute>} />
              <Route path='/login' element={<Login />} />
            </Routes>
          </Layout>
        </SelectedVideosProvider>
      </AuthenticationProvider>
    </BrowserRouter>
  );
}

App.propTypes = {
  baseUrl: PropTypes.string.isRequired
}
