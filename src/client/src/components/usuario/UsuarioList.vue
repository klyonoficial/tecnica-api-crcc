<template>
  <div class="card">
    <DataTable
      :value="usuarios || []"
      :paginator="usuarios && usuarios.length > 0"
      :rows="10"
      dataKey="id"
      :rowHover="true"
      showGridlines
      class="p-datatable-striped p-datatable-gridlines"
      :globalFilterFields="['nome', 'email', 'perfil']"
      :filters="filters"
      v-model:filters="filters"
    >
      <template #header>
        <div class="flex justify-between">
          <Button type="button" icon="pi pi-filter-slash" label="Limpar" outlined @click="clearFilter" />
          <IconField class="w-30">
            <InputIcon>
              <i class="pi pi-search" />
            </InputIcon>
            <InputText v-model="filters['global'].value" placeholder="Procurar" />
          </IconField>
        </div>
      </template>
      <template #empty> Nenhum usuário encontrado. </template>
      <Column field="nome" header="Nome" style="min-width: 12rem">
        <template #body="{ data }">
          {{ data.nome }}
        </template>
      </Column>
      <Column field="email" header="Email" style="min-width: 12rem">
        <template #body="{ data }">
          {{ data.email }}
        </template>
      </Column>
      <Column field="perfil" header="Perfil" style="min-width: 10rem">
        <template #body="{ data }">
          {{ data.perfil }}
        </template>
      </Column>
      <Column field="ativo" header="Status" style="min-width: 8rem">
        <template #body="{ data }">
          <span :class="data.ativo ? 'text-success' : 'text-danger'">
            {{ data.ativo ? 'Ativo' : 'Inativo' }}
          </span>
        </template>
      </Column>
      <Column header="Ações" style="min-width: 8rem" bodyClass="text-center">
        <template #body="{ data }">
          <Button icon="pi pi-pencil" class="p-button-rounded p-button-info p-button-sm mr-2" @click="editarUsuario(data)" />
          <Button icon="pi pi-trash" class="p-button-rounded p-button-danger p-button-sm" @click="removerUsuario(data)" />
        </template>
      </Column>
    </DataTable>
  </div>
</template>

<script>
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import IconField from 'primevue/iconfield';
import InputIcon from 'primevue/inputicon';
import UsuarioService from '@/service/UsuarioService';

export default {
  components: { DataTable, Column, Button, InputText, IconField, InputIcon },
  data() {
    return {
      usuarios: null,
      filters: {
        global: { value: null, matchMode: 'contains' }
      },
      usuarioService: new UsuarioService()
    }
  },
  mounted() {
    this.carregarUsuarios();
  },
  methods: {
    carregarUsuarios() {
      this.usuarioService.getUsuarios()
        .then(data => {
          this.usuarios = Array.isArray(data) ? data : [];
        })
        .catch(() => {
          this.usuarios = [];
        });
    },
    clearFilter() {
      this.filters = {
        global: { value: null, matchMode: 'contains' }
      };
    },
    editarUsuario(usuario) {
      this.$router.push({ name: 'usuario-editar', params: { id: usuario.id } });
    },
    removerUsuario(usuario) {
      if (confirm('Deseja remover este usuário?')) {
        this.usuarioService.deleteUsuario(usuario.id).then(() => {
          this.carregarUsuarios();
        });
      }
    }
  }
}
</script>

<style scoped>
.text-success {
  color: #22c55e;
}
.text-danger {
  color: #ef4444;
}
.mr-2 {
  margin-right: 0.5rem;
}
.w-30 {
  width: 220px;
  min-width: 120px;
}
.text-center {
  text-align: center;
}
</style>
