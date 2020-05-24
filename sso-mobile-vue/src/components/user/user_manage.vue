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
        :key="item._id"
      >
        <f7-skeleton-block style="width: 40px; height: 40px;border-radius: 50%" slot="media"></f7-skeleton-block>
      </f7-list-item>
    </f7-list>
    <f7-block class="text-align-center" v-if="datas.length===0&&isEnd">没有数据</f7-block>
    <f7-block class="text-align-center" v-if="datas.length>0&&isEnd">---end---</f7-block>
  </f7-page>
</template>

<script>
export default {
  name: "user_manage",
  data() {
    return {
      datas: [],
      filter: "",
      isEnd: false,
      loading: true,
      pageIndex: 1,
      pageSize: 15
    };
  },
  created() {
    this.getData();
  },
  methods: {
    onSearch(event) {
      var value = event.target.value;
      this.filter = value;
      this.pageIndex = 1;
      this.isEnd = false;
      this.getData(null, true);
    },
    refresh(done) {
      this.pageIndex = 1;
      this.getData(done, true);
    },
    loadMore() {
      //正在loading数据或者所有数据已经加载完成
      if (this.loading || this.isEnd) return;
      this.pageIndex = this.pageIndex + 1;
      this.getData(null, false);
    },
    getData(callback, replace) {
      this.loading = true;
      this.$axios
        .get(
          this.$urls.user.getbasic +
            "?pageIndex=" +
            this.pageIndex +
            "&pageSize=" +
            this.pageSize +
            "&filter=" +
            this.filter
        )
        .then(response => {
          if (callback) callback();
          this.loading = false;
          if (response.code === 0) {
            if (response.result.length < this.pageSize) this.isEnd = true;
            if (replace) {
              this.datas = response.result;
            } else {
              this.datas = this.datas.concat(response.result);
            }
          }
        });
    }
  }
};
</script>

<style scoped>
</style>
