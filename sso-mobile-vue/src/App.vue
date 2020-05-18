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
      // app params
        f7params: {
          name: 'App',
          id: 'com.myapp.test',
          // specify routes for app
          routes: routes
        }
    };
  },
  components: {},
  mounted() {
    window.console.log(this.$device);
  }
};
</script>

<style scoped>
</style>
