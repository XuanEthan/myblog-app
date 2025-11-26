<script setup>
import { onMounted, reactive, ref } from 'vue';
import { RouterLink } from 'vue-router';
import { usePostStore } from '@/stores/postStore';
import { useToast } from 'vue-toastification'
import SearchForm from '@/components/common/SearchForm.vue';

const toast = useToast()
const postStore = usePostStore();

const IMAGE_SRC_BASE_URL = import.meta.env.VITE_IMAGE_SRC_BASE_URL;

const query = reactive({
    searchKey: ""
})

onMounted(() => {
    postStore.getAll(query.searchKey)
})

async function handleDelete(id) {
    if (!confirm('Delete this Post ?')) return
    await postStore.deletePost(id)
    toast.success('Deleted successfully !')
}

async function handleSearch() {
    await postStore.getAll(query.searchKey)
}

</script>
<template>
    <div class="header">
        <h1>Posts</h1>
        <RouterLink to="/AddPost" class="btn">Add Post</RouterLink>
    </div>

    <SearchForm :placeholder="'Search by Title'" @submit="handleSearch" v-model:searchKey="query.searchKey" />

    <p class="item-count">{{
        postStore.posts.length
        }} items</p>
    <table>
        <thead>
            <tr>
                <th>Title</th>
                <th>Date</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <!-- Nếu có bài viết -->
            <tr v-for="post in postStore.posts" :key="post.id" v-if="postStore.posts.length > 0">
                <td class="title">
                    <div class="thumbnail">
                        <img :src="post.imagePath
                            ? `${IMAGE_SRC_BASE_URL + post.imagePath}`
                            : '/public/Screenshot 2025-10-11 215007.png'" alt="Ảnh" />
                    </div>
                    <RouterLink :to="`/UpdatePost/${post.id}`" class="title-text">
                        {{ post.title }}
                    </RouterLink>
                </td>

                <td>{{ new Date(post.created).toLocaleString() }}</td>

                <td>
                    <RouterLink :to="`/UpdatePost/${post.id}`">Edit</RouterLink> | <a
                        @click.prevent="handleDelete(post.id)" href="#">Delete</a>
                </td>
            </tr>

            <tr v-else>
                <td colspan="3" style="text-align: center; padding: 20px 0; font-size: 14px; ">
                    <i>Not found any posts.</i>
                </td>
            </tr>
        </tbody>
        <tfoot></tfoot>
    </table>
</template>
<style scoped>
a.btn {
    font-size: var(--font-size-normal);
}

a,
form input[type='submit'] {
    color: var(--button-text-color);
}

.header a {
    padding: 8px;
    border: 1px solid var(--button-border-color);
}

table {
    width: 100%;
    max-width: 1000px;
    border-collapse: collapse;
    table-layout: fixed;
    border: 1px solid var(--table-border-bg-color);
    table-layout: fixed;
    margin-top: 8px;
}

th {
    font-size: var(--table-th-font-size);
}

th,
td {
    padding: 8px;
    font-weight: 100;
    vertical-align: middle;

    /* Căn giữa nội dung theo chiều dọc */
}

/* Định nghĩa chiều rộng cho các cột */
table th:nth-child(1) {
    width: 60%;
}

table th:nth-child(2) {
    width: 20%;
}

table th:nth-child(3) {
    width: 20%;
}

/* Căn lề cho các cột */
th:nth-child(1),
td:nth-child(1) {
    text-align: left;
}

th:nth-child(2),
td:nth-child(2) {
    text-align: center;
}

th:nth-child(3),
td:nth-child(3) {
    text-align: center;
}

thead {
    border-bottom: 1px solid var(--table-border-bg-color);
    background-color: white;
}

tbody tr {
    height: 60px;
}

/* Hiệu ứng Zebra-stripe giúp dễ đọc hơn */
tbody tr:nth-child(even) {
    background-color: white;
}

/* 
/* === PHẦN CSS ĐÃ ĐƯỢ̣c TỐI ƯU === */
td.title {
    display: flex;
    align-items: center;
    gap: 12px;
}

.thumbnail {
    width: 80px;
    height: 50px;
    flex-shrink: 0;
    overflow: hidden;
}

.thumbnail img {
    width: 100%;
    height: 100%;
    object-fit: contain;
}

.title-text {
    word-break: break-word;
    font-size: 14px;
    font-weight: 600;
}

td:nth-child(2),
td:nth-child(3) {
    font-size: 13px;
}

p.item-count {
    margin-top: 12px;
}
</style>