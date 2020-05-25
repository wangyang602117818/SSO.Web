<template>
  <f7-page
    name="home"
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
  >
    <f7-navbar title="角色管理" back-link="返回">
      <f7-nav-right>
        <f7-link icon-f7="plus_circle" href="/roleadd/"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-searchbar disable-button-text placeholder="Search" :clear-button="true" @change="onSearch"></f7-searchbar>
    <f7-list media-list>
      <f7-list-item
        v-for="item in datas"
        :link="'/roleupdate/'+item._id"
        :title="item.Name"
        :subtitle="item.Description"
        :key="item._id"
      ></f7-list-item>
    </f7-list>
    <f7-block class="text-align-center" v-if="datas.length===0&&isEnd">没有数据</f7-block>
    <f7-block class="text-align-center" v-if="datas.length>0&&isEnd">---end---</f7-block>
  </f7-page>
</template>

<script>
import ListBase from "../ListBase";
export default {
  name: "role_manage",
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.role.getlist
    };
  },
  methods: {}
};
</script>

<style scoped>
</style>
