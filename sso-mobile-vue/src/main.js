import { createApp } from 'vue'
import { createI18n } from "vue-i18n"
import store from './store'
import App from './App.vue'
import Framework7 from 'framework7/lite-bundle';
import Framework7Vue, { registerComponents } from 'framework7-vue/bundle';
import { f7 } from 'framework7-vue';
import funtools from 'sso-util'
import { baseURL, cookieName, urls, axios } from './config/http'

import 'framework7-icons';
import "framework7/framework7-bundle.css";

Framework7.use(Framework7Vue);

funtools.authorize(baseURL, cookieName);
const i18n = createI18n({
  locale: window.token_jwt_data.Lang,
  messages: {
    'zh-cn': require('./locales/zh-cn.json'),
    'en-us': require('./locales/en-us.json')
  }
});
const app = createApp(App);
app.use(store);
app.use(i18n);

var eventbus = new Framework7.Events();

app.config.globalProperties.$funtools = funtools;
app.config.globalProperties.$urls = urls;
app.config.globalProperties.$axios = axios;
app.config.globalProperties.$cookieName = cookieName;
app.config.globalProperties.$eventbus = eventbus;

app.config.globalProperties.showSuccess = function () {
  f7.toast.show({
    icon: '<i class="f7-icons">checkmark_alt_circle</i>',
    position: "center",
    text: "Success",
    closeTimeout: 1000
  });
};
app.config.globalProperties.showInfo = function (info) {
  f7.toast.show({
    icon: '<i class="f7-icons">info_circle</i>',
    position: "center",
    text: info,
    closeTimeout: 1500
  });
}
registerComponents(app);

const vue = app.mount('#app')
window.vue = vue;
