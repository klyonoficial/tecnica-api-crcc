import axios from 'axios';

// Configuração da API
export const API_CONFIG = {
    BASE_URL: import.meta.env.VITE_API_URL || 'http://localhost:5275/api'
};

// Configuração da instância do axios
const apiClient = axios.create({
    baseURL: API_CONFIG.BASE_URL,
    timeout: 10000,
    headers: {
        'Content-Type': 'application/json'
    }
});

// Interceptor para requisições
apiClient.interceptors.request.use(
    (config) => {
        // Aqui você pode adicionar tokens de autenticação, logs, etc.
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Interceptor para respostas
apiClient.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        // O erro será tratado pelos componentes usando useErrorHandler
        // Aqui podemos fazer logs ou tratamentos globais se necessário
        console.error('API Error:', error);
        return Promise.reject(error);
    }
);

export { apiClient };
export default API_CONFIG;
