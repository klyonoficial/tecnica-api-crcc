<script setup>
import FabricanteService from '@/service/FabricanteService';
import { FilterMatchMode } from '@primevue/core/api';
import { useToast } from 'primevue/usetoast';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useErrorHandler } from '@/utils/errorHandler';

const router = useRouter();
const toast = useToast();
const { handleError } = useErrorHandler();
const fabricanteService = new FabricanteService();

onMounted(() => {
    loadFabricantes();
});

const dt = ref();
const fabricantes = ref([]);
const deleteFabricanteDialog = ref(false);
const deleteFabricantesDialog = ref(false);
const fabricante = ref({});
const selectedFabricantes = ref();
const filters = ref({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS }
});
const loading = ref(false);

const loadFabricantes = async () => {
    loading.value = true;
    try {
        const data = await fabricanteService.getFabricantes();
        fabricantes.value = data;
    } catch (error) {
        handleError(error);
    } finally {
        loading.value = false;
    }
};

const openNew = () => {
    router.push('/fabricantes/novo');
};

const editFabricante = (prod) => {
    router.push(`/fabricantes/${prod.id}/editar`);
};

const confirmDeleteFabricante = (prod) => {
    fabricante.value = prod;
    deleteFabricanteDialog.value = true;
};

const deleteFabricante = async () => {
    try {
        await fabricanteService.deleteFabricante(fabricante.value.id);
        fabricantes.value = fabricantes.value.filter((val) => val.id !== fabricante.value.id);
        deleteFabricanteDialog.value = false;
        fabricante.value = {};
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Fabricante excluído com sucesso', life: 3000 });
    } catch (error) {
        handleError(error);
    }
};

const exportCSV = () => {
    dt.value.exportCSV();
};

const confirmDeleteSelected = () => {
    deleteFabricantesDialog.value = true;
};

const deleteSelectedFabricantes = async () => {
    try {
        const deletePromises = selectedFabricantes.value.map(prod => fabricanteService.deleteFabricante(prod.id));
        await Promise.all(deletePromises);
        
        fabricantes.value = fabricantes.value.filter((val) => !selectedFabricantes.value.includes(val));
        deleteFabricantesDialog.value = false;
        selectedFabricantes.value = null;
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Fabricantes excluídos com sucesso', life: 3000 });
    } catch (error) {
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao excluir fabricantes', life: 3000 });
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
                        :disabled="!selectedFabricantes || !selectedFabricantes.length" />
            </template>

            <template #end>
                <Button label="Exportar" icon="pi pi-upload" severity="secondary" @click="exportCSV($event)" />
            </template>
        </Toolbar>

        <DataTable
            ref="dt"
            v-model:selection="selectedFabricantes"
            :value="fabricantes"
            dataKey="id"
            :paginator="true"
            :rows="10"
            :filters="filters"
            :loading="loading"
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
            :rowsPerPageOptions="[5, 10, 25]"
            currentPageReportTemplate="Mostrando {first} a {last} de {totalRecords} fabricantes"
        >
            <template #header>
                <div class="flex flex-wrap gap-2 items-center justify-between">
                    <h4 class="m-0">Gerenciar Fabricantes</h4>
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
            <Column :exportable="false" header="Ações" style="min-width: 12rem">
                <template #body="slotProps">
                    <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editFabricante(slotProps.data)" />
                    <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteFabricante(slotProps.data)" />
                </template>
            </Column>
        </DataTable>

        <Dialog v-model:visible="deleteFabricanteDialog" :style="{ width: '450px' }" header="Confirmar" :modal="true">
            <div class="flex items-center gap-4">
                <i class="pi pi-exclamation-triangle !text-3xl" />
                <span v-if="fabricante"
                    >Tem certeza que deseja excluir o fabricante <b>{{ fabricante.nome }}</b
                    >?</span
                >
            </div>
            <template #footer>
                <Button label="Não" icon="pi pi-times" text @click="deleteFabricanteDialog = false" />
                <Button label="Sim" icon="pi pi-check" @click="deleteFabricante" />
            </template>
        </Dialog>

        <Dialog v-model:visible="deleteFabricantesDialog" :style="{ width: '450px' }" header="Confirmar" :modal="true">
            <div class="flex items-center gap-4">
                <i class="pi pi-exclamation-triangle !text-3xl" />
                <span>Tem certeza que deseja excluir os fabricantes selecionados?</span>
            </div>
            <template #footer>
                <Button label="Não" icon="pi pi-times" text @click="deleteFabricantesDialog = false" />
                <Button label="Sim" icon="pi pi-check" text @click="deleteSelectedFabricantes" />
            </template>
        </Dialog>
    </div>
</template>
