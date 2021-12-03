import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './components/App/App';
import reportWebVitals from './reportWebVitals';
import {
  RecoilRoot,
} from 'recoil';
import { BrowserRouter } from 'react-router-dom'
import 'bootstrap/dist/css/bootstrap.css';
import 'font-awesome/css/font-awesome.min.css';
import { Sugar } from 'react-preloaders2';

ReactDOM.render(
    <RecoilRoot>
      <BrowserRouter>
        <App />
        <Sugar color={'#ffffff'} background='#212529'/> 
      </BrowserRouter>
    </RecoilRoot>,
  document.getElementById('root')
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
