import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import { Layout } from './components/Layout.jsx';
import { Home } from './components/Home.jsx';
import { Libraries } from './components/Libraries.jsx';
import { AddLibrary } from './components/AddLibrary.jsx';
import { Media } from './components/Media.jsx'
import { Genre } from './components/Genre.jsx'
import { GenresView } from './components/GenresView.jsx'
import { Actor } from './components/Actor.jsx'
import { Actors } from './components/Actors.jsx'

import './styles.scss'

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

export const App = () => {
  return (
    <BrowserRouter basename={baseUrl}>
      <Layout>
        <Routes>
          <Route exact path='/' element={<Home />} />
          <Route path='/libraries' element={<Libraries />} />
          <Route path='/libraries/add' element={<AddLibrary />} />
          <Route path='/media/:id' element={<Media />} />
          <Route path='/genres' element={<GenresView />} />
          <Route path='/genres/:name' element={<Genre />} />
          <Route path='/actors/:id/:name' element={<Actor />} />
          <Route path='/actors' element={<Actors />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}
