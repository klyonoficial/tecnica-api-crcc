<template>
    <div>
        <div class="flex align-items-center justify-content-between mb-3">
            <h2 class="m-0">Lista de Usuário</h2>
        </div>
        <UsuarioList />
    </div>
</template>

<script>
import UsuarioList from '@/components/usuario/UsuarioList.vue';

export default {
  components: { UsuarioList }
};
</script>