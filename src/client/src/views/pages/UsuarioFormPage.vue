<template>
    <div>
        <div class="flex align-items-center justify-content-between mb-3">
            <h2 class="m-0">Cadastro de Usuário</h2>
        </div>
        <UsuarioForm :usuarioId="usuarioId" />
    </div>
</template>

<script>
    import UsuarioForm from '@/components/usuario/UsuarioForm.vue';

export default {
    components: {
        UsuarioForm
    },
    props: {
        usuarioId: {
            type: Number,
            default: null
        }
    }
}
</script>

<style scoped>
</style>