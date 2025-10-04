<template>
  <div class="grid">
    <div class="col-12">
      <div class="card">
        <h5>Cadastro de Usuário</h5>
        <form @submit.prevent="onSubmit" class="p-fluid">
          <div class="field">
            <label for="email">Email</label>
            <input id="email" v-model="usuario.email" type="email" class="p-inputtext w-full" required />
          </div>
          <div class="field">
            <label for="nome">Nome</label>
            <input id="nome" v-model="usuario.nome" type="text" class="p-inputtext w-full" required />
          </div>
          <div class="field">
            <label for="perfil">Perfil</label>
            <Dropdown id="perfil"
              v-model="usuario.perfil"
              :options="perfis"
              optionLabel="label"
              optionValue="value"
              placeholder="Selecione"
              class="w-full"
              required
            />
          </div>
          <div class="field">
            <label for="senha">Senha</label>
            <input id="senha" v-model="usuario.senha" type="password" class="p-inputtext w-full" :required="!usuario.id" />
          </div>
          <div class="field-checkbox">
            <input type="checkbox" id="ativo" v-model="usuario.ativo" />
            <label for="ativo">Ativo</label>
          </div>
          <div class="flex justify-content-end gap-2 mt-4">
            <Button type="submit" label="Salvar" icon="pi pi-save" class="p-button-success" />
            <Button v-if="usuario.id" type="button" label="Redefinir Senha" icon="pi pi-refresh" class="p-button-warning" @click="onResetSenha" />
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import UsuarioService from '@/service/UsuarioService'
import { Usuario } from '@/models/usuario.model'
import Dropdown from 'primevue/dropdown';
import Button from 'primevue/button';

export default {
  components: { Dropdown, Button },
  props: {
    usuarioId: {
      type: Number,
      default: null
    }
  },
  data() {
    return {
      usuario: new Usuario(),
      perfis: [
        { label: 'Administrador', value: 'Administrador' },
        { label: 'Usuário', value: 'Usuário' },
        { label: 'Gerente', value: 'Gerente' }
      ],
      service: new UsuarioService()
    }
  },
  mounted() {
    if (this.usuarioId) {
      this.service.getUsuario(this.usuarioId).then(data => {
        this.usuario = data;
      });
    }
  },
  methods: {
    onSubmit() {
      this.service.saveUsuario(this.usuario).then(() => {
          this.$emit('saved')
          this.usuario = {
              email: '',
              nome: ''             
          }
          alert('Usuário salvo com sucesso!')
      })
    },
    onResetSenha() {
      this.service.resetSenha(this.usuario.id).then(() => {
        alert('Senha redefinida com sucesso!')
      })
    }
  }
}
</script>

<style scoped>
.mt-4 {
  margin-top: 1.5rem;
}
.gap-2 > * + * {
  margin-left: 0.75rem;
}
.field {
  margin-bottom: 0.5cm;
}
.field-checkbox label {
  margin-left: 0.5rem;
}
</style>
