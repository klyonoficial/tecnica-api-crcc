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
        const dto = {
            id: usuario.id ?? 0, // Garante que id nunca ser� nulo
            nome: produto.nome,
            fabricanteId: produto.fabricanteId
        };
        
        if (produto.id) {
            return apiClient.put('/produtos', dto).then(res => res.data);
        } else {
            return apiClient.post('/produtos', dto).then(res => res.data);
        }
    }
    
    deleteProduto(id) {
        return apiClient.delete(`/produtos/${id}`);
    }
}

export default ProdutoService;