<template>
    <div class="p-fluid">
        <h4>Confirmação</h4>
        <p>Revise os dados do cliente antes de finalizar o cadastro:</p>
        
        <div class="card surface-card p-4 mb-3">
            <h5>Dados Pessoais</h5>
            <div class="grid">
                <div class="col-12 md:col-4">
                    <div class="field">
                        <label class="font-bold">CPF/CNPJ</label>
                        <div class="p-inputtext p-component p-disabled">
                            {{ cliente.cpfCnpj || 'Não informado' }}
                        </div>
                    </div>
                </div>
                <div class="col-12 md:col-4">
                    <div class="field">
                        <label class="font-bold">Razão Social</label>
                        <div class="p-inputtext p-component p-disabled">
                            {{ cliente.razaoSocial || 'Não informado' }}
                        </div>
                    </div>
                </div>
                <div class="col-12 md:col-4">
                    <div class="field">
                        <label class="font-bold">Nome Fantasia</label>
                        <div class="p-inputtext p-component p-disabled">
                            {{ cliente.nomeFantasia || 'Não informado' }}
                        </div>
                    </div>
                </div>
                <div class="col-12 md:col-4">
                    <div class="field">
                        <label class="font-bold">Contato</label>
                        <div class="p-inputtext p-component p-disabled">
                            {{ cliente.contato || 'Não informado' }}
                        </div>
                    </div>
                </div>
                <div class="col-12 md:col-4">
                    <div class="field">
                        <label class="font-bold">Telefone</label>
                        <div class="p-inputtext p-component p-disabled">
                            {{ cliente.telefoneDdd && cliente.telefoneNumero ? 
                                `(${cliente.telefoneDdd}) ${cliente.telefoneNumero}` : 'Não informado' }}
                        </div>
                    </div>
                </div>
                <div class="col-12 md:col-4">
                    <div class="field">
                        <label class="font-bold">Status</label>
                        <div class="p-inputtext p-component p-disabled">
                            {{ cliente.status || 'Não informado' }}
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="card surface-card p-4 mb-3">
            <h5>Endereços</h5>
            <div v-if="cliente.enderecos.length === 0" class="p-inputtext p-component p-disabled">
                Nenhum endereço cadastrado
            </div>
            <div v-for="(endereco, index) in cliente.enderecos" :key="index" 
                class="p-2 border-bottom-1 surface-border" :class="{'surface-100': endereco.principal}">
                <div class="flex align-items-center">
                    <div class="flex-1">
                        <p class="font-bold m-0">
                            {{ endereco.principal ? 'Endereço Principal' : `Endereço ${index + 1}` }}
                        </p>
                        <p class="m-0">
                            {{ endereco.logradouro }}, {{ endereco.numero }}
                            {{ endereco.complemento ? ', ' + endereco.complemento : '' }}
                        </p>
                        <p class="m-0">
                            {{ endereco.bairro }} - {{ endereco.cidade }}/{{ endereco.uf }} - CEP: {{ endereco.cep }}
                        </p>
                    </div>
                </div>
            </div>
        </div>

        <div class="card surface-card p-4 mb-3">
            <h5>Dados de Cobrança</h5>
            <div class="grid">
                <div class="col-12 md:col-6">
                    <div class="field">
                        <label class="font-bold">Dia de Cobrança</label>
                        <div class="p-inputtext p-component p-disabled">
                            {{ cliente.diaCobranca || 'Não informado' }}
                        </div>
                    </div>
                </div>
                <div class="col-12 md:col-6">
                    <div class="field">
                        <label class="font-bold">Dados Serasa</label>
                        <div class="p-inputtext p-component p-disabled">
                            {{ cliente.dadosSerasa || 'Não informado' }}
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="card surface-card p-4 mb-3">
            <h5>Documentos</h5>
            <div v-if="cliente.arquivos.length === 0" class="p-inputtext p-component p-disabled">
                Nenhum documento anexado
            </div>
            <div v-else class="grid">
                <div v-for="(arquivo, index) in cliente.arquivos" :key="index" class="col-12 md:col-6 lg:col-4">
                    <div class="p-2 border-1 surface-border border-round mb-2">
                        <div class="flex align-items-center">
                            <i :class="getIconClass(arquivo.tipo)" class="text-xl mr-2"></i>
                            <div class="flex-1">
                                <p class="font-bold m-0 text-overflow-ellipsis" :title="arquivo.nome">
                                    {{ arquivo.nome }}
                                </p>
                                <p class="text-sm text-color-secondary m-0">
                                    {{ arquivo.descricao || 'Sem descrição' }}
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="flex align-items-center justify-content-center mt-5">
            <div class="surface-card p-3 border-round">
                <div class="text-center">
                    <i class="pi pi-check-circle text-5xl text-green-500 mb-3"></i>
                    <h5>Tudo pronto!</h5>
                    <p class="mb-0">Clique em "Finalizar" para salvar o cliente.</p>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { onMounted } from 'vue';

export default {
    props: {
        cliente: {
            type: Object,
            required: true
        }
    },
    emits: ['update:valid'],
    setup(props, { emit }) {
        // Retorna o ícone baseado no tipo de arquivo
        const getIconClass = (tipo) => {
            const iconMap = {
                'RG': 'pi pi-id-card',
                'CPF': 'pi pi-id-card',
                'CNPJ': 'pi pi-building',
                'COMP_RESIDENCIA': 'pi pi-home',
                'CONTRATO': 'pi pi-file-pdf',
                'OUTROS': 'pi pi-file'
            };
            
            return iconMap[tipo] || 'pi pi-file';
        };

        // Este passo sempre é válido, então emitimos true na montagem
        onMounted(() => {
            emit('update:valid', true);
        });

        return {
            getIconClass
        };
    }
}
</script>

<style scoped>
.p-disabled {
    background-color: var(--surface-200);
    color: var(--text-color-secondary);
    padding: 0.75rem;
    border-radius: 6px;
    border: 1px solid var(--surface-border);
}

.text-overflow-ellipsis {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    max-width: 200px;
}
</style>