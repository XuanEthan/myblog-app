<script setup>
import { onMounted, reactive } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { usePostStore } from '@/stores/postStore'
import { useToast } from 'vue-toastification'
import { PostUpdateDto, PostResponseDto } from '@/models/Post'
import { useCategoryStore } from '@/stores/categoryStore'
import { useTagStore } from '@/stores/tagStore'
import PostForm from '@/components/feature/post/PostForm.vue'


const route = useRoute()
const router = useRouter()
const toast = useToast()

const postStore = usePostStore()
const categoryStore = useCategoryStore()
const tagStore = useTagStore()

// let postUpdate = reactive(new PostUpdateDto())
const post = reactive(new PostResponseDto())

// Reactive để lưu lỗi nhập liệu
const inputErrorMessages = reactive({
    image : [],
    Title: [],
    Paragraph: []
})

// Hàm cập nhật bài viết
async function handleUpdate(form) {
    try {
        await postStore.updatePost(form)
        toast.success('Updated successfully!')
        router.back()
    } catch (e) {
        Object.assign(inputErrorMessages, e.response.data.errors)
    }
}

onMounted(async () => {
    try {
        const resp = await postStore.getById(Number(route.params.id))

        await categoryStore.getAll()
        await tagStore.getAll()

        Object.assign(post, resp)
        console.log(post);
    } catch (e) {
        router.push('/NotFound')
    }
})
</script>

<template>
    <div class="container">
        <PostForm v-model:post="post" :inputErrorMessages="inputErrorMessages" :categoryStore="categoryStore"
            :tagStore="tagStore" @submit="handleUpdate" :aboutTo="'Save'" />
    </div>
</template>

<style scoped>
.container {
    width: 100%;
    height: 100%;
    position: relative;
    padding-top: 50px;
}

/* --- GRID LAYOUT --- */
form {
    display: grid;
    width: 100%;
    height: 100%;
    grid-template-columns: auto 20%;
    gap: 12px;
}

/* --- FORM LEFT --- */
.form-left {
    display: flex;
    flex-direction: column;
    gap: 10px;
}

form input,
form textarea {
    border: none;
}

form input[type='text'],
form textarea {
    display: block;
    width: 100%;
    padding: 6px;
    border: 1px solid #ddd;
}

form textarea {
    height: 400px;
    resize: none;
}

form input[name='title'] {
    font-size: 30px;
    font-weight: bold;
}

.error-msg {
    color: red;
    font-size: 13px;
}

/* --- FORM RIGHT --- */
.form-right {
    width: 100%;
    display: flex;
    flex-direction: column;
    gap: 16px;
}

.card {
    background-color: #fff;
    border: 1px solid #ddd;
    padding: 12px;
}

.card>label {
    display: block;
    font-weight: 600;
    margin-bottom: 6px;
    border-bottom: 1px solid black;
}

.checkbox-group {
    display: flex;
    gap: 4px;
}

.checkbox-group.categories {
    flex-direction: column;
}

.checkbox-group.tags {
    flex-wrap: wrap;
    width: 100%;
    height: auto;
}

.checkbox-group label {
    display: flex;
    align-items: center;
    font-size: 14px;
}

/* --- TAG INPUT --- */
.card input[type='text'] {
    width: 100%;
    padding: 6px;
    border: 1px solid #ccc;
    border-radius: 4px;
}
</style>