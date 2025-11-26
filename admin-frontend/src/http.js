import router from '@/router'
import { useAuthStore } from '@/stores/AuthStore'
import axios from 'axios'
import { useToast } from 'vue-toastification'

const toast = useToast()
const http = axios.create({
  baseURL: import.meta.env.VITE_API_BASE_URL,
})

http.interceptors.request.use((config) => {
  const token = localStorage.getItem('token')
  if (token) config.headers.Authorization = `Bearer ${token}`
  return config
})

// xử lý lỗi ở đây
http.interceptors.response.use(
  (response) => {
    return response
  },
  (error) => {
    console.log(error)
    const { response } = error
    if (response) {
      const statusCode = response.status
      const url = error.config?.url
      const authStore = useAuthStore()

      switch (statusCode) {
        case 401:
          if (!url.includes('/Auth/Login')) {
            authStore.logout()
            router.push('/Login')
          }
      }

      if (statusCode >= 500) {
        toast.error(`Server Error . Status Code :(${statusCode})`, { timeout: 4000 })
      }
    } else {
      toast.error(`Cannot connect to Server . Message : ${error.message}`, { timeout: 4000 })
    }
    return Promise.reject(error)
  },
)
export default http
