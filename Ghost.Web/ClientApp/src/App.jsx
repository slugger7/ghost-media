import React from 'react';
import { Routes, Route } from 'react-router-dom';
import { Layout } from './components/Layout.jsx';
import { Home } from './components/Home.jsx';

export const App = () => {
  return (
    <Layout>
      <Routes>
        <Route exact path='/' element={<Home />} />
      </Routes>
    </Layout>
  );
}
