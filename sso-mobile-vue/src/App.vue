<template>
  <f7-app v-bind="f7params">
    <f7-view main url="/"></f7-view>
  </f7-app>
</template>

<script>
import routes from "./config/router.js";
import { getDevice } from "framework7";
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
          stackPages: true,
        },
      },
    };
  },
  components: {},
  created() {},
  mounted() {
    const device = getDevice();
    window.console.log(device);
  },
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
/*/////////////// */
.setting_line {
  display: flex;
  height: 50px;
  width: 100%;
  align-items: center;
  border-bottom: 1px solid #e3e4e4;
}
.setting_line:active {
  background-color: #ddd;
}
.setting_line .setting_line_name {
  flex: 1;
  display: flex;
}

.setting_line .setting_line_right {
  text-align: center;
  display: flex;
  align-items: center;
}

.setting_line .setting_line_right .setting_line_data {
  display: inline-block;
  color: #888;
}

.setting_line_v {
  display: flex;
  width: 100%;
  flex-direction: column;
  align-items: left;
}

.setting_line_v .setting_line_name {
  flex: 1;
  width: 100%;
  display: flex;
  align-items: center;
}

.setting_line_v .setting_line_data {
  flex: 1;
  width: 100%;
  display: flex;
  align-items: flex-end;
}
.bold {
  font-weight: bold;
}
</style>
