import Vue from 'vue'
import VueI18n from "vue-i18n"
import store from './store'
import App from './App.vue'
import Framework7 from 'framework7/framework7-lite.esm.bundle'
import Framework7Vue from 'framework7-vue'
import funtools from 'sso-util'
import { baseURL, cookieName, urls, axios } from './config/http'

import 'framework7/css/framework7.bundle.css'
import 'framework7-icons'

import { f7App, f7Tabs, f7Tab, f7Toolbar, f7View, f7Page, f7Link, f7Navbar, f7NavRight, f7Icon, f7Row, f7Col, f7Searchbar, f7List, f7ListItem, f7Preloader, f7Block, f7SkeletonBlock, f7ListInput, f7SwipeoutActions, f7SwipeoutButton, f7Sheet, f7PageContent, f7BlockTitle, f7Button, f7Chip, f7Popup } from "framework7-vue"

Vue.component('f7-app', f7App);
Vue.component('f7-view', f7View);
Vue.component('f7-page', f7Page);
Vue.component('f7-link', f7Link);
Vue.component('f7-navbar', f7Navbar);
Vue.component('f7-nav-right', f7NavRight);
Vue.component('f7-tabs', f7Tabs);
Vue.component('f7-toolbar', f7Toolbar);
Vue.component('f7-tab', f7Tab);
Vue.component('f7-icon', f7Icon);
Vue.component('f7-row', f7Row);
Vue.component('f7-col', f7Col);
Vue.component('f7-searchbar', f7Searchbar);
Vue.component('f7-list', f7List);
Vue.component('f7-list-item', f7ListItem);
Vue.component('f7-preloader', f7Preloader);
Vue.component('f7-block', f7Block);
Vue.component('f7-skeleton-block', f7SkeletonBlock);
Vue.component('f7-list-input', f7ListInput);
Vue.component('f7-swipeout-actions', f7SwipeoutActions);
Vue.component('f7-swipeout-button', f7SwipeoutButton);
Vue.component('f7-sheet', f7Sheet);
Vue.component('f7-page-content', f7PageContent);
Vue.component('f7-block-title', f7BlockTitle);
Vue.component('f7-button', f7Button);
Vue.component("f7-chip", f7Chip);
Vue.component("f7-popover", f7Popup);

Framework7.use(Framework7Vue);

Vue.prototype.$funtools = funtools;
funtools.authorize(baseURL, cookieName);
//语言信息
Vue.use(VueI18n);
const i18n = new VueI18n({
  locale: window.token_jwt_data.Lang,
  messages: {
      'zh-cn': require('./locales/zh-cn.json'),
      'en-us': require('./locales/en-us.json')
  }
});

Vue.prototype.$urls = urls
Vue.prototype.$axios = axios
Vue.prototype.$cookieName = cookieName
Vue.prototype.showSuccess = function () {
  this.$f7.toast.show({
    icon: '<i class="f7-icons">checkmark_alt_circle</i>',
    position: "center",
    text: "Success",
    closeTimeout: 1000
  });
}
Vue.prototype.showInfo = function (info) {
  this.$f7.toast.show({
    icon: '<i class="f7-icons">info_circle</i>',
    position: "center",
    text: info,
    closeTimeout: 1500
  });
}
Vue.config.productionTip = false;
var vue = new Vue({
  i18n,
  store,
  render: h => h(App),
});
window.vue = vue;
vue.$mount('#app')
