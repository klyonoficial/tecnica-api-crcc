import axios from 'axios';

export default class UsuarioService {
    getUsuarios() {
        return axios.get('/api/usuarios').then(res => res.data);
    }

    getUsuario(id) {
        return axios.get(`/api/usuarios/${id}`).then(res => res.data);
    }

    saveUsuario(usuario) {
        if (usuario.id) {
            return axios.put(`/api/usuarios/${usuario.id}`, usuario).then(res => res.data);
        } else {
            return axios.post('/api/usuarios', usuario).then(res => res.data);
        }
    }

    resetSenha(id) {
        return axios.post(`/api/usuarios/${id}/reset-senha`);
    }
}
