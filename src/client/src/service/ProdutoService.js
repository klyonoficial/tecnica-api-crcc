import { apiClient } from '@/config/api';

class ProdutoService {
    
    getProdutos() {
        return apiClient.get('/produtos')
            .then(res => res.data);
    }
    
    getProduto(id) {
        return apiClient.get(`/produtos/${id}`)
            .then(res => res.data);
    }
    
    saveProduto(produto) {
        if (produto.id) {
            return apiClient.put('/produtos', produto)
                .then(res => res.data);
        } else {
            return apiClient.post('/produtos', produto)
                .then(res => res.data);
        }
    }
    
    deleteProduto(id) {
        return apiClient.delete(`/produtos/${id}`);
    }
}

export default ProdutoService;