<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <h5>{{ isEdit ? 'Editar Produto' : 'Cadastro de Produto' }}</h5>
                
                <form @submit.prevent="onSubmit" class="p-fluid">
                    <div class="field">
                        <label for="nome" class="block font-bold mb-2">Nome *</label>
                        <InputText 
                            id="nome" 
                            v-model="produto.nome" 
                            :class="{ 'p-invalid': submitted && !produto.nome }"
                            required="true" 
                            autofocus 
                            fluid
                        />
                        <small v-if="submitted && !produto.nome" class="p-error">Nome é obrigatório.</small>
                    </div>

                    <div class="field">
                        <label for="fabricanteId" class="block font-bold mb-2">ID do Fabricante *</label>
                        <InputNumber 
                            id="fabricanteId" 
                            v-model="produto.fabricanteId" 
                            :class="{ 'p-invalid': submitted && !produto.fabricanteId }"
                            required="true"
                            :min="1"
                            fluid
                        />
                        <small v-if="submitted && !produto.fabricanteId" class="p-error">ID do Fabricante é obrigatório.</small>
                    </div>

                    <div class="flex justify-content-end gap-2">
                        <Button 
                            type="button" 
                            label="Cancelar" 
                            icon="pi pi-times" 
                            class="p-button-secondary" 
                            @click="$router.push('/produtos')" 
                        />
                        <Button 
                            type="submit" 
                            :label="isEdit ? 'Atualizar' : 'Salvar'" 
                            icon="pi pi-check" 
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
import ProdutoService from '@/service/ProdutoService';
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
        const produtoService = new ProdutoService();
        const produto = reactive({
            id: null,
            nome: '',
            fabricanteId: null
        });
        const submitted = ref(false);
        const submitting = ref(false);

        const isEdit = computed(() => props.id !== null);

        const loadProduto = async () => {
            if (props.id) {
                try {
                    const data = await produtoService.getProduto(props.id);
                    Object.assign(produto, data);
                } catch (error) {
                    handleError(error);
                }
            }
        };

        const onSubmit = async () => {
            submitted.value = true;

            if (!produto.nome || !produto.fabricanteId) {
                return;
            }

            submitting.value = true;
            try {
                await produtoService.saveProduto(produto);
                toast.add({ 
                    severity: 'success', 
                    summary: 'Sucesso', 
                    detail: isEdit.value ? 'Produto atualizado com sucesso!' : 'Produto criado com sucesso!', 
                    life: 3000 
                });
                // Redirecionar para a lista
                router.push('/produtos');
            } catch (error) {
                handleError(error);
            } finally {
                submitting.value = false;
            }
        };

        onMounted(() => {
            if (props.id) {
                loadProduto();
            }
        });

        return {
            produto,
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
