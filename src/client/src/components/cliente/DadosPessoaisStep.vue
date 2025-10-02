<template>
    <div class="p-fluid">
        <h4>Dados Pessoais</h4>

        <div class="field grid">
            <div class="col-12 md:col-4">
                <label for="cpfCnpj" class="font-bold">CPF/CNPJ*</label>
                <InputMask 
                    id="cpfCnpj" 
                    v-model="form.cpfCnpj" 
                    :mask="cpfCnpjMask" 
                    placeholder="Digite o CPF ou CNPJ"
                    @input="onCpfCnpjInput" />
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
            telefoneNumero: props.cliente.telefoneNumero || ''
        });

        // Observa mudanças no prop cliente e atualiza o form
        watch(() => props.cliente, (newCliente) => {
            form.value.cpfCnpj = newCliente.cpfCnpj || '';
            form.value.razaoSocial = newCliente.razaoSocial || '';
            form.value.nomeFantasia = newCliente.nomeFantasia || '';
            form.value.contato = newCliente.contato || '';
            form.value.telefoneDdd = newCliente.telefoneDdd || '';
            form.value.telefoneNumero = newCliente.telefoneNumero || '';
            
            // Atualiza a máscara do CPF/CNPJ
            cpfCnpjMask.value = determineMask(newCliente.cpfCnpj);
        }, { deep: true });

        // Função para determinar a máscara baseada no valor
        const determineMask = (value) => {
            if (!value) return '999.999.999-99';
            const cleanValue = value.replace(/\D/g, '');
            return cleanValue.length <= 11 ? '999.999.999-99' : '99.999.999/9999-99';
        };

        const cpfCnpjMask = ref(determineMask(form.value.cpfCnpj));

        const onCpfCnpjInput = (event) => {
            const value = event.target.value.replace(/\D/g, ''); // Remove caracteres não numéricos
            
            // Atualiza a máscara baseado no número de dígitos
            if (value.length <= 11) {
                cpfCnpjMask.value = '999.999.999-99'; // CPF
            } else {
                cpfCnpjMask.value = '99.999.999/9999-99'; // CNPJ
            }
        };


        // Funções de validação para CPF e CNPJ
        const isValidCPF = (cpf) => {
            const cleanCpf = cpf.replace(/\D/g, '');
            if (cleanCpf.length !== 11) return false;
            
            // Verifica se todos os dígitos são iguais
            if (/^(\d)\1{10}$/.test(cleanCpf)) return false;
            
            // Validação dos dígitos verificadores
            let sum = 0;
            for (let i = 0; i < 9; i++) {
                sum += parseInt(cleanCpf.charAt(i)) * (10 - i);
            }
            let remainder = (sum * 10) % 11;
            if (remainder === 10 || remainder === 11) remainder = 0;
            if (remainder !== parseInt(cleanCpf.charAt(9))) return false;
            
            sum = 0;
            for (let i = 0; i < 10; i++) {
                sum += parseInt(cleanCpf.charAt(i)) * (11 - i);
            }
            remainder = (sum * 10) % 11;
            if (remainder === 10 || remainder === 11) remainder = 0;
            return remainder === parseInt(cleanCpf.charAt(10));
        };

        const isValidCNPJ = (cnpj) => {
            const cleanCnpj = cnpj.replace(/\D/g, '');
            if (cleanCnpj.length !== 14) return false;
            
            // Verifica se todos os dígitos são iguais
            if (/^(\d)\1{13}$/.test(cleanCnpj)) return false;
            
            // Validação do primeiro dígito verificador
            let sum = 0;
            let weight = 2;
            for (let i = 11; i >= 0; i--) {
                sum += parseInt(cleanCnpj.charAt(i)) * weight;
                weight = weight === 9 ? 2 : weight + 1;
            }
            let remainder = sum % 11;
            const digit1 = remainder < 2 ? 0 : 11 - remainder;
            if (digit1 !== parseInt(cleanCnpj.charAt(12))) return false;
            
            // Validação do segundo dígito verificador
            sum = 0;
            weight = 2;
            for (let i = 12; i >= 0; i--) {
                sum += parseInt(cleanCnpj.charAt(i)) * weight;
                weight = weight === 9 ? 2 : weight + 1;
            }
            remainder = sum % 11;
            const digit2 = remainder < 2 ? 0 : 11 - remainder;
            return digit2 === parseInt(cleanCnpj.charAt(13));
        };

        const validateCpfCnpj = (value) => {
            if (!value) return false;
            const cleanValue = value.replace(/\D/g, '');
            
            if (cleanValue.length === 11) {
                return isValidCPF(value);
            } else if (cleanValue.length === 14) {
                return isValidCNPJ(value);
            }
            return false;
        };

        // Validações
        const rules = {
            cpfCnpj: { 
                required: helpers.withMessage('CPF/CNPJ é obrigatório', required),
                valid: helpers.withMessage('CPF/CNPJ inválido', validateCpfCnpj)
            },
            razaoSocial: { 
                required: helpers.withMessage('Razão Social é obrigatória', required) 
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
            cpfCnpjMask,
            onCpfCnpjInput
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