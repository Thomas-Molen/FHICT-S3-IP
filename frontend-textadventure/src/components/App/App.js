import React from 'react';
import './App.css';
import { withRouter } from 'react-router-dom'
import Routes from '../router'
import { useAuthHook } from '../../helpers';

function App() {
  useAuthHook();
  return (
    <>
      <Routes />
    </>
  );
}

export default withRouter(App);
