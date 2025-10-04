<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <h5>{{ isEdit ? 'Editar Fabricante' : 'Cadastro de Fabricante' }}</h5>
                
                <form @submit.prevent="onSubmit" class="p-fluid">
                    <div class="field">
                        <label for="nome" class="block font-bold mb-2">Nome *</label>
                        <InputText 
                            id="nome" 
                            v-model="fabricante.nome" 
                            :class="{ 'p-invalid': submitted && !fabricante.nome }"
                            required="true" 
                            autofocus 
                            fluid
                        />
                        <small v-if="submitted && !fabricante.nome" class="p-error">Nome é obrigatório.</small>
                    </div>

                    <div class="flex justify-content-end gap-5">
                        <Button label="Voltar" icon="pi pi-arrow-left" @click="$router.push('/fabricantes')" />
                        <Button 
                            type="submit" 
                            :label="isEdit ? 'Atualizar' : 'Salvar'" 
                            icon="pi pi-save"
                            :loading="submitting"
                        />
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
import { ref, reactive, onMounted, computed } from 'vue';
import { useToast } from 'primevue/usetoast';
import { useRouter } from 'vue-router';
import FabricanteService from '@/service/FabricanteService';
import { useErrorHandler } from '@/utils/errorHandler';

export default {
    props: {
        id: {
            type: Number,
            default: null
        }
    },
    setup(props) {
        const toast = useToast();
        const { handleError } = useErrorHandler();
        const router = useRouter();
        const fabricanteService = new FabricanteService();
        const fabricante = reactive({
            id: null,
            nome: ''
        });
        const submitted = ref(false);
        const submitting = ref(false);

        const isEdit = computed(() => props.id !== null);

        const loadFabricante = async () => {
            if (props.id) {
                try {
                    const data = await fabricanteService.getFabricante(props.id);
                    Object.assign(fabricante, data);
                } catch (error) {
                    handleError(error);
                }
            }
        };

        const onSubmit = async () => {
            submitted.value = true;

            if (!fabricante.nome) {
                return;
            }

            submitting.value = true;
            try {
                await fabricanteService.saveFabricante(fabricante);
                toast.add({ 
                    severity: 'success', 
                    summary: 'Sucesso', 
                    detail: isEdit.value ? 'Fabricante atualizado com sucesso!' : 'Fabricante criado com sucesso!', 
                    life: 3000 
                });
                // Redirecionar para a lista
                //TODO: REVISAR
                //router.push('/fabricantes');
            } catch (error) {
                handleError(error);
            } finally {
                submitting.value = false;
            }
        };

        onMounted(() => {
            if (props.id) {
                loadFabricante();
            }
        });

        return {
            fabricante,
            submitted,
            submitting,
            isEdit,
            onSubmit
        };
    }
}
</script>

<style scoped>
.field {
    margin-bottom: 1.5rem;
}

.p-error {
    color: #e24c4c;
    font-size: 0.875rem;
}
</style>
