import { defineStore } from 'pinia'
import { getAll, addCategory, deleteCategory, updateCategory } from '@/services/CategoryService'
import { getById } from '@/services/PostService'

export const useCategoryStore = defineStore('category', {
  state: () => ({
    categories: [],
  }),
  actions: {
    async getById(id) {
      return (await getById(id)).data
    },
    async getAll(searchKey = '') {
      try {
        const resp = await getAll(searchKey)
        this.categories = resp.data
      } catch (e) {
        throw e
      }
    },
    async addCategory(category) {
      try {
        const resp = await addCategory(category)
      } catch (e) {
        throw e
      }
    },
    async updateCategory(category) {
      try {
        const resp = await updateCategory(category)
      } catch (e) {
        throw e
      }
    },
    async deleteCategory(id) {
      try {
        const resp = await deleteCategory(id)
        this.categories = this.categories.filter((c) => c.id !== id)
      } catch (e) {
        throw e
      }
    },
  },
})
