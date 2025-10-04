import { apiClient } from '@/config/api';

class FabricanteService {
    
    getFabricantes() {
        return apiClient.get('/fabricantes')
            .then(res => res.data);
    }
    
    getFabricante(id) {
        return apiClient.get(`/fabricantes/${id}`)
            .then(res => res.data);
    }
    
    saveFabricante(fabricante) {
        const dto = {
            id: fabricante.id ?? 0, // Garante que id nunca ser� nulo
            nome: fabricante.nome
        };
        
        if (fabricante.id) {
            return apiClient.put('/fabricantes', dto).then(res => res.data);
        } else {
            return apiClient.post('/fabricantes', dto).then(res => res.data);
        }
    }    
}

export default FabricanteService;