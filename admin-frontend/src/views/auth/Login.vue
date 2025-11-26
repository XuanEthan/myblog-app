<script setup>
import { useAuthStore } from '@/stores/AuthStore';
import { reactive } from 'vue';
import router from '@/router';

const authStore = useAuthStore()

const loginForm = reactive({
    userName: '',
    password: ''
})

async function handleSubmit() {
    console.log(loginForm)
    let isSuccessful = await authStore.login(loginForm)
    if(isSuccessful) router.push('/')
}

</script>
<template>
  <div class="login-container">
    <form class="login-form" @submit.prevent="handleSubmit">
      <h2>Log in to MyBlog</h2>

      <div class="input-group">
        <label>Username</label>
        <input type="text" v-model="loginForm.userName" placeholder="Enter username" />
      </div>

      <div class="input-group">
        <label>Password</label>
        <input type="password" v-model="loginForm.password" placeholder="Enter password" />
      </div>

      <button type="submit" class="btn-login">Login</button>

      <p class="error">{{ authStore.errorMessage  }}</p>
    </form>
  </div>
</template>

<style scoped>
.login-container {
  width: 100%;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background: #f5f6fa;
}

.login-form {
  width: 360px;
  background: #fff;
  padding: 32px;
  border-radius: 12px;
  box-shadow: 0 8px 25px rgba(0, 0, 0, 0.08);
  box-sizing: border-box;
}

.login-form h2 {
  text-align: center;
  margin-bottom: 24px;
  font-size: 26px;
  font-weight: 600;
  color: #333;
}

.input-group {
  margin-bottom: 18px;
  display: flex;
  flex-direction: column;
}

.input-group label {
  font-size: 14px;
  margin-bottom: 6px;
  color: #555;
}

.input-group input {
  padding: 10px 12px;
  border-radius: 8px;
  border: 1px solid #dcdde1;
  font-size: 15px;
  outline: none;
  transition: 0.2s;
}

.input-group input:focus {
  border-color: #4c8bfd;
  box-shadow: 0 0 0 3px rgba(76, 139, 253, 0.15);
}

.btn-login {
  width: 100%;
  padding: 12px;
  margin-top: 6px;
  border: none;
  background: black;
  color: white;
  border-radius: 8px;
  font-size: 16px;
  font-weight: 600;
  cursor: pointer;
  transition: background 0.25s ease;
}

.btn-login:hover {
  background: #3c73d9;
}

.error {
  margin-top: 10px;
  color: #e84118;
  font-size: 14px;
  text-align: center;
}
</style>