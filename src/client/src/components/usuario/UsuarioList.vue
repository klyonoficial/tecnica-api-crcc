<script setup>
import UsuarioService from '@/service/UsuarioService';
import { FilterMatchMode } from '@primevue/core/api';
import { useToast } from 'primevue/usetoast';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useErrorHandler } from '@/utils/errorHandler';

const router = useRouter();
const toast = useToast();
const { handleError } = useErrorHandler();
const usuarioService = new UsuarioService();

onMounted(() => {
    loadUsuarios();
});

const dt = ref();
const usuarios = ref([]);
const deleteUsuarioDialog = ref(false);
const deleteUsuariosDialog = ref(false);
const usuario = ref({});
const selectedUsuarios = ref();
const filters = ref({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS }
});
const loading = ref(false);

const loadUsuarios = async () => {
    loading.value = true;
    try {
        const data = await usuarioService.getUsuarios();
        usuarios.value = data;
    } catch (error) {
        handleError(error);
    } finally {
        loading.value = false;
    }
};

const openNew = () => {
    router.push('/usuarios/novo');
};

const editUsuario = (prod) => {
    router.push(`/usuarios/${prod.id}/editar`);
};

const confirmDeleteUsuario = (prod) => {
    usuario.value = prod;
    deleteUsuarioDialog.value = true;
};

const deleteUsuario = async () => {
    try {
        await usuarioService.deleteUsuario(usuario.value.id);
        usuarios.value = usuarios.value.filter((val) => val.id !== usuario.value.id);
        deleteUsuarioDialog.value = false;
        usuario.value = {};
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Usuario excluído com sucesso', life: 3000 });
    } catch (error) {
        handleError(error);
    }
};

const exportCSV = () => {
    dt.value.exportCSV();
};

const confirmDeleteSelected = () => {
    deleteUsuariosDialog.value = true;
};

const deleteSelectedUsuarios = async () => {
    try {
        const deletePromises = selectedUsuarios.value.map(prod => usuarioService.deleteUsuario(prod.id));
        await Promise.all(deletePromises);

        usuarios.value = usuarios.value.filter((val) => !selectedUsuarios.value.includes(val));
        deleteUsuariosDialog.value = false;
        selectedUsuarios.value = null;
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Usuarios excluídos com sucesso', life: 3000 });
    } catch (error) {
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao excluir usuarios', life: 3000 });
    }
};
</script>

<template>
    <div class="card">
        <Toolbar class="mb-6">
            <template #start>
                <Button label="Novo" icon="pi pi-plus" severity="secondary" class="mr-2" @click="openNew" />
                <Button label="Excluir"
                        icon="pi pi-trash"
                        severity="secondary"
                        @click="confirmDeleteSelected"
                        :disabled="!selectedUsuarios || !selectedUsuarios.length" />
            </template>

            <template #end>
                <Button label="Exportar" icon="pi pi-upload" severity="secondary" @click="exportCSV($event)" />
            </template>
        </Toolbar>

        <DataTable ref="dt"
                   v-model:selection="selectedUsuarios"
                   :value="usuarios"
                   dataKey="id"
                   :paginator="true"
                   :rows="10"
                   :filters="filters"
                   :loading="loading"
                   paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
                   :rowsPerPageOptions="[5, 10, 25]"
                   currentPageReportTemplate="Mostrando {first} a {last} de {totalRecords} usuarios">
            <template #header>
                <div class="flex flex-wrap gap-2 items-center justify-between">
                    <h4 class="m-0">Gerenciar Usuários</h4>
                    <IconField>
                        <InputIcon>
                            <i class="pi pi-search" />
                        </InputIcon>
                        <InputText v-model="filters['global'].value" placeholder="Buscar..." />
                    </IconField>
                </div>
            </template>

            <Column selectionMode="multiple" style="width: 3rem" :exportable="false"></Column>
            <Column field="id" header="ID" sortable style="min-width: 8rem"></Column>
            <Column field="nome" header="Nome" sortable style="min-width: 16rem"></Column>
            <Column field="email" header="Email" sortable style="min-width: 16rem"></Column>
            <Column field="perfil" header="Perfil" sortable style="min-width: 16rem"></Column>
            <Column field="status" header="Status" sortable style="min-width: 16rem"></Column>
            <Column :exportable="false" header="Ações" style="min-width: 12rem">
                <template #body="slotProps">
                    <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editUsuario(slotProps.data)" />
                    <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteUsuario(slotProps.data)" />
                </template>
            </Column>
        </DataTable>

        <Dialog v-model:visible="deleteUsuarioDialog" :style="{ width: '450px' }" header="Confirmar" :modal="true">
            <div class="flex items-center gap-4">
                <i class="pi pi-exclamation-triangle !text-3xl" />
                <span v-if="usuario">
                    Tem certeza que deseja excluir o usuario <b>{{ usuario.nome }}</b>?
                </span>
            </div>
            <template #footer>
                <Button label="Não" icon="pi pi-times" text @click="deleteUsuarioDialog = false" />
                <Button label="Sim" icon="pi pi-check" @click="deleteUsuario" />
            </template>
        </Dialog>

        <Dialog v-model:visible="deleteUsuariosDialog" :style="{ width: '450px' }" header="Confirmar" :modal="true">
            <div class="flex items-center gap-4">
                <i class="pi pi-exclamation-triangle !text-3xl" />
                <span>Tem certeza que deseja excluir os usuarios selecionados?</span>
            </div>
            <template #footer>
                <Button label="Não" icon="pi pi-times" text @click="deleteUsuariosDialog = false" />
                <Button label="Sim" icon="pi pi-check" text @click="deleteSelectedUsuarios" />
            </template>
        </Dialog>
    </div>
</template>
