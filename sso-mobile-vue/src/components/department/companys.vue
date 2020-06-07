<template>
  <f7-page
    name="coms"
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
  >
    <f7-navbar title="公司部门管理" back-link="返回">
      <f7-nav-right>
        <f7-link icon-f7="plus_circle" href="/companyadd/"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-searchbar disable-button-text placeholder="Search" :clear-button="true" @change="onSearch"></f7-searchbar>
    <f7-list media-list>
      <f7-list-item
        v-for="item in datas"
        swipeout
        :link="'/departmentmanage/'+item.Code+'/'+item.Name"
        :title="item.Name"
        :subtitle="item.Description||' '"
        :key="item.Id"
      >
        <f7-swipeout-actions right>
          <f7-swipeout-button color="blue" @click="updateCompany(item.Id)">Update</f7-swipeout-button>
          <f7-swipeout-button color="red" @click="delCompany(item.Id)">Delete</f7-swipeout-button>
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
  name: "coms",
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.company.getlist
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
    delCompany(id) {
      var that = this;
      this.$f7.dialog.confirm("确定删除?", "提示", function() {
        that.$axios
          .post(that.$urls.company.delete, { ids: [id] })
          .then(response => {
            if (response.code === 0) that.removeItem(id);
          });
      });
    },
    updateCompany(id) {
      this.$f7router.navigate("/companyupdate/" + id);
    }
  }
};
</script>

<style scoped>
</style>
