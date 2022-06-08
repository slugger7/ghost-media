import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
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

import './styles.scss'

export const App = ({ baseUrl }) => {
  return (
    <BrowserRouter basename={baseUrl}>
      <Layout>
        <Routes>
          <Route exact path='/' element={<Home />} />
          <Route path='/settings' element={<Settings />} />
          <Route path='/libraries/add' element={<AddLibrary />} />
          <Route path='/media/:id/:title' element={<Media />} />
          <Route path='/genres' element={<Genres />} />
          <Route path='/genres/:name' element={<Genre />} />
          <Route path='/actors/:id/:name' element={<Actor />} />
          <Route path='/actors' element={<Actors />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}

App.propTypes = {
  baseUrl: PropTypes.string.isRequired
}
