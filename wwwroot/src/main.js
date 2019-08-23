
import Vue from 'vue'
import VueRouter from 'vue-router'
import VueResource from 'vue-resource'
import babelPolyfill from 'babel-polyfill'

import About from '@/components/about'
import Home from '@/components/home'
import OverView from '@/components/overview';
import UserBasic from '@/components/user_basic';
import UserAuthority from '@/components/user_authority';
import Role from '@/components/role';
import Department from '@/components/department';
import Log from '@/components/log';
import Settings from '@/components/settings';

import { Button, Icon,Layout,Menu,Table,Input,Drawer,Form ,Row,Col} from 'ant-design-vue'
import 'ant-design-vue/dist/antd.css'


Vue.use(Button)
Vue.use(Icon)
Vue.use(Layout)
Vue.use(Menu)
Vue.use(Table)
Vue.use(Input)
Vue.use(Drawer)
Vue.use(Form)
Vue.use(Row)
Vue.use(Col)

Vue.use(VueRouter)
Vue.use(VueResource)
Vue.use(babelPolyfill)

Vue.http.options.root = 'http://www.sso.com:8030/';

Vue.config.productionTip = false

const router = new VueRouter({
  // mode: 'history',
  routes: [
    {
      path: '/',
      component: Home,
      name: "home",
      children: [
        {
          path:'',
          component: OverView
        },
        {
          path: 'overview',
          name: "overview",
          component: OverView
        },
        {
          path: 'userbasic',
          name: "userbasic",
          component: UserBasic
        },
        {
          path: 'userauthority',
          name: "userauthority",
          component: UserAuthority
        },
        {
          path: 'role',
          name: "role",
          component: Role
        },
        {
          path: 'department',
          name: "department",
          component: Department
        },
        {
          path: 'log',
          name: "log",
          component: Log
        },
        {
          path: 'settings',
          name: "settings",
          component: Settings
        },
      ]
    },
    {
      path: '/about',
      name: "about",
      component: About
    }

  ]
})


new Vue({
  el: "#app",
  router: router
})

// new Vue({
//   render: h => h(App)
// }).$mount('#app')


