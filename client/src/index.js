import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import "./styles/index.css";
import './styles/input.css'; 
import 'semantic-ui-css/semantic.min.css';

const root = ReactDOM.createRoot(document.getElementById("root"));
root.render(
  <React.StrictMode>
      <App />
  </React.StrictMode>
);