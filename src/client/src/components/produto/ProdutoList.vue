<script setup>
import ProdutoService from '@/service/ProdutoService';
import { FilterMatchMode } from '@primevue/core/api';
import { useToast } from 'primevue/usetoast';
import { onMounted, ref } from 'vue';
import { useRouter } from 'vue-router';
import { useErrorHandler } from '@/utils/errorHandler';

const router = useRouter();
const toast = useToast();
const { handleError } = useErrorHandler();
const produtoService = new ProdutoService();

onMounted(() => {
    loadProdutos();
});

const dt = ref();
const produtos = ref([]);
const deleteProdutoDialog = ref(false);
const deleteProdutosDialog = ref(false);
const produto = ref({});
const selectedProdutos = ref();
const filters = ref({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS }
});
const loading = ref(false);

const loadProdutos = async () => {
    loading.value = true;
    try {
        const data = await produtoService.getProdutos();
        produtos.value = data;
    } catch (error) {
        handleError(error);
    } finally {
        loading.value = false;
    }
};

const openNew = () => {
    router.push('/produtos/novo');
};

const editProduto = (prod) => {
    router.push(`/produtos/${prod.id}/editar`);
};

const confirmDeleteProduto = (prod) => {
    produto.value = prod;
    deleteProdutoDialog.value = true;
};

const deleteProduto = async () => {
    try {
        await produtoService.deleteProduto(produto.value.id);
        produtos.value = produtos.value.filter((val) => val.id !== produto.value.id);
        deleteProdutoDialog.value = false;
        produto.value = {};
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Produto excluído com sucesso', life: 3000 });
    } catch (error) {
        handleError(error);
    }
};

const exportCSV = () => {
    dt.value.exportCSV();
};

const confirmDeleteSelected = () => {
    deleteProdutosDialog.value = true;
};

const deleteSelectedProdutos = async () => {
    try {
        const deletePromises = selectedProdutos.value.map(prod => produtoService.deleteProduto(prod.id));
        await Promise.all(deletePromises);
        
        produtos.value = produtos.value.filter((val) => !selectedProdutos.value.includes(val));
        deleteProdutosDialog.value = false;
        selectedProdutos.value = null;
        toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Produtos excluídos com sucesso', life: 3000 });
    } catch (error) {
        toast.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao excluir produtos', life: 3000 });
    }
};
</script>

<template>
    <div class="card">
        <Toolbar class="mb-6">
            <template #start>
                <Button label="Novo" icon="pi pi-plus" severity="secondary" class="mr-2" @click="openNew" />
                <Button 
                    label="Excluir" 
                    icon="pi pi-trash" 
                    severity="secondary" 
                    @click="confirmDeleteSelected" 
                    :disabled="!selectedProdutos || !selectedProdutos.length" 
                />
            </template>

            <template #end>
                <Button label="Exportar" icon="pi pi-upload" severity="secondary" @click="exportCSV($event)" />
            </template>
        </Toolbar>

        <DataTable
            ref="dt"
            v-model:selection="selectedProdutos"
            :value="produtos"
            dataKey="id"
            :paginator="true"
            :rows="10"
            :filters="filters"
            :loading="loading"
            paginatorTemplate="FirstPageLink PrevPageLink PageLinks NextPageLink LastPageLink CurrentPageReport RowsPerPageDropdown"
            :rowsPerPageOptions="[5, 10, 25]"
            currentPageReportTemplate="Mostrando {first} a {last} de {totalRecords} produtos"
        >
            <template #header>
                <div class="flex flex-wrap gap-2 items-center justify-between">
                    <h4 class="m-0">Gerenciar Produtos</h4>
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
            <Column field="fabricanteNome" header="Fabricante" sortable style="min-width: 12rem"></Column>
            <Column :exportable="false" style="min-width: 12rem">
                <template #body="slotProps">
                    <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="editProduto(slotProps.data)" />
                    <Button icon="pi pi-trash" outlined rounded severity="danger" @click="confirmDeleteProduto(slotProps.data)" />
                </template>
            </Column>
        </DataTable>

        <Dialog v-model:visible="deleteProdutoDialog" :style="{ width: '450px' }" header="Confirmar" :modal="true">
            <div class="flex items-center gap-4">
                <i class="pi pi-exclamation-triangle !text-3xl" />
                <span v-if="produto"
                    >Tem certeza que deseja excluir o produto <b>{{ produto.nome }}</b
                    >?</span
                >
            </div>
            <template #footer>
                <Button label="Não" icon="pi pi-times" text @click="deleteProdutoDialog = false" />
                <Button label="Sim" icon="pi pi-check" @click="deleteProduto" />
            </template>
        </Dialog>

        <Dialog v-model:visible="deleteProdutosDialog" :style="{ width: '450px' }" header="Confirmar" :modal="true">
            <div class="flex items-center gap-4">
                <i class="pi pi-exclamation-triangle !text-3xl" />
                <span>Tem certeza que deseja excluir os produtos selecionados?</span>
            </div>
            <template #footer>
                <Button label="Não" icon="pi pi-times" text @click="deleteProdutosDialog = false" />
                <Button label="Sim" icon="pi pi-check" text @click="deleteSelectedProdutos" />
            </template>
        </Dialog>
    </div>
</template>
