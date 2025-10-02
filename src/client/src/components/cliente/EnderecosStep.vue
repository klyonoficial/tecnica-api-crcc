<template>
    <div class="p-fluid">
        <div class="flex justify-content-between align-items-center mb-3">
            <h4>Endereços</h4>
            <Button type="button" icon="pi pi-plus" label="Novo Endereço" @click="addEndereco" />
        </div>

        <div v-if="enderecos.length === 0" class="empty-message p-4 text-center">
            <i class="pi pi-map-marker text-5xl mb-3 text-color-secondary"></i>
            <p>Nenhum endereço cadastrado. Clique em "Novo Endereço" para adicionar.</p>
        </div>

        <div v-else>
            <Accordion :activeIndex="openIndex">
                <AccordionTab v-for="(endereco, index) in enderecos" :key="index" 
                    :header="getEnderecoHeader(endereco, index)">
                    <div class="endereco-form">
                        <!-- Primeira linha: Tipo -->
                        <div class="field">
                            <label :for="'tipo-' + index" class="font-bold">Tipo*</label>
                            <Dropdown :id="'tipo-' + index" v-model="endereco.tipo" :options="tiposEndereco" 
                                optionLabel="label" optionValue="value" placeholder="Selecione o tipo" class="w-full" />
                            <small v-if="validations[index] && validations[index].tipo && validations[index].tipo.$invalid && validations[index].tipo.$dirty" class="p-error">
                                Tipo é obrigatório
                            </small>
                        </div>

                        <!-- Segunda linha: CEP -->
                        <div class="field">
                            <label :for="'cep-' + index" class="font-bold">CEP*</label>
                            <div class="p-inputgroup">
                                <InputMask :id="'cep-' + index" v-model="endereco.cep" mask="99999-999" placeholder="CEP" class="w-full" />
                                <Button icon="pi pi-search" @click="buscarCep(endereco)" class="p-button-success" />
                            </div>
                            <small v-if="validations[index] && validations[index].cep && validations[index].cep.$invalid && validations[index].cep.$dirty" class="p-error">
                                CEP é obrigatório
                            </small>
                        </div>

                        <!-- Terceira linha: Logradouro e Número -->
                        <div class="field grid">
                            <div class="col-12 md:col-8">
                                <label :for="'logradouro-' + index" class="font-bold">Logradouro*</label>
                                <InputText :id="'logradouro-' + index" v-model="endereco.logradouro" placeholder="Logradouro" class="w-full" />
                                <small v-if="validations[index] && validations[index].logradouro && validations[index].logradouro.$invalid && validations[index].logradouro.$dirty" class="p-error">
                                    Logradouro é obrigatório
                                </small>
                            </div>
                            <div class="col-12 md:col-4">
                                <label :for="'numero-' + index" class="font-bold">Número*</label>
                                <InputText :id="'numero-' + index" v-model="endereco.numero" placeholder="Número" class="w-full" />
                                <small v-if="validations[index] && validations[index].numero && validations[index].numero.$invalid && validations[index].numero.$dirty" class="p-error">
                                    Número é obrigatório
                                </small>
                            </div>
                        </div>

                        <!-- Quarta linha: Complemento -->
                        <div class="field">
                            <label :for="'complemento-' + index" class="font-bold">Complemento</label>
                            <InputText :id="'complemento-' + index" v-model="endereco.complemento" placeholder="Complemento" class="w-full" />
                        </div>

                        <!-- Quinta linha: Bairro, Cidade e UF -->
                        <div class="field grid">
                            <div class="col-12 md:col-5">
                                <label :for="'bairro-' + index" class="font-bold">Bairro*</label>
                                <InputText :id="'bairro-' + index" v-model="endereco.bairro" placeholder="Bairro" class="w-full" />
                                <small v-if="validations[index] && validations[index].bairro && validations[index].bairro.$invalid && validations[index].bairro.$dirty" class="p-error">
                                    Bairro é obrigatório
                                </small>
                            </div>
                            <div class="col-12 md:col-5">
                                <label :for="'cidade-' + index" class="font-bold">Cidade*</label>
                                <InputText :id="'cidade-' + index" v-model="endereco.cidade" placeholder="Cidade" class="w-full" />
                                <small v-if="validations[index] && validations[index].cidade && validations[index].cidade.$invalid && validations[index].cidade.$dirty" class="p-error">
                                    Cidade é obrigatória
                                </small>
                            </div>
                            <div class="col-12 md:col-2">
                                <label :for="'uf-' + index" class="font-bold">UF*</label>
                                <Dropdown :id="'uf-' + index" v-model="endereco.uf" :options="estados" placeholder="UF" class="w-full" />
                                <small v-if="validations[index] && validations[index].uf && validations[index].uf.$invalid && validations[index].uf.$dirty" class="p-error">
                                    UF é obrigatória
                                </small>
                            </div>
                        </div>

                        <!-- Sexta linha: Opções -->
                        <div class="field grid align-items-end">
                            <div class="col-12 md:col-6">
                                <div class="field-checkbox">
                                    <Checkbox :id="'principal-' + index" v-model="endereco.principal" :binary="true" 
                                        @change="setPrincipal(index)" />
                                    <label :for="'principal-' + index" class="ml-2">Endereço Principal</label>
                                </div>
                            </div>
                            <div class="col-12 md:col-6 flex justify-content-end">
                                <Button type="button" icon="pi pi-trash" class="p-button-danger p-button-outlined" 
                                    label="Remover" @click="removerEndereco(index)" />
                            </div>
                        </div>
                    </div>
                </AccordionTab>
            </Accordion>
        </div>
    </div>
</template>

<script>
import { ref, reactive, watch } from 'vue';
import { useVuelidate } from '@vuelidate/core';
import { required } from '@vuelidate/validators';
import Accordion from 'primevue/accordion';
import AccordionTab from 'primevue/accordiontab';
import InputMask from 'primevue/inputmask';
import Dropdown from 'primevue/dropdown';
import Checkbox from 'primevue/checkbox';
import { Endereco } from '@/models/cliente.model';
import axios from 'axios';

export default {
    components: {
        Accordion,
        AccordionTab,
        InputMask,
        Dropdown,
        Checkbox
    },
    props: {
        enderecos: {
            type: Array,
            required: true
        }
    },
    emits: ['update:enderecos', 'update:valid'],
    setup(props, { emit }) {
        const openIndex = ref(0);
        const validations = reactive([]);

        const estados = [
            'AC', 'AL', 'AM', 'AP', 'BA', 'CE', 'DF', 'ES', 'GO',
            'MA', 'MG', 'MS', 'MT', 'PA', 'PB', 'PE', 'PI', 'PR',
            'RJ', 'RN', 'RO', 'RR', 'RS', 'SC', 'SE', 'SP', 'TO'
        ];

        const tiposEndereco = [
            { label: 'Cobrança', value: 'COBR' },
            { label: 'Entrega', value: 'ENT' },
            { label: 'Outros', value: 'OUT' }
        ];

        // Cria validações para cada endereço
        const createValidation = (endereco) => {
            return useVuelidate({
                cep: { required },
                logradouro: { required },
                numero: { required },
                bairro: { required },
                cidade: { required },
                uf: { required },
                tipo: { required }
            }, endereco);
        };

        // Inicializa validações - garante que sempre teremos pelo menos um endereço
        const initializeValidations = () => {
            // Limpa validações existentes
            validations.length = 0;
            
            // Se não houver endereços, adiciona um vazio
            if (props.enderecos.length === 0) {
                const novoEndereco = new Endereco();
                props.enderecos.push(novoEndereco);
            }

            // Cria validações para todos os endereços
            props.enderecos.forEach(endereco => {
                validations.push(createValidation(endereco));
            });
        };

        // Inicializa as validações
        initializeValidations();

        // Adiciona um novo endereço
        const addEndereco = () => {
            const novoEndereco = new Endereco();
            props.enderecos.push(novoEndereco);
            validations.push(createValidation(novoEndereco));
            openIndex.value = props.enderecos.length - 1;
            validateAll();
        };

        // Remove um endereço
        const removerEndereco = (index) => {
            props.enderecos.splice(index, 1);
            validations.splice(index, 1);
            
            if (props.enderecos.length === 0) {
                addEndereco();
            }
            
            validateAll();
        };

        // Define um endereço como principal
        const setPrincipal = (index) => {
            props.enderecos.forEach((endereco, i) => {
                endereco.principal = i === index;
            });
        };

        // Busca CEP via API
        const buscarCep = async (endereco) => {
            if (!endereco.cep || endereco.cep.length < 8) return;
            
            try {
                const cepLimpo = endereco.cep.replace(/\D/g, '');
                const response = await axios.get(`https://viacep.com.br/ws/${cepLimpo}/json/`);
                
                if (!response.data.erro) {
                    endereco.logradouro = response.data.logradouro;
                    endereco.bairro = response.data.bairro;
                    endereco.cidade = response.data.localidade;
                    endereco.uf = response.data.uf;
                }
            } catch (error) {
                console.error('Erro ao buscar CEP:', error);
            }
        };

        // Formata o cabeçalho do accordion
        const getEnderecoHeader = (endereco, index) => {
            const tipoLabel = tiposEndereco.find(t => t.value === endereco.tipo)?.label || 'Outros';
            const enderecoInfo = endereco.logradouro ? 
                `${endereco.logradouro}, ${endereco.numero}${endereco.complemento ? ', ' + endereco.complemento : ''} - ${endereco.cidade}/${endereco.uf}` : 
                `Endereço ${index + 1}`;
                
            const headerInfo = `[${tipoLabel}] ${enderecoInfo}`;
            return endereco.principal ? 
                `${headerInfo} (Principal)` : 
                headerInfo;
        };

        // Verifica a validade de todos os endereços
        const validateAll = () => {
            if (validations.length === 0 || props.enderecos.length === 0) {
                emit('update:valid', false);
                return false;
            }
            
            let isValid = true;
            
            validations.forEach((validation, index) => {
                if (validation && typeof validation.$touch === 'function') {
                    try {
                        validation.$touch();
                        if (validation.$invalid) {
                            isValid = false;
                        }
                    } catch (error) {
                        console.warn('Erro na validação do endereço', index, error);
                        isValid = false;
                    }
                }
            });
            
            emit('update:valid', isValid && props.enderecos.length > 0);
            return isValid;
        };

        // Observa mudanças nos endereços para validação
        watch(() => [...props.enderecos], () => {
            validateAll();
        }, { deep: true });

        // Validação inicial
        validateAll();

        return {
            openIndex,
            validations,
            estados,
            tiposEndereco,
            addEndereco,
            removerEndereco,
            setPrincipal,
            buscarCep,
            getEnderecoHeader
        };
    }
}
</script>

<style scoped>
.empty-message {
    border: 1px dashed var(--surface-border);
    border-radius: 8px;
    color: var(--text-color-secondary);
}

.endereco-form {
    padding: 1rem 0;
}

.endereco-form .field {
    margin-bottom: 1.5rem;
}

.endereco-form .field:last-child {
    margin-bottom: 0;
}

.endereco-form label {
    display: block;
    margin-bottom: 0.5rem;
    font-weight: 600;
    color: var(--text-color);
}

.endereco-form .p-inputgroup {
    display: flex;
    width: 100%;
}

.endereco-form .p-inputgroup .p-inputmask,
.endereco-form .p-inputgroup .p-inputtext {
    flex: 1;
}

.endereco-form .p-inputgroup .p-button {
    flex-shrink: 0;
    margin-left: 0.25rem;
}

.endereco-form .field-checkbox {
    display: flex;
    align-items: center;
    margin-top: 0.5rem;
}

.endereco-form .field-checkbox .p-checkbox {
    margin-right: 0.5rem;
}

.endereco-form .field-checkbox label {
    margin-bottom: 0;
    font-weight: normal;
    cursor: pointer;
}

.endereco-form .p-error {
    display: block;
    margin-top: 0.25rem;
    font-size: 0.875rem;
}

/* Responsividade melhorada */
@media (max-width: 768px) {
    .endereco-form .field.grid {
        display: block;
    }
    
    .endereco-form .field.grid > div {
        width: 100%;
        margin-bottom: 1rem;
    }
    
    .endereco-form .field.grid > div:last-child {
        margin-bottom: 0;
    }
}

/* Melhor alinhamento dos botões */
.endereco-form .flex.justify-content-end {
    display: flex;
    justify-content: flex-end;
    align-items: center;
    margin-top: 1rem;
}

/* Estilo para o accordion */
:deep(.p-accordion .p-accordion-header .p-accordion-header-link) {
    padding: 1rem 1.5rem;
    font-weight: 600;
}

:deep(.p-accordion .p-accordion-content) {
    padding: 1.5rem;
}
</style>