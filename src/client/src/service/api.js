import axios from 'axios';

const api = axios.create({
  baseURL: window.VITE_API_URL || 'http://localhost:5000', 
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
    Accept: 'application/json',
  },
});

export default api;
