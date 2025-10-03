import { apiClient } from '@/config/api';

export default class CidadeService {
    
    getCidadesByUF(uf) {
        return apiClient.get(`/cidades/by-uf?uf=${uf}`)
            .then(res => res.data);
    }
    
    getCidadeById(id) {
        return apiClient.get(`/cidades/${id}`)
            .then(res => res.data);
    }
    
    getAllCidades() {
        return apiClient.get('/cidades')
            .then(res => res.data);
    }
}
