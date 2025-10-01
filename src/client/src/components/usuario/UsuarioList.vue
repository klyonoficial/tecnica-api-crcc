<template>
  <div class="card">
    <DataTable
      :value="usuarios"
      :paginator="true"
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
          <Button icon="pi pi-pencil" class="p-button-rounded p-button-info p-button-sm mr-2" @click="$emit('editar', data)" />
          <Button icon="pi pi-trash" class="p-button-rounded p-button-danger p-button-sm" @click="$emit('remover', data)" />
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

export default {
  components: { DataTable, Column, Button, InputText, IconField, InputIcon },
  props: {
    usuarios: {
      type: Array,
      default: () => []
    }
  },
  data() {
    return {
      filters: {
        global: { value: null, matchMode: 'contains' }
      }
    }
  },
  methods: {
    clearFilter() {
      this.filters = {
        global: { value: null, matchMode: 'contains' }
      };
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
