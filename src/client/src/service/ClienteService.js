import axios from 'axios';

export default class ClienteService {
    
    getClientes() {
        return axios.get('/api/clientes')
            .then(res => res.data);
    }
    
    getCliente(id) {
        return axios.get(`/api/clientes/${id}`)
            .then(res => res.data);
    }
    
    saveCliente(cliente) {
        if (cliente.id) {
            return axios.put(`/api/clientes/${cliente.id}`, cliente)
                .then(res => res.data);
        } else {
            return axios.post('/api/clientes', cliente)
                .then(res => res.data);
        }
    }
    
    deleteCliente(id) {
        return axios.delete(`/api/clientes/${id}`);
    }
}