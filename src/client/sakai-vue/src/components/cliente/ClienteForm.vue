<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <h5>Cadastro de Cliente</h5>
                <TabView v-model:activeIndex="currentStep">
                    <TabPanel header="Dados Pessoais" :disabled="!canAccessStep(0)">
                        <DadosPessoaisStep 
                            :cliente="cliente" 
                            @update:valid="updateStepValidity(0, $event)" />
                    </TabPanel>

                    <TabPanel header="Endereços" :disabled="!canAccessStep(1)">
                        <EnderecosStep 
                            :enderecos="cliente.enderecos" 
                            @update:enderecos="cliente.enderecos = $event" 
                            @update:valid="updateStepValidity(1, $event)" />
                    </TabPanel>

                    <TabPanel header="Dados de Cobrança" :disabled="!canAccessStep(2)">
                        <CobrancaStep 
                            :cliente="cliente" 
                            @update:valid="updateStepValidity(2, $event)" />
                    </TabPanel>

                    <TabPanel header="Arquivos" :disabled="!canAccessStep(3)">
                        <ArquivosStep 
                            :arquivos="cliente.arquivos" 
                            @update:arquivos="cliente.arquivos = $event" 
                            @update:valid="updateStepValidity(3, $event)" />
                    </TabPanel>

                    <TabPanel header="Confirmação" :disabled="!canAccessStep(4)">
                        <ConfirmacaoStep 
                            :cliente="cliente" 
                            @update:valid="updateStepValidity(4, $event)" />
                    </TabPanel>
                </TabView>

                <div class="flex justify-content-between mt-4">
                    <Button 
                        type="button" 
                        label="Voltar" 
                        icon="pi pi-chevron-left"
                        class="p-button-secondary" 
                        :disabled="currentStep === 0"
                        @click="prevStep" />

                    <Button 
                        v-if="currentStep < 4" 
                        type="button" 
                        label="Próximo" 
                        icon="pi pi-chevron-right"
                        iconPos="right"
                        :disabled="!stepsValid[currentStep]"
                        @click="nextStep" />
                        
                    <Button 
                        v-else 
                        type="submit" 
                        label="Finalizar" 
                        icon="pi pi-check"
                        :disabled="!formValid || submitting"
                        :loading="submitting"
                        @click="onSubmit" />
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { ref, computed, reactive, watch } from 'vue';
import { useToast } from 'primevue/usetoast';
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';
import DadosPessoaisStep from './DadosPessoaisStep.vue';
import EnderecosStep from './EnderecosStep.vue';
import CobrancaStep from './CobrancaStep.vue';
import ArquivosStep from './ArquivosStep.vue';
import ConfirmacaoStep from './ConfirmacaoStep.vue';
import ClienteService from '@/service/ClienteService';
import { Cliente } from '@/models/cliente.model';

export default {
    components: {
        TabView,
        TabPanel,
        DadosPessoaisStep,
        EnderecosStep,
        CobrancaStep,
        ArquivosStep,
        ConfirmacaoStep
    },
    props: {
        id: {
            type: Number,
            default: null
        }
    },
    setup(props) {
        const toast = useToast();
        const clienteService = new ClienteService();
        const cliente = reactive(new Cliente());
        const currentStep = ref(0);
        const submitting = ref(false);
        const stepsValid = reactive([false, false, false, false, true]); // O último passo (confirmação) é sempre válido
        
        const formValid = computed(() => {
            return stepsValid.every((step, index) => {
                // Só verificamos a validade dos passos até o atual
                return index > currentStep.value || step === true;
            });
        });

        // Controla quais abas podem ser acessadas
        const canAccessStep = (stepIndex) => {
            if (stepIndex === 0) return true; // Primeira aba sempre acessível
            
            // Para acessar uma aba, todas as anteriores devem ser válidas
            for (let i = 0; i < stepIndex; i++) {
                if (!stepsValid[i]) return false;
            }
            return true;
        };

        const loadCliente = async () => {
            if (props.id) {
                try {
                    const data = await clienteService.getCliente(props.id);
                    Object.assign(cliente, data);
                    
                    // Garante que enderecos e arquivos sejam arrays
                    if (!cliente.enderecos) {
                        cliente.enderecos = [];
                    }
                    if (!cliente.arquivos) {
                        cliente.arquivos = [];
                    }
                } catch (error) {
                    toast.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao carregar dados do cliente', life: 3000 });
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

        const nextStep = () => {
            if (currentStep.value < 4 && stepsValid[currentStep.value]) {
                currentStep.value++;
            }
        };

        const prevStep = () => {
            if (currentStep.value > 0) {
                currentStep.value--;
            }
        };

        const updateStepValidity = (step, valid) => {
            stepsValid[step] = valid;
        };

        const onSubmit = async () => {
            if (!formValid.value) return;
            
            submitting.value = true;
            try {
                await clienteService.saveCliente(cliente);
                toast.add({ severity: 'success', summary: 'Sucesso', detail: 'Cliente salvo com sucesso!', life: 3000 });
                // Redirecionar ou redefinir o formulário conforme necessário
            } catch (error) {
                toast.add({ severity: 'error', summary: 'Erro', detail: 'Erro ao salvar cliente', life: 3000 });
            } finally {
                submitting.value = false;
            }
        };

        // Observa mudanças na aba ativa para sincronizar com currentStep
        watch(currentStep, (newStep) => {
            // Impede navegar para abas não liberadas
            if (!canAccessStep(newStep)) {
                // Volta para a última aba válida
                let validStep = 0;
                for (let i = 4; i >= 0; i--) {
                    if (canAccessStep(i)) {
                        validStep = i;
                        break;
                    }
                }
                currentStep.value = validStep;
            }
        });

        // Carregar cliente se ID for fornecido
        if (props.id) {
            loadCliente();
        }

        return {
            cliente,
            currentStep,
            stepsValid,
            formValid,
            submitting,
            canAccessStep,
            nextStep,
            prevStep,
            updateStepValidity,
            onSubmit
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

:deep(.p-tabview-nav li.p-disabled .p-tabview-nav-link) {
    opacity: 0.5;
    cursor: not-allowed;
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