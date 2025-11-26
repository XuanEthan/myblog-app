<script setup>
import { RouterView, RouterLink } from 'vue-router';
import { useAuthStore } from '@/stores/AuthStore';
import router from '@/router';

const authStore = useAuthStore()

function logout() {
    let isSuccessful = authStore.logout()
    if (isSuccessful) router.push('/Login')
}

</script>

<template>
    <div class="container">
        <nav class="navbar navbar-left">
            <div class="logo">MyBlog</div>
            <RouterLink to="/Dasboard">
                <i class="fa-solid fa-gauge"></i> Dashboard
            </RouterLink>
            <RouterLink to="/Posts" @click="toggleDropdown">
                <i class="fa-solid fa-file-alt"></i> Posts
            </RouterLink>
            <!-- <RouterLink to="/Categories"> <i class="fa-solid fa-list"></i> Categories
            </RouterLink> -->
            <RouterLink to="/Tags"> <i class="fa-solid fa-tags"></i> Tags</RouterLink>
        </nav>
        <main>
            <nav class="navbar navbar-top">
                <div class="Notification">
                    <RouterLink to="/Notification"><i class="fa-solid fa-bell"></i></RouterLink>
                </div>
                <p>Hi,Admin</p>
                <button v-on:click="logout" class="btn-log-out">Log out</button>
            </nav>
            <div class="content">
                <RouterView />
            </div>
        </main>

    </div>
</template>

<style scoped>
.navbar-left a {
    display: block;
    padding: 10px 12px;
    font-size: 16px;
    text-decoration: none;
    color: var(--text-color-normal);
}

div.logo {
    display: block;
    padding: 16px 13px;
    color: white;
}

.navbar-left a:hover {
    background: var(--bar-hover-clicked);
}

.navbar-left {
    width: 250px;
    height: 100%;
}

.navbar-top {
    width: 100%;
    height: 40px;
    background: var(--navbar-bg-color);
    display: flex;
    justify-content: flex-end;
    align-items: center;
    padding-right: 6px;
    gap: 16px;
}

.navbar-top a {
    font-size: 14px;
    color: white;
}

.content {
    box-shadow: inset 0px 0px 6px 0px black;
}

p {
    font-size: 13px;
    color: white;
}

.btn-log-out {
    background-color: inherit;
    border: none;
    color: white;
}

.btn-log-out:hover {
    color: var(--bar-hover-clicked);
}

a,
button,
input[type="submit"] {
    cursor: pointer;
}
</style>
