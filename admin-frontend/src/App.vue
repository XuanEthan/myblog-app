<script setup>
import { computed } from 'vue';
import { useRoute } from 'vue-router';

import DefaultLayout from './layouts/DefaultLayout.vue';
import ErrorLayout from './layouts/ErrorLayout.vue';
import AuthLayout from './layouts/AuthLayout.vue';

// SỬA Ở ĐÂY: Dùng object {} thay vì array []
const layouts = {
  AuthLayout,
  DefaultLayout, // Tương đương DefaultLayout: DefaultLayout
  ErrorLayout,   // Tương đương ErrorLayout: ErrorLayout
};

const route = useRoute();

const layoutComponent = computed(() => {
  // Dòng này giờ sẽ hoạt động đúng
  const layoutName = route.meta.layout || 'DefaultLayout';
  return layouts[layoutName];
});

</script>

<template>
  <component :is="layoutComponent">
  </component>
</template>

<style>
@import "./assets/styles.css";
</style>