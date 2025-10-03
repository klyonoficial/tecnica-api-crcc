import { apiClient } from '@/config/api';

export default class UsuarioService {
    getUsuarios() {
        return apiClient.get('/usuarios').then(res => res.data);
    }

    getUsuario(id) {
        return apiClient.get(`/usuarios/${id}`).then(res => res.data);
    }

    saveUsuario(usuario) {
        // Monta o DTO conforme esperado pelo backend
        const dto = {
            id: usuario.id ?? 0, // Garante que id nunca ser� nulo
            nome: usuario.nome,
            login: usuario.login,
            email: usuario.email,
            ativo: usuario.ativo,
            deveAlterarSenha: usuario.deveAlterarSenha
        };
        if (usuario.id) {
            return apiClient.put('/usuarios', dto).then(res => res.data);
        } else {
            return apiClient.post('/usuarios', dto).then(res => res.data);
        }
    }

    deleteUsuario(id) {
        return apiClient.delete(`/usuarios/${id}`);
    }

    resetSenha(id) {
        return apiClient.post(`/usuarios/${id}/reset-senha`);
    }
}
