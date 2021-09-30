import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css'
import Lobby from './components/ExampleComponent/Lobby.js';
import { HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { useState } from 'react';

const App = () => {
  const [connection, setConnection] = useState();
  const joinGame = async (user, room) => {
    try
    {
      const connection = new HubConnectionBuilder()
      .withUrl("https://localhost:5001/game")
      .configureLogging(LogLevel.Information)
      .build();

      connection.on("ReceiveMessage", (user, message) => {
        console.log('message received: ', message);
      });

      await connection.start();
      await connection.invoke("JoinGame", {user, room});
      setConnection(connection);
    } 
    catch (ex) 
    {
      console.log(ex);
    }
  }

  return <div className='app'>
    <h2>MyGame</h2>
    <hr className='line' />
    <Lobby joinGame={joinGame} />
  </div>
}

export default App;
