import './App.css';
import { NavigationComponent, FooterComponent, IntroductionPageComponent } from '../';
import { withRouter } from 'react-router-dom'
import Routes from '../router'
import { useHistory } from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.min.css';

function App({ location }) {
  console.log(location)

  return (
    <div>
      <IntroductionPageComponent />
      <Routes />
      <FooterComponent />
    </div>
  );
}

export default withRouter(App);
