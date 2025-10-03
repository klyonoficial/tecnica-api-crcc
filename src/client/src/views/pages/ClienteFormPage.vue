<template>
    <div>
        <div class="flex align-items-center justify-content-between mb-3">
            <h2 class="m-0">Cadastro de Cliente</h2>
        </div>
        <ClienteForm />
    </div>
</template>

<script>
import ClienteForm from '@/components/cliente/ClienteForm.vue';

export default {
    components: {
        ClienteForm
    }
}
</script>

<style scoped>
</style>