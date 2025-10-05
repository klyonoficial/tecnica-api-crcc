<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <TabView v-model:activeIndex="currentStep">
                    <TabPanel header="Dados Pessoais">
                        <DadosPessoaisStep 
                            :cliente="cliente" 
                            @update:valid="updateStepValidity(0, $event)" />
                    </TabPanel>

                    <TabPanel header="Endereços">
                        <EnderecosStep 
                            :enderecos="cliente.enderecos" 
                            @update:enderecos="cliente.enderecos = $event" 
                            @update:valid="updateStepValidity(1, $event)" />
                    </TabPanel>

                    <TabPanel header="Dados de Cobrança">
                        <CobrancaStep 
                            :cliente="cliente" 
                            @update:valid="updateStepValidity(2, $event)" />
                    </TabPanel>

                    <TabPanel header="Dados Serasa">
                        <DadosSerasaStep 
                            :cliente="cliente" 
                            @update:valid="updateStepValidity(3, $event)" />
                    </TabPanel>

                    <TabPanel header="Dados Adicionais">
                        <DadosAdicionaisStep 
                            :cliente="cliente" 
                            @update:valid="updateStepValidity(4, $event)" />
                    </TabPanel>

                    <TabPanel header="Arquivos">
                        <ArquivosStep 
                            :arquivos="cliente.arquivos" 
                            @update:arquivos="cliente.arquivos = $event" 
                            @update:valid="updateStepValidity(5, $event)" />
                    </TabPanel>
                </TabView>

                <div class="flex justify-content-between gap-5">
                    <Button label="Voltar" icon="pi pi-arrow-left" @click="voltar" />
                    <Button type="submit"
                            label="Salvar"
                            icon="pi pi-save"
                            :disabled="submitting"
                            :loading="submitting"
                            @click="onSubmit" />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { ref, computed, reactive, watch, toRaw } from 'vue';
import { useToast } from 'primevue/usetoast';
import { useRoute } from 'vue-router';
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';
import DadosPessoaisStep from './DadosPessoaisStep.vue';
import EnderecosStep from './EnderecosStep.vue';
import CobrancaStep from './CobrancaStep.vue';
import DadosSerasaStep from './DadosSerasaStep.vue';
import DadosAdicionaisStep from './DadosAdicionaisStep.vue';
import ArquivosStep from './ArquivosStep.vue';
import ClienteService from '@/service/ClienteService';
import { Cliente } from '@/models/cliente.model';
import { useErrorHandler } from '@/utils/errorHandler';

export default {
    components: {
        TabView,
        TabPanel,
        DadosPessoaisStep,
        EnderecosStep,
        CobrancaStep,
        DadosSerasaStep,
        DadosAdicionaisStep,
        ArquivosStep
    },
    setup() {
        const toast = useToast();
        const route = useRoute();
        const { handleError } = useErrorHandler();
        const clienteService = new ClienteService();
        const cliente = reactive(new Cliente());
        const currentStep = ref(0);
        const submitting = ref(false);
        const stepsValid = reactive([false, false, false, true, true, false]); // Dados Serasa e Dados Adicionais são sempre válidos
        
        // Recupera o ID da URL
        const clienteId = route.params.id ? parseInt(route.params.id) : null;
    
        const formValid = computed(() => {
            // Para finalizar, apenas os campos obrigatórios precisam estar válidos
            // Dados Pessoais (0), Endereços (1) e Dados de Cobrança (2) são obrigatórios
            return stepsValid[0] && stepsValid[1] && stepsValid[2];
        });

        const loadCliente = async () => {
            if (clienteId) {
                try {
                    const data = await clienteService.getCliente(clienteId);
                    Object.assign(cliente, data);
                    
                    // Garante que enderecos e arquivos sejam arrays
                    if (!cliente.enderecos) {
                        cliente.enderecos = [];
                    }
                    if (!cliente.arquivos) {
                        cliente.arquivos = [];
                    }
                } catch (error) {
                    handleError(error);
                }
            } else {
                // Para novo cliente, garante que os arrays estejam inicializados
                if (!cliente.enderecos) {
                    cliente.enderecos = [];
                }
                if (!cliente.arquivos) {
                    cliente.arquivos = [];
                }
            }
        };


        const updateStepValidity = (step, valid) => {
            stepsValid[step] = valid;
        };

        const onSubmit = async () => {
            submitting.value = true;
            try {
                const clienteSalvo = await clienteService.saveCliente(cliente);
                
                // Se o cliente foi criado (não tinha ID), atualiza o ID local
                if (!cliente.id && clienteSalvo && clienteSalvo.id) {
                    cliente.id = clienteSalvo.id;
                    // Atualiza a URL para incluir o ID do cliente
                    window.history.replaceState({}, '', `/clientes/${clienteSalvo.id}`);
                }
                
                toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Cliente salvo com sucesso!', life: 3000 });
            } catch (error) {
                handleError(error);
            } finally {
                submitting.value = false;
            }
        };

        const voltar = () => {
            // Navegar de volta para a lista de clientes
            window.history.back();
        };

        // Carregar cliente se ID for fornecido
        if (clienteId) {
            loadCliente();
        }

        return {
            cliente,
            currentStep,
            stepsValid,
            formValid,
            submitting,
            updateStepValidity,
            onSubmit,
            voltar
        };
    }
}
</script>

<style scoped>
/* Estilos customizados para as tabs do formulário */
:deep(.p-tabview-nav) {
    border-bottom: 2px solid var(--surface-border);
}

:deep(.p-tabview-nav li .p-tabview-nav-link) {
    padding: 1rem 1.5rem;
    font-weight: 600;
    border-radius: 6px 6px 0 0;
    margin-right: 0.25rem;
}

:deep(.p-tabview-nav li.p-highlight .p-tabview-nav-link) {
    background: var(--primary-color);
    color: var(--primary-color-text);
    border-bottom: 2px solid var(--primary-color);
}

:deep(.p-tabview-nav li:not(.p-highlight) .p-tabview-nav-link:hover) {
    background: var(--surface-hover);
}

:deep(.p-tabview-panels) {
    padding: 2rem 1rem;
}

/* Estilo para os botões de navegação */
.mt-4 {
    padding-top: 1rem;
    border-top: 1px solid var(--surface-border);
}
</style>