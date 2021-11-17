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
    <f7-navbar :title="$t('manage.user_manage')" :back-link="$t('common.back')">
      <f7-nav-right>
        <f7-link
          icon-f7="person_crop_circle_fill_badge_plus"
          href="/useradd/"
        ></f7-link>
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
        :link="'/userupdate/' + item.UserId"
        :title="item.UserName"
        :subtitle="item.CompanyName + '|' + item.DepartmentName"
        :key="item.Id"
        swipeout
        @swipeout:delete="removeUser(item.UserId)"
      >
        <template #media>
          <f7-skeleton-block
            style="width: 40px; height: 40px; border-radius: 50%"
          >
            <img
              :src="
                $axios.defaults.baseURL +
                $urls.file.downloadPic +
                '/' +
                item.FileId +
                '/' +
                item.FileName
              "
              v-if="item.FileId"
            />
          </f7-skeleton-block>
        </template>
        <f7-swipeout-actions right>
          <f7-swipeout-button
            delete
            color="red"
            :confirm-title="$t('common.tips')"
            :confirm-text="$t('confirm.sure_delete')"
            >Delete</f7-swipeout-button
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
  name: "user_manage",
  mixins: [ListBase],
  props: {
    f7router: Object
  },
  data() {
    return {
      getlist: this.$urls.user.getbasic,
    };
  },
  created(){
    this.$eventbus.on('userupdate',function(data){
      alert('x');
    });
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
    removeUser(userId) {
      this.$axios
        .post(this.$urls.user.remove, { userIds: [userId] })
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
img {
  width: 40px;
  height: 40px;
  border-radius: 50%;
}
</style>
