import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';

import { Layout } from './components/Layout.jsx';
import { Home } from './components/Home.jsx';
import { Libraries } from './components/Libraries.jsx';
import { AddLibrary } from './components/AddLibrary.jsx';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');

export const App = () => {
  return (
    <BrowserRouter basename={baseUrl}>
      <Layout>
        <Routes>
          <Route exact path='/' element={<Home />} />
          <Route path='/libraries' element={<Libraries />} />
          <Route path='/libraries/add' element={<AddLibrary />} />
        </Routes>
      </Layout>
    </BrowserRouter>
  );
}
