<template>
    <div class="card">
        <div class="flex justify-between items-center mb-4">
            <h5 class="text-xl font-semibold">Clientes</h5>
            <Button 
                label="Novo Cliente" 
                icon="pi pi-plus" 
                @click="navigateToNew" 
                class="p-button-success"
            />
        </div>

        <!-- Filtros -->
        <div class="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4">
            <div class="field">
                <label for="filtro" class="block text-sm font-medium mb-2">Buscar</label>
                <InputText 
                    id="filtro"
                    v-model="filters.filtro" 
                    placeholder="Nome, CPF/CNPJ ou Contato"
                    @input="onFilterChange"
                    class="w-full"
                />
            </div>
            <div class="field">
                <label for="status" class="block text-sm font-medium mb-2">Status</label>
                <Dropdown 
                    id="status"
                    v-model="filters.status" 
                    :options="statusOptions" 
                    optionLabel="label" 
                    optionValue="value"
                    placeholder="Todos os status"
                    @change="onFilterChange"
                    class="w-full"
                    showClear
                />
            </div>
            <div class="field flex items-end">
                <Button 
                    label="Limpar Filtros" 
                    icon="pi pi-filter-slash" 
                    @click="clearFilters"
                    class="p-button-outlined"
                />
            </div>
        </div>

        <!-- Tabela -->
        <DataTable 
            :value="clientes" 
            :loading="loading"
            :paginator="true" 
            :rows="10"
            :rowsPerPageOptions="[5, 10, 20, 50]"
            paginatorTemplate="RowsPerPageDropdown FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink"
            currentPageReportTemplate="Mostrando {first} a {last} de {totalRecords} clientes"
            responsiveLayout="scroll"
            class="p-datatable-sm"
        >
            <Column field="cpfCnpj" header="CPF/CNPJ" :sortable="true">
                <template #body="{ data }">
                    <span class="font-mono">{{ formatCpfCnpj(data.cpfCnpj) }}</span>
                </template>
            </Column>
            
            <Column field="razaoSocial" header="Razão Social" :sortable="true">
                <template #body="{ data }">
                    <div>
                        <div class="font-semibold">{{ data.razaoSocial }}</div>
                        <div v-if="data.nomeFantasia" class="text-sm text-gray-600">{{ data.nomeFantasia }}</div>
                    </div>
                </template>
            </Column>
            
            <Column field="contato" header="Contato" :sortable="true" />
            
            <Column field="telefone" header="Telefone">
                <template #body="{ data }">
                    <span v-if="data.telefone" class="font-mono">{{ data.telefone }}</span>
                    <span v-else class="text-gray-400">-</span>
                </template>
            </Column>
            
            <Column field="status" header="Status" :sortable="true">
                <template #body="{ data }">
                    <Tag 
                        :value="getStatusLabel(data.status)" 
                        :severity="getStatusSeverity(data.status)"
                    />
                </template>
            </Column>
            
            <Column header="Ações" :exportable="false" style="min-width: 4rem">
                <template #body="{ data }">
                    <div class="flex gap-2">
                        <Button 
                            icon="pi pi-pencil" 
                            class="p-button-rounded p-button-text p-button-warning" 
                            @click="editCliente(data)"
                            v-tooltip.top="'Editar'"
                        />
                    </div>
                </template>
            </Column>
        </DataTable>
    </div>

</template>

<script>
import { ref, onMounted, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { useToast } from 'primevue/usetoast';
import ClienteService from '@/service/ClienteService';
import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Dropdown from 'primevue/dropdown';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Tag from 'primevue/tag';

export default {
    name: 'ClienteList',
    components: {
        Button,
        InputText,
        Dropdown,
        DataTable,
        Column,
        Tag
    },
    setup() {
        const router = useRouter();
        const toast = useToast();
        const clienteService = new ClienteService();
        
        const clientes = ref([]);
        const loading = ref(false);
        
        const filters = reactive({
            filtro: '',
            status: null
        });
        
        
        const statusOptions = [
            { label: 'Pendente', value: 'Pendente' },
            { label: 'Aprovado', value: 'Aprovado' },
            { label: 'Reprovado', value: 'Reprovado' },
            { label: 'Inativo', value: 'Inativo' }
        ];
        
        let filterTimeout = null;
        
        const loadClientes = async () => {
            loading.value = true;
            try {
                const data = await clienteService.getClientes(filters.status, filters.filtro);
                clientes.value = data;
            } catch (error) {
                console.error('Erro ao carregar clientes:', error);
                toast.add({
                    severity: 'error',
                    summary: 'Erro',
                    detail: 'Erro ao carregar lista de clientes',
                    life: 3000
                });
            } finally {
                loading.value = false;
            }
        };
        
        const onFilterChange = () => {
            if (filterTimeout) {
                clearTimeout(filterTimeout);
            }
            filterTimeout = setTimeout(() => {
                loadClientes();
            }, 500);
        };
        
        const clearFilters = () => {
            filters.filtro = '';
            filters.status = null;
            loadClientes();
        };
        
        const navigateToNew = () => {
            router.push('/clientes/novo');
        };
        
        const editCliente = (cliente) => {
            router.push(`/clientes/${cliente.id}/editar`);
        };
        
        const formatCpfCnpj = (cpfCnpj) => {
            if (!cpfCnpj) return '';
            
            const numbers = cpfCnpj.replace(/\D/g, '');
            
            if (numbers.length === 11) {
                // CPF: 000.000.000-00
                return numbers.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
            } else if (numbers.length === 14) {
                // CNPJ: 00.000.000/0000-00
                return numbers.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
            }
            
            return cpfCnpj;
        };
        
        const getStatusLabel = (status) => {
            return status || 'Indefinido';
        };
        
        const getStatusSeverity = (status) => {
            const severityMap = {
                'Pendente': 'warning',
                'Aprovado': 'success',
                'Reprovado': 'danger',
                'Inativo': 'secondary'
            };
            return severityMap[status] || 'info';
        };
        
        onMounted(() => {
            loadClientes();
        });
        
        return {
            clientes,
            loading,
            filters,
            statusOptions,
            loadClientes,
            onFilterChange,
            clearFilters,
            navigateToNew,
            editCliente,
            formatCpfCnpj,
            getStatusLabel,
            getStatusSeverity
        };
    }
};
</script>

<style scoped>
.field {
    margin-bottom: 1rem;
}

.font-mono {
    font-family: 'Courier New', Courier, monospace;
}
</style>
