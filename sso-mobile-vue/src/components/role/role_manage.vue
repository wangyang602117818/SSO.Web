<template>
  <f7-page
    name="role_manage"
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
  >
    <f7-navbar :title="$t('manage.role_manage')" :back-link="$t('common.back')">
      <f7-nav-right>
        <f7-link icon-f7="plus_circle" href="/roleadd/"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-searchbar
      disable-button-text
      :placeholder="$t('common.search')"
      :clear-button="true"
      @change="onSearch"
    ></f7-searchbar>
    <f7-list media-list>
      <f7-list-item
        v-for="item in datas"
        :link="'/roleupdate/' + item.Id"
        :title="item.Name"
        :subtitle="item.Description"
        :key="item.Id"
        swipeout
        @swipeout:delete="delRole(item.Id)"
      >
        <f7-swipeout-actions right>
          <f7-swipeout-button
            color="red"
            delete
            :confirm-title="$t('common.tips')"
            :confirm-text="$t('confirm.sure_delete')"
            >{{ $t("common.delete") }}</f7-swipeout-button
          >
        </f7-swipeout-actions>
      </f7-list-item>
    </f7-list>
    <f7-block class="text-align-center" v-if="datas.length === 0 && isEnd">{{
      $t("common.no_data")
    }}</f7-block>
    <f7-block class="text-align-center" v-if="datas.length > 0 && isEnd"
      >---{{ $t("common.end") }}---</f7-block
    >
  </f7-page>
</template>

<script>
import ListBase from "../ListBase";
export default {
  name: "role_manage",
  props: {
    f7router: Object,
  },
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.role.getlist,
    };
  },
  mounted() {},
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
    delRole(id) {
      this.$axios
        .post(this.$urls.role.delete, { ids: [id] })
        .then((response) => {
          if (response.code != 0) {
            this.getData(true);
          }
        });
    },
  },
};
</script>

<style scoped>
</style>
