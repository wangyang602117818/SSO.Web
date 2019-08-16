import Vue from 'vue'
import VueRouter from 'vue-router'

import App from './App.vue'
import About from './components/about'
import Home from './components/home'

import { Button, Icon } from 'ant-design-vue'
import 'ant-design-vue/dist/antd.css'

Vue.use(Button)
Vue.use(Icon)

Vue.use(VueRouter)

Vue.config.productionTip = false

const router = new VueRouter({
  routes:[
    { path: '/', component: Home },
    { path: '/about', component: About }
    
  ]
})

new Vue({
  el:"#app",
  router: router,
  render: c => c(App)
})

// new Vue({
//   render: h => h(App)
// }).$mount('#app')


