import axios from 'axios';

export default class UsuarioService {
    getUsuarios() {
        return axios.get('/api/usuarios').then(res => res.data);
    }

    getUsuario(id) {
        return axios.get(`/api/usuarios/${id}`).then(res => res.data);
    }

    saveUsuario(usuario) {
        // Monta o DTO conforme esperado pelo backend
        const dto = {
            id: usuario.id ?? 0, // Garante que id nunca será nulo
            nome: usuario.nome,
            email: usuario.email,
            ativo: usuario.ativo,
            deveAlterarSenha: usuario.deveAlterarSenha
        };
        if (usuario.id) {
            return axios.put('/api/usuarios', dto).then(res => res.data);
        } else {
            return axios.post('/api/usuarios', dto).then(res => res.data);
        }
    }

    deleteUsuario(id) {
        return axios.delete(`/api/usuarios/${id}`);
    }

    resetSenha(id) {
        return axios.post(`/api/usuarios/${id}/reset-senha`);
    }
}
