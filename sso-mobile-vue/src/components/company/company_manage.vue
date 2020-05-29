<template>
  <f7-page
    name="company_manage"
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
  >
    <f7-navbar title="公司管理" back-link="返回">
      <f7-nav-right>
        <f7-link icon-f7="plus_circle" href="/companyadd/"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-searchbar disable-button-text placeholder="Search" :clear-button="true" @change="onSearch"></f7-searchbar>
    <f7-list media-list>
      <f7-list-item
        v-for="item in datas"
        swipeout
        :link="'/companyupdate/'+item.Id"
        :title="item.Name"
        :subtitle="item.Description||' '"
        :key="item.Id"
      >
        <f7-swipeout-actions right>
          <f7-swipeout-button color="blue" @click="delCompany(item.Id)">Delete</f7-swipeout-button>
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
  name: "company_manage",
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.company.getlist
    };
  },
  methods: {
    delCompany(id) {
      var that = this;
      this.$f7.dialog.confirm("确定删除?", "提示", function() {
        that.$axios
          .post(that.$urls.company.delete, { ids: [id] })
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
