<template>
  <div id="app">
    <f7-app :params="f7params">
      <f7-view main url="/"></f7-view>
    </f7-app>
  </div>
</template>

<script>
import routes from "./config/router";
export default {
  name: "App",
  data() {
    return {
      f7params: {
        name: "App",
        id: "sso-mobile-vue",
        theme: "ios",
        routes: routes,
        view: {
          pushState: true,
          stackPages: true
        }
      }
    };
  },
  components: {},
  created() {},
  mounted() {
    window.console.log(this.$device);
  }
};
</script>

<style>
.list {
  margin-top: 5px !important;
  margin-bottom: 5px !important;
}
.skeleton-block {
  background-color: #e0e0e0 !important;
}
.block {
  margin: 10px !important;
}
</style>
