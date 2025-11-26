<script setup>
import { onMounted, reactive } from 'vue'
import { useRouter } from 'vue-router'
import { usePostStore } from '@/stores/postStore'
import { useToast } from 'vue-toastification'
import { PostCreateDto } from '@/models/Post'
import { useCategoryStore } from '@/stores/categoryStore'
import { useTagStore } from '@/stores/tagStore'
import PostForm from '@/components/feature/post/PostForm.vue'


const toast = useToast()
const router = useRouter()

const postStore = usePostStore()
const categoryStore = useCategoryStore()
const tagStore = useTagStore()


// let post = reactive(new PostCreateDto())
const inputErrorMessages = reactive({
    ImageFile : [],
    Title: [],
    Content: [],
})

async function handleAdd(form) {
    try {
        await postStore.addPost(form)
        toast.success('Added successfully !')
        router.back()
    } catch (e) {
        if (e.response?.data?.errors) {
            Object.assign(inputErrorMessages, structuredClone(e.response.data.errors))
        }
    }
}

onMounted(() => {
    categoryStore.getAll()
    tagStore.getAll()
})
</script>

<template>
    <div class="container">
        <PostForm :inputErrorMessages="inputErrorMessages" :categoryStore="categoryStore"
            :tagStore="tagStore" @submit="handleAdd" />
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
</style>