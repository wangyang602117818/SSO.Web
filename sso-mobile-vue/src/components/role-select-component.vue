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
      name="role-select-component"
      ptr
      @ptr:refresh="refresh"
      infinite
      :infinite-distance="50"
      :infinite-preloader="loading"
      @infinite="loadMore"
    >
      <f7-navbar :title="$t('navigator.role_select')" class="swipe-handler">
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
          :title="item.Name"
          :subtitle="item.Description"
          v-for="(item,index) in datas"
          :key="index"
          link="#"
          @click="selectItem(index)"
        >
          <f7-icon
            slot="media"
            f7="checkmark_alt_circle_fill"
            color="#1890ff"
            size="24"
            v-if="itemSelected(item)"
          ></f7-icon>
          <f7-icon slot="media" f7="circle" color="gray" size="24" v-else></f7-icon>
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
  name: "role-select-component",
  props: { show: { type: Boolean }, selected: { type: Array } },
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.role.getlist,
    };
  },
  created() {},
  computed: {},
  methods: {
    itemSelected(item) {
      var count = this.selected.find((sel) => sel.Name == item.Name);
      return count > 0;
    },
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
    selectItem(index) {
      var item = this.datas[index];
      this.$emit("select",item);
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
