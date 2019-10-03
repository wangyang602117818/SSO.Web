import Vue from 'vue'
import VueRouter from 'vue-router'
import VueResource from 'vue-resource'

import common from './js/common.js'

import Login from '@/components/login'

import { Button, Icon, Input, Form, Row, Col, message, Card } from 'ant-design-vue'
import 'ant-design-vue/dist/antd.css'

Vue.use(Button)
Vue.use(Icon)
Vue.use(Input)
Vue.use(Form)
Vue.use(Row)
Vue.use(Col)
Vue.use(Card)

Vue.prototype.$message = message
Vue.prototype.$common = common

Vue.use(VueRouter)
Vue.use(VueResource)

Vue.http.options.root = 'http://www.sso.com:8030/'
var urls = {
    login: 'sso/login'
};
Vue.prototype.$urls = urls
Vue.config.productionTip = false

const router = new VueRouter({
    mode: 'history',
    routes: [
        {
            path: '/',
            name: "login",
            component: Login
        },
        {
            path: '/redirect',
            name: 'redirect',
            beforeEnter(to, from, next) {
                window.location = to.params.returnUrl;
            }
        }
    ]
})
new Vue({
    el: "#app",
    router: router
})