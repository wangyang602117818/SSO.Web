<template>
  <f7-page
    name="navigation_manage"
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
  >
    <f7-navbar title="导航管理" back-link="返回">
      <f7-nav-right>
        <f7-link icon-f7="plus_circle" href="/navigationadd/"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-searchbar disable-button-text placeholder="Search" :clear-button="true" @change="onSearch"></f7-searchbar>
    <f7-list media-list>
      <f7-list-item
        v-for="item in datas"
        swipeout
        :link="'/navigationupdate/'+item.Id"
        :title="item.Title"
        :subtitle="item.BaseUrl"
        :key="item.Id"
      >
        <img slot="media" :src="item.LogoUrl" width="40" />
        <f7-swipeout-actions right>
          <f7-swipeout-button color="red" @click="delNavigation(item.Id)">Delete</f7-swipeout-button>
        </f7-swipeout-actions>
      </f7-list-item>
    </f7-list>
    <f7-block class="text-align-center" v-if="datas.length===0&&isEnd">没有数据</f7-block>
    <f7-block class="text-align-center" v-if="datas.length>0&&isEnd">---end---</f7-block>
  </f7-page>
</template>

<script>
import ListBase from "../ListBase";
export default {
  name: "navigation_manage",
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.navigation.getlist
    };
  },
  methods: {
    getQuerystring() {
      var url =
        "?pageIndex=" +
        this.pageIndex +
        "&pageSize=" +
        this.pageSize +
        "&filter=" +
        this.filter;
      return url;
    },
    delNavigation(id) {
      var that = this;
      this.$f7.dialog.confirm("确定删除?", "提示", function() {
        that.$axios
          .post(that.$urls.navigation.delete, { ids: [id] })
          .then(response => {
            if (response.code === 0) that.removeItem(id);
          });
      });
    }
  }
};
</script>

<style scoped>
</style>
