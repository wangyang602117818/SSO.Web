import Vue from 'vue'
import VueResource from 'vue-resource'

import Logout from '@/components/logout'

import common from './js/common.js'

import { Row, Col, Divider, Card, Avatar } from 'ant-design-vue'
import 'ant-design-vue/dist/antd.css'

Vue.use(Row)
Vue.use(Col)
Vue.use(Divider)
Vue.use(Card)
Vue.use(Avatar)

Vue.use(VueResource)
Vue.prototype.$common = common

Vue.http.options.root = ''
var urls = {
    geturlmeta: 'sso/geturlmeta'
};
Vue.prototype.$urls = urls

new Vue({
    render: h => h(Logout),
}).$mount('#app')