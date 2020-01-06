import Vue from 'vue'
import VueResource from 'vue-resource'
import babelPolyfill from 'babel-polyfill'

import Logout from '@/components/logout'

import common from './js/common.js'
// import authorize from './js/authorize.js'

import { Button, Row, Col, Divider, Tabs, Card, Avatar, Select, Icon, Dropdown, Modal, Form, Input, message, Menu, Popconfirm } from 'ant-design-vue'
import 'ant-design-vue/dist/antd.css'

Vue.use(Button)
Vue.use(Row)
Vue.use(Col)
Vue.use(Divider)
Vue.use(Tabs)
Vue.use(Card)
Vue.use(Avatar)
Vue.use(Select)
Vue.use(Icon)
Vue.use(Dropdown)
Vue.use(Modal)
Vue.use(Form)
Vue.use(Input)
Vue.use(Menu)
Vue.use(Popconfirm)

Vue.prototype.$message = message
Vue.prototype.$common = common

Vue.use(VueResource)
Vue.use(babelPolyfill)

// Vue.http.options.root = 'http://www.sso.com:8030/'
Vue.http.options.root = ''
// var cookieName = 'sso.manage.auth';
// Vue.http.interceptors.push(function (request, next) {//拦截器
//     // request.credentials = true;    // 跨域携带cookie
//     request.headers.set('Authorization', authorize.getCookie(cookieName));
//     next(response => {
//       if (response.body.code == 401) {
//         // window.location.href = Vue.http.options.root + urls.login + "?returnUrl=" + window.location.href;
//         this.$message.warning("登陆已过期!");
//         return false;
//       }
//       return response;
//     });
//   })

var urls = {
    geturlmeta: 'sso/geturlmeta',
    login: 'sso/login',
    logout: 'sso/logout',
    decodetoken: 'sso/decodetoken',
    addnavigation: 'sso/addnavigation',
    deletenavigation: 'sso/deletenavigation',
    getnavigationbyid: 'sso/getnavigationbyid',
    updatenavigation: 'sso/updatenavigation'
};
Vue.prototype.$urls = urls

new Vue({
    render: h => h(Logout),
}).$mount('#app')