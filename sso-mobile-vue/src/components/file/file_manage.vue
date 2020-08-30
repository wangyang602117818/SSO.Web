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
    <f7-list media-list>
      <f7-list-item
        v-for="item in datas"
        :key="item._id"
        link="#"
        :title="item.FileName"
        :after="''"
        :subtitle="item.CreateTime+' | '+item.Percent+'%'"
        :text="$funtools.convertFileSize(item.Length)+' | ' +item.Download+' | ' + item.From"
      >
        <img
          slot="media"
          :src="$axios.defaults.baseURL+$urls.file.downloadPic+'/'+item._id+'/'+item.FileName"
          width="80"
        />
      </f7-list-item>
    </f7-list>
    <f7-block class="text-align-center" v-if="datas.length===0&&isEnd">{{$t('common.no_data')}}</f7-block>
    <f7-block class="text-align-center" v-if="datas.length>0&&isEnd">---{{$t('common.end')}}---</f7-block>
  </f7-page>
</template>

<script>
import ListBase from "../ListBase";
export default {
  name: "file_manage",
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.file.getlist,
      orderBy: "CreateTime",
      orderType: "desc",
      from: "",
      startTime: "",
      endTime: "",
      filterFileType: "",
      fileDelete: false,
    };
  },
  methods: {
    getQuerystring() {
      var url =
        "?pageIndex=" +
        this.pageIndex +
        "&pageSize=" +
        this.pageSize +
        "&sorts[0].key=" +
        this.orderBy +
        "&sorts[0].value=" +
        this.orderType;
      if (this.from) url += "&from=" + this.from;
      if (this.filter) url += "&filter=" + this.filter;
      if (this.startTime) url += "&startTime=" + this.startTime;
      if (this.endTime) url += "&endTime=" + this.endTime;
      if (this.filterFileType) url += "&fileType=" + this.filterFileType;
      url += "&delete=" + this.fileDelete;
      return url;
    },
  },
};
</script>

<style scoped>
</style>
