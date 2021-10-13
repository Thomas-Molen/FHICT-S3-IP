import './App.css';
import { withRouter } from 'react-router-dom'
import Routes from '../router'
import 'bootstrap/dist/css/bootstrap.min.css';
import { useEffect } from 'react';

function App() {

  useEffect(() => RefreshJWT())

  async function RefreshJWT() 
  {
    await fetch('https://localhost:5001/api/User/renew-token', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
      },
      credentials: "include",
    })
      .then(response => response.json())
      .then(data => {
        console.log(data);
      })
      .catch(error => {
        console.error('Error:', error);
      });
  }

  return (
    <div>
      <Routes />
    </div>
  );
}



export default withRouter(App);
