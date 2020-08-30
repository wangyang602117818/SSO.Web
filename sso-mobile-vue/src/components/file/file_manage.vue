<template>
  <f7-page
    name="file_manage"
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
  >
    <f7-navbar :title="$t('navigator.file_manage')" :back-link="$t('common.back')">
      <f7-nav-right>
        <f7-link icon-f7="plus_circle" href="/companyadd/"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-searchbar
      disable-button-text
      :placeholder="$t('common.search')"
      :clear-button="true"
      @change="onSearch"
    ></f7-searchbar>
  </f7-page>
</template>

<script>
import ListBase from "../ListBase";
export default {
  name: "file_manage",
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.company.getlist,
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
  },
};
</script>

<style scoped>
</style>
