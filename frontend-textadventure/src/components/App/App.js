import React from 'react';
import './App.css';
import { withRouter } from 'react-router-dom'
import Routes from '../router'
import 'bootstrap/dist/css/bootstrap.min.css';
import authHook from '../../actions/useAuthHook'

function App() {
  authHook();
  return (
    <React.Fragment>
      <Routes />
    </React.Fragment>
  );
}

export default withRouter(App);
