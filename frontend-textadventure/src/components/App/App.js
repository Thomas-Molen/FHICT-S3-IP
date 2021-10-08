import './App.css';
import { withRouter } from 'react-router-dom'
import Routes from '../router'
import 'bootstrap/dist/css/bootstrap.min.css';

function App({ location }) {
  console.log(location)

  return (
    <div>
      <Routes />
    </div>
  );
}

export default withRouter(App);
