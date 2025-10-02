import { apiClient } from '@/config/api';

export default class ClienteService {
    
    getClientes(status = null, filtro = null) {
        const params = new URLSearchParams();
        if (status) params.append('status', status);
        if (filtro) params.append('filtro', filtro);
        
        const queryString = params.toString();
        const url = `/clientes${queryString ? `?${queryString}` : ''}`;
        
        return apiClient.get(url)
            .then(res => res.data);
    }
    
    getCliente(id) {
        return apiClient.get(`/clientes/${id}`)
            .then(res => res.data);
    }
    
    saveCliente(cliente) {
        if (cliente.id) {
            return apiClient.put('/clientes', cliente)
                .then(res => res.data);
        } else {
            return apiClient.post('/clientes', cliente)
                .then(res => res.data);
        }
    }
    
    deleteCliente(id) {
        return apiClient.delete(`/clientes/${id}`);
    }
    
    aprovarCliente(id) {
        return apiClient.put(`/clientes/${id}/aprovar`);
    }
    
    reprovarCliente(id) {
        return apiClient.put(`/clientes/${id}/reprovar`);
    }
    
    inativarCliente(id) {
        return apiClient.put(`/clientes/${id}/inativar`);
    }
}