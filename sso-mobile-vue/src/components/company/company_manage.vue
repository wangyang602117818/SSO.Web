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
    <f7-navbar
      :title="$t('common.company') + $t('common.manage')"
      :back-link="$t('common.back')"
    >
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
    <f7-list media-list>
      <f7-list-item
        v-for="item in datas"
        :link="'/companyupdate/' + item.Id"
        :title="item.Name"
        :subtitle="item.Description || ' '"
        :key="item.Id"
        swipeout
        @swipeout:delete="delCompany(item.Id)"
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
      <f7-block-footer v-if="datas.length === 0 && isEnd">
        <p style="text-align: center">---{{ $t("common.no_data") }}---</p>
      </f7-block-footer>
      <f7-block-footer v-if="datas.length > 0 && isEnd">
        <p style="text-align: center">---{{ $t("common.end") }}---</p>
      </f7-block-footer>
    </f7-list>
  </f7-page>
</template>

<script>
import ListBase from "../ListBase";
export default {
  name: "company_manage",
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.company.getlist,
    };
  },
  created() {
    this.$eventbus.on("companyupdate", this.companyUpdate);
    this.$eventbus.on("companyadd", this.companyAdd);
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
    companyUpdate(id) {
      this.getByCompanyId(id, "update");
    },
    companyAdd(id) {
      this.getByCompanyId(id, "add");
    },
    getByCompanyId(id, type) {
      this.$axios
        .post(this.$urls.company.getById + "/" + id)
        .then((response) => {
          if (response.code == 0) {
            this.addOrUpdateItem(response, type);
          }
        });
    },
    delCompany(id) {
      this.$axios
        .post(this.$urls.company.delete, { ids: [id] })
        .then((response) => {
          if (response.code != 0) {
            this.f7router.refreshPage();
          }
        });
    },
  },
};
</script>

<style scoped>
</style>
