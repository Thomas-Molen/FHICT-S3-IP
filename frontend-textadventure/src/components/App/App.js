import './App.css';
import { NavigationComponent, FooterComponent } from '../';
import { withRouter } from 'react-router-dom'
import Routes from '../router'
import { useHistory } from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.min.css';

function App({ location }) {
  console.log(location)

  return (
    <div>
      <NavigationComponent />
      <Routes />
      <FooterComponent />
    </div>
  );
}

export default withRouter(App);
