<template>
  <f7-popup
    class="popup-add-access"
    swipe-to-close
    swipe-handler=".swipe-handler"
    :opened="show"
    @popup:opened="$emit('opened')"
    @popup:closed="$emit('closed')"
  >
    <f7-page
      name="user-select-component"
      ptr
      @ptr:refresh="refresh"
      infinite
      :infinite-distance="50"
      :infinite-preloader="loading"
      @infinite="loadMore"
    >
      <f7-navbar :title="$t('navigator.user_select')" class="swipe-handler">
        <f7-nav-right>
          <f7-link popup-close>{{$t("common.save")}}</f7-link>
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
          :title="item.UserName"
          :subtitle="item.UserId"
          v-for="(item,index) in datas"
          :key="index"
          link="#"
          @click="selectItem(index)"
        >
        <template #media>
          <f7-icon
            f7="checkmark_alt_circle_fill"
            color="#1890ff"
            size="24"
            v-if="users.indexOf(item.UserId)>-1"
          ></f7-icon>
          <f7-icon f7="circle" color="gray" size="24" v-else></f7-icon>
        </template>
        </f7-list-item>
      </f7-list>
      <f7-block class="text-align-center" v-if="datas.length===0&&isEnd">{{$t('common.no_data')}}</f7-block>
      <f7-block class="text-align-center" v-if="datas.length>0&&isEnd">---{{$t('common.end')}}---</f7-block>
    </f7-page>
  </f7-popup>
</template>

<script>
import ListBase from "./ListBase";
export default {
  name: "user-select-component",
  props: { show: { type: Boolean }, selected: { type: Array } },
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.user.getbasic,
    };
  },
  computed: {
    users: function () {
      var users = [];
      for (var i = 0; i < this.selected.length; i++) {
        users.push(this.selected[i].UserId);
      }
      return users;
    },
  },
  methods: {
    getQuerystring() {
      var url =
        "?pageIndex=" +
        this.pageIndex +
        "&pageSize=" +
        this.pageSize +
        "&filter=" +
        this.filter+"&orderField=UserName&orderType=asc";
      return url;
    },
    selectItem(index) {
      var item = this.datas[index];
      this.$emit("select", item);
    },
  },
};
</script>

<style scoped>
.popup-add-access {
  height: 80%;
  bottom: 0px;
  top: auto;
}
</style>
