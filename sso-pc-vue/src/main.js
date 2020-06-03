import Vue from 'vue'
import VueRouter from 'vue-router'
import funtools from 'sso-util'
import { baseURL, cookieName, urls, axios } from './config/http';
import routes from "./config/routes";

import langEn from './lang/en-US'
import langZh from './lang/zh-CN'
import "./css/index.css"

import { Button, Icon, Layout, Menu, Table, Input, Select, TreeSelect, InputNumber, Drawer, Form, Row, Col, message, notification, Popconfirm, Tabs, Tree, Divider, Tag, Switch, Tooltip, Card, Dropdown, Spin, DatePicker } from 'ant-design-vue'

Vue.use(VueRouter)

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
Vue.use(Spin)
Vue.use(DatePicker)

Vue.prototype.$message = message
Vue.prototype.$notification = notification
Vue.prototype.$funtools = funtools
Vue.prototype.$baseUrl = baseURL
Vue.prototype.$cookieName = cookieName
Vue.prototype.$urls = urls
Vue.prototype.$axios = axios

funtools.authorize(baseURL, cookieName);
Vue.prototype.$lang = (window.token_jwt_data.Lang == "en-us") ? langEn : langZh

Vue.config.productionTip = false
var vue = new Vue({
  el: "#app",
  router: routes
})
window.vue = vue;
