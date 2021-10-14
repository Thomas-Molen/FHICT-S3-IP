import './App.css';
import { withRouter } from 'react-router-dom'
import Routes from '../router'
import 'bootstrap/dist/css/bootstrap.min.css';

import { atom } from 'recoil';
import { JWTMiddleware } from '../JWTMiddleware';

function App() {

  return (
    <div>
      <JWTMiddleware />
      <Routes />
    </div>
  );
}

export default withRouter(App);
