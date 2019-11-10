import Vue from 'vue'
import VueResource from 'vue-resource'

import Logout from '@/components/logout'

import common from './js/common.js'

import { Row, Col, Divider, Card, Avatar,Select,Icon,Dropdown,Modal,Form,Input } from 'ant-design-vue'
import 'ant-design-vue/dist/antd.css'

Vue.use(Row)
Vue.use(Col)
Vue.use(Divider)
Vue.use(Card)
Vue.use(Avatar)
Vue.use(Select)
Vue.use(Icon)
Vue.use(Dropdown)
Vue.use(Modal)
Vue.use(Form)
Vue.use(Input)

Vue.use(VueResource)
Vue.prototype.$common = common

Vue.http.options.root = 'http://www.sso.com:8030/'
var urls = {
    geturlmeta: 'sso/geturlmeta'
};
Vue.prototype.$urls = urls

new Vue({
    render: h => h(Logout),
}).$mount('#app')