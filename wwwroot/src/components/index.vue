<template>
  <div class="container">
    <div class="top">
      <div class="login">
        <div
          class="login_user"
          @mouseleave="show_logout=false"
          @mouseover="show_logout=true"
          v-if="user"
        >
          <span>{{user.UserName}}</span>
        </div>
        <div class="login_user" @click="login" v-else>
          <span>{{this.lang.login}}</span>
        </div>
        <div
          class="login_tools"
          @mouseleave="show_logout=false"
          @mouseover="show_logout=true"
          v-if="user"
          v-show="show_logout"
        >
         <a class="tools_item" target="_self" @click="changeLang">{{this.user.Lang=="en-us"?'中':'EN'}}</a>
          <a class="tools_item" target="_self" :href="this.$urls.logout">{{this.lang.logout}}</a>
        </div>
      </div>
    </div>
    <div class="search">
      <input type="text" name="search" :placeholder="this.lang.search" />
    </div>
    <div class="main">
      <div class="main_tab">
        <div class="tab_title">{{this.lang.my_navigation}}</div>
        <div class="tab_data_line" v-for="(item,index) in urls" :key="index">
          <div class="data_site_wrap" v-for="(u,index) in item" :key="index">
            <a class="data_site" v-if="u" :href="u.BaseUrl" :title="u.BaseUrl" target="_blank">
              <div class="logo">
                <span v-if="u.LogoUrl">
                  <img alt="icon" :src="u.LogoUrl" />
                </span>
                <span v-else>
                  <span class="avatar">{{$funtools.trimChar($funtools.getFileName(u.Title,1),".")}}</span>
                </span>
              </div>
              <div class="title" :title="u.Title">{{$funtools.getFileName(u.Title,3)}}</div>
            </a>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import langEn from "../lang/en-US";
import langZh from "../lang/zh-CN";
export default {
  name: "index",
  data() {
    return {
      urls: [],
      len: 5,
      user: null,
      lang: langZh,
      show_logout: false
    };
  },
  created() {
    this.getUrlMeta();
    this.getUser();
  },
  methods: {
    login() {
      window.location.href = this.$http.options.root + this.$urls.login;
    },
    getFileName(fileName, length) {
      return this.$funtools.getFileName(fileName, length);
    },
    getUrlMeta() {
      this.$http.get(this.$urls.geturlmeta).then(response => {
        if (response.body.code == 0)
          this.urls = this.$funtools.reMapArray(response.body.result, this.len);
      });
    },
    getUser() {
      this.$http.get(this.$urls.decodetoken).then(response => {
        if (response.body.code == 0 && response.body.result) {
          this.user = response.body.result;
          this.lang = response.body.result.Lang == "en-us" ? langEn : langZh;
        }
      });
    },
    changeLang() {
      var lang = this.user.Lang == "en-us" ? "zh-cn" : "en-us";
      this.$http
        .get(this.$urls.setLang + "?lang=" + lang)
        .then(response => {
          if (response.body.code == 0) {
            location.reload();
          }
        });
    }
  }
};
</script>
<style>
* {
  margin: 0;
  padding: 0;
  font-size: 14px;
  font-family: -apple-system, BlinkMacSystemFont, segoe ui, Roboto,
    helvetica neue, Arial, noto sans, sans-serif, apple color emoji,
    segoe ui emoji, segoe ui symbol, noto color emoji;
}
body {
  background-color: #e2e2e2;
}
a {
  text-decoration: none;
  color: #666;
}
.top {
  border: 0;
  display: flex;
  height: 40px;
  align-items: center;
  justify-content: flex-end;
  padding-right: 10px;
  background-color: rgba(200, 200, 200, 1);
  box-shadow: 0 0 1px #888;
}
.login {
  height: 40px;
  display: flex;
  align-items: center;
  justify-content: center;
  cursor: pointer;
  color: #666;
  font-size: 13px;
  position: relative;
}
.login_user {
  margin: 0 10px;
  height: 100%;
  display: flex;
  align-items: center;
  justify-content: center;
}
.login_user:hover {
  color: #000;
}
.login_tools {
  position: absolute;
  height: 80px;
  width: 100%;
  left: -1px;
  top: 40px;
  border-radius: 5px;
  display: flex;
  flex-direction: column;
  background-color: #fff;
  box-shadow: 0 2px 10px 0 rgba(0, 0, 0, 0.15);
  -webkit-box-shadow: 0 2px 10px 0 rgba(0, 0, 0, 0.15);
  -moz-box-shadow: 0 2px 10px 0 rgba(0, 0, 0, 0.15);
  -o-box-shadow: 0 2px 10px 0 rgba(0, 0, 0, 0.15);
}
.login_tools .tools_item {
  height: 35px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.login_tools .tools_item:hover {
  color: #4e71f2;
}
.search {
  height: 120px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.search input {
  width: 40%;
  padding: 8px 10px;
  border: 1px solid #ddd;
  outline: none;
  background-color: #fff;
}
.main {
  display: flex;
  align-items: center;
  justify-content: center;
}
.main_tab {
  width: 40%;
  padding: 8px 10px;
  border: 1px solid #eee;
  min-height: 300px;
  background-color: #fff;
}
.tab_title {
  height: 35px;
  display: flex;
  align-items: center;
  justify-content: left;
  color: #666;
  border-bottom: 1px solid #eee;
  margin: 5px;
}
.tab_data_line {
  height: 100px;
  position: relative;
  display: flex;
  padding-left: -50px;
}
.data_site_wrap {
  flex: 1;
  margin: 5px;
}
.data_site {
  border: 1px solid #eee;
  height: 100%;
  display: flex;
  flex-direction: column;
  text-align: center;
  justify-content: center;
  overflow: hidden;
  background-color: #f0f2f5;
}
.data_site:hover {
  background-color: #f4f4f4;
  cursor: pointer;
}
.logo {
  flex: 2;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
}
.logo img {
  height: 40px;
  width: 40px;
}
.title {
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: flex-start;
  width: 100%;
}
.avatar {
  display: inline-block;
  height: 40px;
  width: 40px;
  border-radius: 20px;
  display: flex;
  justify-content: center;
  align-items: center;
  background-color: #3498db;
}
</style>