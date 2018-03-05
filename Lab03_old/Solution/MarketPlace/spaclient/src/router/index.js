import Vue from 'vue'
import Router from 'vue-router'
import HomeView from '@/components/HomeView'
import CreateView from '@/components/CreateView'
import DetailsView from '@/components/DetailsView'
import UpdateView from '@/components/UpdateView'
import DeleteView from '@/components/DeleteView'

Vue.use(Router)

export default new Router({
  routes: [
    {
      path: '/',
      name: 'HomeView',
      component: HomeView
    },
    {
      path: '/create',
      name: 'CreateView',
      component: CreateView
    },
    {
      path: '/details/:id',
      name: 'DetailsView',
      component: DetailsView
    },
    {
      path: '/delete/:id',
      name: 'DeleteView',
      component: DeleteView
    },
    {
      path: '/update/:id',
      name: 'UpdateView',
      component: UpdateView
    }
  ]
})
