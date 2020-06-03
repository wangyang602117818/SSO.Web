import React from 'react';
import ReactDOM from 'react-dom';
import funtools from 'fun-tools';
import { baseURL, cookieName } from './config/http';
import App from './components/App';

import * as serviceWorker from './serviceWorker';
import './css/index.css';

funtools.authorize(baseURL, cookieName);

ReactDOM.render(
  <App/>
  ,document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();