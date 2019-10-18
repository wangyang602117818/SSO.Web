import Vue from 'vue'
import VueRouter from 'vue-router'
import VueResource from 'vue-resource'
import babelPolyfill from 'babel-polyfill'

import Home from '@/components/home'
import OverView from '@/components/overview'
import UserBasic from '@/components/user_basic'
import Role from '@/components/role'
import Company from '@/components/company'
import Department from '@/components/department'
import Log from '@/components/log'
import Navigation from '@/components/navigation'
import common from './js/common.js'
import "@/css/index.css"

import { Button, Icon, Layout, Menu, Table, Input, Select, TreeSelect, InputNumber, Drawer, Form, Row, Col, message, Popconfirm, Tabs, Tree, Divider, Tag, Switch, Tooltip, Card, Dropdown } from 'ant-design-vue'
import 'ant-design-vue/dist/antd.css'

Vue.use(Button)
Vue.use(Icon)
Vue.use(Layout)
Vue.use(Menu)
Vue.use(Table)
Vue.use(Input)
Vue.use(InputNumber)
Vue.use(Select)
Vue.use(Drawer)
Vue.use(Form)
Vue.use(Row)
Vue.use(Col)
Vue.use(Tree)
Vue.use(TreeSelect)
Vue.use(Tabs)
Vue.use(Popconfirm)
Vue.use(Divider)
Vue.use(Tag)
Vue.use(Switch)
Vue.use(Tooltip)
Vue.use(Dropdown)
Vue.use(Card)

Vue.prototype.$message = message
Vue.prototype.$common = common

Vue.use(VueRouter)
Vue.use(VueResource)
Vue.use(babelPolyfill)

Vue.http.options.root = 'http://www.sso.com:8030/'
Vue.http.interceptors.push(function (request, next) {//拦截器
  request.credentials = true;    // 跨域携带cookie
  next(response => {
    if (response.body.code == 401) {
      window.location.href = Vue.http.options.root + urls.login + "?returnUrl=" + window.location.href;
      return false;
    }
    return response;
  });
})

var urls = {
  role: {
    add: "role/add",
    update: "role/update",
    delete: "role/delete",
    getlist: "role/getlist",
    getall: "role/getall",
    getById: "role/getbyid"
  },
  company: {
    add: "company/add",
    update: "company/update",
    delete: "company/delete",
    getlist: "company/getlist",
    getall: "company/getall",
    getById: "company/getbyid"
  },
  department: {
    add: "department/add",
    getdepartments: "department/getdepartments",
    get: "department/get",
    update: "department/update",
    delete: "department/delete"
  },
  user: {
    add: "user/add",
    getbasic: "user/getbasic",
    remove: "user/remove",
    delete: "user/delete",
    update: "user/update",
    restore: "user/restore",
    getbyuserid: "user/getbyuserid"
  },
  navigation: {
    add: "navigation/add",
    getlist: "navigation/getlist",
  },
  login: 'sso/login',
  logout: 'sso/logout'
}
Vue.prototype.$urls = urls
Vue.config.productionTip = false

const router = new VueRouter({
  mode: 'history',
  routes: [
    {
      path: '/',
      component: Home,
      children: [
        {
          path: '',
          component: OverView
        },
        {
          path: 'index.html',
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
          path: 'role',
          name: "role",
          component: Role
        },
        {
          path: 'company',
          name: "company",
          component: Company
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
          path: 'navigation',
          name: "navigation",
          component: Navigation
        },
      ]
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


