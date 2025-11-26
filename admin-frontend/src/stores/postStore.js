import { defineStore } from 'pinia'
import { getAll, getById, addPost, updatePost, deletePost } from '@/services/PostService'

export const usePostStore = defineStore('post', {
  state: () => ({
    posts: [],
  }),
  actions: {
    async getAll(searchKey) {
      try {
        const resp = await getAll(searchKey)
        this.posts = resp.data
      } catch (e) {
        throw e
      }
    },
    async getById(id) {
      return (await getById(id)).data
    },
    async addPost(post) {
      try {
        await addPost(post)
      } catch (e) {
        throw e
      }
    },
    async updatePost(post) {
      try {
        await updatePost(post)
      } catch (e) {
        throw e
      }
    },
    async deletePost(id) {
      try {
        await deletePost(id)
        this.posts = this.posts.filter((p) => p.id !== id)
      } catch (e) {
        throw e
      }
    },
  },
})
