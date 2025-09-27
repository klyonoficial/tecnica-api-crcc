<template>
    <div class="p-fluid">
        <h4>Dados Pessoais</h4>

        <div class="field grid">
            <div class="col-12 md:col-4">
                <label for="cpfCnpj" class="font-bold">CPF/CNPJ*</label>
                <InputMask id="cpfCnpj" v-model="form.cpfCnpj" :mask="cpfCnpjMask" placeholder="Digite o CPF ou CNPJ" />
                <small v-if="v$.cpfCnpj.$invalid && v$.cpfCnpj.$dirty" class="p-error">{{ v$.cpfCnpj.$errors[0].$message }}</small>
            </div>
            
            <div class="col-12 md:col-4">
                <label for="razaoSocial" class="font-bold">Razão Social*</label>
                <InputText id="razaoSocial" v-model="form.razaoSocial" placeholder="Digite a razão social" />
                <small v-if="v$.razaoSocial.$invalid && v$.razaoSocial.$dirty" class="p-error">{{ v$.razaoSocial.$errors[0].$message }}</small>
            </div>
            
            <div class="col-12 md:col-4">
                <label for="nomeFantasia" class="font-bold">Nome Fantasia</label>
                <InputText id="nomeFantasia" v-model="form.nomeFantasia" placeholder="Digite o nome fantasia" />
            </div>
        </div>

        <div class="field grid">
            <div class="col-12 md:col-4">
                <label for="contato" class="font-bold">Contato</label>
                <InputText id="contato" v-model="form.contato" placeholder="Digite o nome para contato" />
            </div>
            
            <div class="col-12 md:col-2">
                <label for="telefoneDdd" class="font-bold">DDD</label>
                <InputMask id="telefoneDdd" v-model="form.telefoneDdd" mask="99" placeholder="DDD" />
            </div>
            
            <div class="col-12 md:col-3">
                <label for="telefoneNumero" class="font-bold">Telefone</label>
                <InputMask id="telefoneNumero" v-model="form.telefoneNumero" mask="9999-9999" placeholder="Telefone" />
            </div>
            
            <div class="col-12 md:col-3">
                <label for="status" class="font-bold">Status*</label>
                <Dropdown id="status" v-model="form.status" :options="statusOptions" optionLabel="label" optionValue="value" placeholder="Selecione o status" />
                <small v-if="v$.status.$invalid && v$.status.$dirty" class="p-error">{{ v$.status.$errors[0].$message }}</small>
            </div>
        </div>
    </div>
</template>

<script>
import { ref, watch, computed } from 'vue';
import { useVuelidate } from '@vuelidate/core';
import { required, helpers } from '@vuelidate/validators';
import InputMask from 'primevue/inputmask';

export default {
    components: {
        InputMask
    },
    props: {
        cliente: {
            type: Object,
            required: true
        }
    },
    emits: ['update:valid'],
    setup(props, { emit }) {
        const form = ref({
            cpfCnpj: props.cliente.cpfCnpj || '',
            razaoSocial: props.cliente.razaoSocial || '',
            nomeFantasia: props.cliente.nomeFantasia || '',
            contato: props.cliente.contato || '',
            telefoneDdd: props.cliente.telefoneDdd || '',
            telefoneNumero: props.cliente.telefoneNumero || '',
            status: props.cliente.status || 'Ativo'
        });

        const cpfCnpjMask = computed(() => {
            return form.value.cpfCnpj && form.value.cpfCnpj.length > 11 ? '99.999.999/9999-99' : '999.999.999-99';
        });

        const statusOptions = [
            { label: 'Ativo', value: 'Ativo' },
            { label: 'Inativo', value: 'Inativo' },
            { label: 'Bloqueado', value: 'Bloqueado' },
            { label: 'Pendente', value: 'Pendente' }
        ];

        // Validações
        const rules = {
            cpfCnpj: { 
                required: helpers.withMessage('CPF/CNPJ é obrigatório', required) 
            },
            razaoSocial: { 
                required: helpers.withMessage('Razão Social é obrigatória', required) 
            },
            status: { 
                required: helpers.withMessage('Status é obrigatório', required) 
            }
        };

        const v$ = useVuelidate(rules, form);

        // Atualiza os dados do cliente quando o formulário muda
        watch(form, () => {
            // Atualiza o objeto cliente com os valores atuais do formulário
            Object.assign(props.cliente, form.value);
            
            // Verifica a validade do formulário e emite o evento
            v$.value.$validate();
            emit('update:valid', !v$.value.$invalid);
        }, { deep: true });

        // Valida o formulário inicialmente
        v$.value.$validate();
        emit('update:valid', !v$.value.$invalid);

        return {
            form,
            v$,
            statusOptions,
            cpfCnpjMask
        };
    }
}
</script>

<style scoped>
.p-inputtext {
    width: 100%;
}

/* Ajustes para melhor espaçamento dentro das tabs */
.field .col-12, 
.field .md\:col-4,
.field .md\:col-2,
.field .md\:col-3 { 
    padding-left: 0.5rem; 
    padding-right: 0.5rem; 
    margin-bottom: 1rem;
}

.field.grid {
    margin-bottom: 1.5rem;
}

.field.grid:last-child {
    margin-bottom: 0;
}
</style>