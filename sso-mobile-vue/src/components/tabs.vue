<template>
  <f7-page :page-content="false">
    <f7-toolbar tabbar bottom>
      <f7-link tab-link="#navigator" text="导航" icon-f7="flag_circle" icon-size="24px"></f7-link>
      <f7-link tab-link="#manage"  text="管理" icon-f7="square_grid_2x2_fill" icon-size="24px"></f7-link>
      <f7-link tab-link="#me" tab-link-active text="我的" icon-f7="person_crop_circle" icon-size="24px"></f7-link>
    </f7-toolbar>
    <f7-tabs>
      <f7-tab id="navigator" >
        <Navigator />
      </f7-tab>
      <f7-tab id="manage" >
        <Manage />
      </f7-tab>
      <f7-tab id="me" tab-active>
        <Me />
      </f7-tab>
    </f7-tabs>
    
  </f7-page>
</template>

<script>
import Navigator from "./navigator";
import Manage from "./manage";
import Me from "./me";
export default {
  name: "tabs",
  components: { Navigator, Manage, Me },
  data() {
    return {};
  },
  computed: {},
  methods: {}
};
</script>

<style scoped>
#me{
  height: 100%;
  background-color: #fff;
}
</style>
