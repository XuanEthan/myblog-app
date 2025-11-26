import { createRouter, createWebHistory } from 'vue-router'
import AddPost from '@/views/posts/AddPost.vue'
import Posts from '@/views/posts/Posts.vue'
import UpdatePost from '@/views/posts/UpdatePost.vue'
// import Categories from '@/views/categories/Categories.vue'
import Tags from '@/views/tags/Tags.vue'
import UpdateTag from '@/views/tags/UpdateTag.vue'
import NotFound from '@/views/errors/NotFound.vue'
import InternalServerError from '@/views/errors/InternalServerError.vue'
import UndefineError from '@/views/errors/UndefineError.vue'
import Login from '@/views/auth/Login.vue'
import Dasboard from '@/views/Dasboard.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/Login', component: Login, meta: { layout: 'AuthLayout' } },
    { path: '/Dasboard', component: Dasboard, meta: { requireAuth: true } },
    { path: '/Posts', component: Posts, meta: { requireAuth: true } },
    { path: '/AddPost', component: AddPost, meta: { requireAuth: true } },
    { path: '/UpdatePost/:id', component: UpdatePost, meta: { requireAuth: true } },
    // { path: '/Categories', component: Categories, meta: { requireAuth: true } },
    { path: '/Tags', component: Tags, meta: { requireAuth: true } },
    { path: '/UpdateTag/:id', component: UpdateTag, meta: { requireAuth: true } },
    { path: '/NotFound', component: NotFound, meta: { layout: 'ErrorLayout' } },
    {
      path: '/InternalServerError',
      component: InternalServerError,
      meta: { layout: 'ErrorLayout' },
    },
    {
      path: '/UndefineError',
      component: UndefineError,
      meta: { layout: 'ErrorLayout' },
    },
  ],
})

router.beforeEach((to) => {
  const token = localStorage.getItem('token')
  if ((to.meta.requireAuth || to.path === '/') && !token) {
    return '/Login'
  }

  if (to.path === '/Login' && token) {
    return '/'
  }
})

export default router
