<template>
  <f7-page name="role_manage" ptr @ptr:refresh="refresh" infinite :infinite-distance="50" :infinite-preloader="loading"
    @infinite="loadMore">
    <f7-navbar :title="$t('manage.role_manage')" :back-link="$t('common.back')">
      <f7-nav-right>
        <f7-link icon-f7="plus_circle" href="/roleadd/"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-searchbar disable-button-text :placeholder="$t('common.search')" :clear-button="true" @change="onSearch">
    </f7-searchbar>
    <f7-list media-list>
      <f7-list-item v-for="item in datas" :link="'/roleupdate/' + item.Id" :title="item.Name"
        :subtitle="item.Description" :key="item.Id" swipeout @swipeout:delete="delRole(item.Id)">
        <f7-swipeout-actions right>
          <f7-swipeout-button color="red" delete :confirm-title="$t('common.tips')"
            :confirm-text="$t('confirm.sure_delete')">{{ $t("common.delete") }}</f7-swipeout-button>
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
  mounted() { },
  created() {
    this.$eventbus.off("roleupdate").on("roleupdate", this.roleUpdate);
    this.$eventbus.off("roleadd").on("roleadd", this.roleAdd);
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
    roleUpdate(roleName) {
      this.getByRoleName(roleName, "update");
    },
    roleAdd(roleName) {
      this.getByRoleName(roleName, "add");
    },
    getByRoleName(roleName, type) {
      this.$axios
        .get(this.$urls.role.getByName + "?roleName=" + roleName)
        .then((response) => {
          if (response.code == 0) {
            this.addOrUpdateItem(response, type);
          }
        });
    },
    delRole(id) {
      this.$axios
        .post(this.$urls.role.delete, { ids: [id] })
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
