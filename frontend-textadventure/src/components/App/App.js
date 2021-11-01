import './App.css';
import { withRouter } from 'react-router-dom'
import Routes from '../router'
import 'bootstrap/dist/css/bootstrap.min.css';

import authHook from '../../actions/useAuthHook'
function App() {
  authHook();
  return (
    <div>
      <Routes />
    </div>
  );
}

export default withRouter(App);
