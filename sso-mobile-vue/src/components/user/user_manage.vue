<template>
  <f7-page
    name="user_manage"
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
  >
    <f7-navbar title="用户管理" back-link="返回">
      <f7-nav-right>
        <f7-link icon-f7="person_crop_circle_fill_badge_plus" href="/useradd/"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-searchbar disable-button-text placeholder="Search" :clear-button="true" @change="onSearch"></f7-searchbar>
    <f7-list media-list>
      <f7-list-item
        v-for="item in datas"
        :link="'/userupdate/'+item.UserId"
        :title="item.UserName"
        :subtitle="item.CompanyName+'|'+item.DepartmentName"
        :key="item.Id"
        swipeout
      >
        <f7-skeleton-block style="width: 40px; height: 40px;border-radius: 50%" slot="media"></f7-skeleton-block>
        <f7-swipeout-actions right>
          <f7-swipeout-button color="blue" @click="removeUser(item.Id,item.UserId)">Delete</f7-swipeout-button>
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
  name: "user_manage",
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.user.getbasic
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
    removeUser(id, userId) {
      var that = this;
      this.$f7.dialog.confirm("确定删除?", "提示", function() {
        that.$axios
          .post(that.$urls.user.remove, { userIds: [userId] })
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
