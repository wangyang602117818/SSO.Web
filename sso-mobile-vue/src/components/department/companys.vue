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
    <f7-navbar
      :title="$t('manage.department_manage')"
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
        :link="'/departmentmanage/' + item.Code + '/' + item.Name"
        :title="item.Name"
        :subtitle="item.Description || ' '"
        :key="item.Id"
        swipeout
        @swipeout:delete="delCompany(item.Id)"
      >
        <f7-swipeout-actions right>
          <f7-swipeout-button color="blue" @click="updateCompany(item.Id)">{{
            $t("common.update")
          }}</f7-swipeout-button>
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
  name: "coms",
  mixins: [ListBase],
  props: {
    f7router: Object,
  },
  data() {
    return {
      getlist: this.$urls.company.getlist,
    };
  },
  created() {
    this.$eventbus.on("companyupdate", this.getByCompanyId);
    this.$eventbus.on("companyadd", this.getByCompanyId);
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
    getByCompanyId() {
      this.pageIndex = 1;
      this.isEnd = false;
      this.getData(true, null);
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
    updateCompany(id) {
      this.f7router.navigate("/companyupdate/" + id);
    },
  },
};
</script>

<style scoped>
</style>
