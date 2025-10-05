<script setup>
import ClienteService from '@/service/ClienteService';
import { FilterMatchMode } from '@primevue/core/api';
import { useToast } from 'primevue/usetoast';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useErrorHandler } from '@/utils/errorHandler';

const router = useRouter();
const toast = useToast();
const { handleError } = useErrorHandler();
const clienteService = new ClienteService();

onMounted(() => {
    loadClientes();
});

const dt = ref();
const clientes = ref([]);
const deleteClienteDialog = ref(false);
const deleteClientesDialog = ref(false);
const cliente = ref({});
const selectedClientes = ref();
const filters = ref({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS }
});
const loading = ref(false);

const loadClientes = async () => {
    loading.value = true;
    try {
        const data = await clienteService.getClientes();
        clientes.value = data;
    } catch (error) {
        handleError(error);
    } finally {
        loading.value = false;
    }
};

const openNew = () => {
    router.push('/clientes/novo');
};

const editCliente = (prod) => {
    router.push(`/clientes/${prod.id}/editar`);
};

const confirmDeleteCliente = (prod) => {
    cliente.value = prod;
    deleteClienteDialog.value = true;
};

const deleteCliente = async () => {
    try {
        await clienteService.deleteCliente(cliente.value.id);
        clientes.value = clientes.value.filter((val) => val.id !== cliente.value.id);
        deleteClienteDialog.value = false;
        cliente.value = {};
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Cliente excluído com sucesso', life: 3000 });
    } catch (error) {
        handleError(error);
    }
};

const exportCSV = () => {
    dt.value.exportCSV();
};

const confirmDeleteSelected = () => {
    deleteClientesDialog.value = true;
};

const deleteSelectedClientes = async () => {
    try {
        const deletePromises = selectedClientes.value.map(prod => clienteService.deleteCliente(prod.id));
        await Promise.all(deletePromises);

        clientes.value = clientes.value.filter((val) => !selectedClientes.value.includes(val));
        deleteClientesDialog.value = false;
        selectedClientes.value = null;
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Clientes excluídos com sucesso', life: 3000 });
    } catch (error) {
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao excluir clientes', life: 3000 });
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
                        :disabled="!selectedClientes || !selectedClientes.length" />
            </template>

            <template #end>
                <Button label="Exportar" icon="pi pi-upload" severity="secondary" @click="exportCSV($event)" />
            </template>
        </Toolbar>

        <DataTable ref="dt"
                   v-model:selection="selectedClientes"
                   :value="clientes"
                   dataKey="id"
                   :paginator="true"
                   :rows="10"
                   :filters="filters"
                   :loading="loading"
                   paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
                   :rowsPerPageOptions="[5, 10, 25]"
                   currentPageReportTemplate="Mostrando {first} a {last} de {totalRecords} clientes">
            <template #header>
                <div class="flex flex-wrap gap-2 items-center justify-between">
                    <h4 class="m-0">Gerenciar Clientes</h4>
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
            <Column field="cpfCnpj" header="CPF/CNPJ" sortable style="min-width: 16rem"></Column>
            <Column field="razaoSocial" header="Razão Social" sortable style="min-width: 16rem"></Column>
            <Column field="contato" header="Contato" sortable style="min-width: 16rem"></Column>
            <Column field="telefone" header="Telefone" sortable style="min-width: 16rem"></Column>
            <Column field="status" header="Status" sortable style="min-width: 16rem"></Column>
            <Column :exportable="false" header="Ações" style="min-width: 12rem">
                <template #body="slotProps">
                    <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editCliente(slotProps.data)" />
                    <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteCliente(slotProps.data)" />
                </template>
            </Column>
        </DataTable>

        <Dialog v-model:visible="deleteClienteDialog" :style="{ width: '450px' }" header="Confirmar" :modal="true">
            <div class="flex items-center gap-4">
                <i class="pi pi-exclamation-triangle !text-3xl" />
                <span v-if="cliente">
                    Tem certeza que deseja excluir o cliente <b>{{ cliente.nome }}</b>?
                </span>
            </div>
            <template #footer>
                <Button label="Não" icon="pi pi-times" text @click="deleteClienteDialog = false" />
                <Button label="Sim" icon="pi pi-check" @click="deleteCliente" />
            </template>
        </Dialog>

        <Dialog v-model:visible="deleteClientesDialog" :style="{ width: '450px' }" header="Confirmar" :modal="true">
            <div class="flex items-center gap-4">
                <i class="pi pi-exclamation-triangle !text-3xl" />
                <span>Tem certeza que deseja excluir os clientes selecionados?</span>
            </div>
            <template #footer>
                <Button label="Não" icon="pi pi-times" text @click="deleteClientesDialog = false" />
                <Button label="Sim" icon="pi pi-check" text @click="deleteSelectedClientes" />
            </template>
        </Dialog>
    </div>
</template>
