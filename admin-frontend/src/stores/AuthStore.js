import { defineStore } from 'pinia'
import { login } from '@/services/AuthService'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    errorMessage: '',
  }),
  actions: {
    isValidInputs(loginForm) {
      let isValid = true

      this.errorMessage = ''

      if (loginForm.userName === '' || loginForm.password === '') {
        this.errorMessage = 'Tên đăng nhập hoặc mật khẩu không được để trống.'
        isValid = false
      }
      return isValid
    },

    async login(loginForm) {
      var isSuccessful = true
      if (!this.isValidInputs(loginForm)) return
      
      var res = await login(loginForm)
      if (res.ok) {
        if (res.result.data?.token) {
          localStorage.setItem('token', res.result.data?.token)
        }
      } else {
        this.errorMessage = res.result.response?.data
        isSuccessful = false
      }
      return isSuccessful
    },

    logout() {
      if (localStorage.getItem('token')) {
        localStorage.removeItem('token')
      }
      return true
    },
  },
})
