<template>
  <f7-page
    name="file_manage"
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
    class="filemanage"
  >
    <f7-navbar
      :title="$t('navigator.file_manage')"
      :back-link="$t('common.back')"
    >
      <f7-nav-right>
        <f7-link
          icon-f7="hourglass_tophalf_fill"
          sheet-open=".sheet-top"
        ></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-sheet top backdrop class="sheet-top">
      <f7-toolbar bottom>
        <div class="left">
          <f7-link sheet-close @click="onClear">{{
            $t("common.clear")
          }}</f7-link>
        </div>
        <div class="right">
          <f7-link sheet-close @click="onSearch">{{ $t("common.ok") }}</f7-link>
        </div>
      </f7-toolbar>
      <f7-page-content>
        <f7-block-title>{{ $t("common.from") }}</f7-block-title>
        <f7-block strong>
          <f7-chip
            :color="item == from ? 'blue' : ''"
            :text="item"
            v-for="(item, index) in froms"
            :key="index"
            @click="fromClick"
          ></f7-chip>
        </f7-block>
        <f7-block-title>{{ $t("navigator.file_type") }}</f7-block-title>
        <f7-block strong>
          <f7-chip
            :color="item.value == filterFileType ? 'blue' : ''"
            :text="item.text"
            v-for="(item, index) in fileTypes"
            :key="index"
            :data-value="item.value"
            @click="fileTypeClick"
          ></f7-chip>
        </f7-block>
        <f7-block-title>{{ $t("common.order") }}</f7-block-title>
        <f7-block strong>
          <f7-chip
            :color="orderBy == 'CreateTime' ? 'blue' : ''"
            :text="$t('navigator.upload_time') + getOrderSymbol('CreateTime')"
            data-orderby="CreateTime"
            @click="orderChange"
          ></f7-chip>
          <f7-chip
            :color="orderBy == 'FileName' ? 'blue' : ''"
            :text="$t('navigator.file_name') + getOrderSymbol('FileName')"
            data-orderby="FileName"
            @click="orderChange"
          ></f7-chip>
          <f7-chip
            :color="orderBy == 'Length' ? 'blue' : ''"
            :text="$t('navigator.file_size') + getOrderSymbol('Length')"
            data-orderby="Length"
            @click="orderChange"
          ></f7-chip>
          <f7-chip
            :color="orderBy == 'Download' ? 'blue' : ''"
            :text="$t('navigator.download_count') + getOrderSymbol('Download')"
            data-orderby="Download"
            @click="orderChange"
          ></f7-chip>
        </f7-block>
      </f7-page-content>
    </f7-sheet>
    <f7-searchbar
      disable-button-text
      :placeholder="$t('common.search')"
      :clear-button="true"
      @change="onSearch"
    ></f7-searchbar>
    <f7-photo-browser
      :photos="photos"
      theme="dark"
      type="standalone"
      :swiper="{ preloadImages: true, lazy: { enabled: false } }"
      ref="standaloneDark"
    ></f7-photo-browser>
    <f7-list media-list>
      <f7-list-item
        v-for="(item, index) in datas"
        :key="item._id"
        :id="index"
        link="#"
        :title="item.FileName"
        :after="''"
        :subtitle="item.CreateTime + ' | ' + item.Percent + '%'"
        :text="
          $funtools.convertFileSize(item.Length) +
          ' | ' +
          item.Download +
          ' | ' +
          item.From
        "
        @click="itemClick(index)"
        swipeout
        @swipeout:delete="delFile(item._id)"
      >
        <template #media>
          <img
            :src="
              $axios.defaults.baseURL +
              $urls.file.downloadPic +
              '/' +
              item._id +
              '/' +
              item.FileName
            "
          />
        </template>
        <f7-swipeout-actions right>
          <f7-swipeout-button
            delete
            color="red"
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
    <f7-fab position="right-bottom" href="/fileadd">
      <f7-icon ios="f7:plus" aurora="f7:plus" md="material:add"></f7-icon>
    </f7-fab>
  </f7-page>
</template>

<script>
import ListBase from "../ListBase";
export default {
  name: "file_manage",
  mixins: [ListBase],
  props: {
    f7router: Object,
  },
  data() {
    return {
      getlist: this.$urls.file.getlist,
      orderBy: "CreateTime",
      orderType: "desc",
      froms: [],
      from: "",
      startTime: "",
      endTime: "",
      filterFileType: "",
      fileDelete: false,
      fileTypes: [
        {
          text: this.$t("navigator.all"),
          value: "",
        },
        {
          text: this.$t("navigator.video"),
          value: "video",
        },
        {
          text: this.$t("navigator.image"),
          value: "image",
        },
        {
          text: this.$t("navigator.audio"),
          value: "audio",
        },
        {
          text: this.$t("navigator.office"),
          value: "office",
        },
        {
          text: this.$t("navigator.pdf"),
          value: "pdf",
        },
        {
          text: this.$t("navigator.text"),
          value: "text",
        },
        {
          text: this.$t("navigator.attachment"),
          value: "attachment",
        },
      ],
    };
  },
  computed: {
    photos: function () {
      var photos = [];
      this.datas.forEach((item, index) => {
        var url =
          this.$axios.defaults.baseURL +
          this.$urls.file.downloadFile +
          "/" +
          item._id +
          "/" +
          item.FileName;
        if (item.FileType == "image") {
          photos.push({
            url: url,
            caption: item.FileName,
          });
        } else if (item.FileType == "video") {
          photos.push({
            html:
              "<video controls width='99%' height='80%'><source src='" +
              url +
              "'></video>",
            caption: item.FileName,
          });
        } else {
          photos.push({
            url:
              this.$axios.defaults.baseURL +
              this.$urls.file.downloadPic +
              "/" +
              item._id +
              "/" +
              item.FileName,
            caption: item.FileName,
          });
        }
      });
      return photos;
    },
  },
  created() {
    this.getFroms();
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
    itemClick(index) {
      var item = this.datas[index];
      if (item.FileType == "pdf") {
        var url = this.$urls.preview + "/" + item._id + "/" + item.FileName;
        window.open(url);
      } else {
        this.$refs.standaloneDark.open(index);
      }
    },
    onClear() {
      this.from = "";
      this.filterFileType = "";
      this.onSearch();
    },
    getOrderSymbol: function (orderBy) {
      if (orderBy == this.orderBy) {
        if (this.orderType == "asc") return "↑";
        if (this.orderType == "desc") return "↓";
      }
      return "";
    },
    fromClick($event) {
      var from = $event.currentTarget.innerText;
      if (this.from == from) {
        this.from = "";
        return;
      }
      this.from = from;
    },
    orderChange($event) {
      var orderby = $event.currentTarget.dataset.orderby;
      if (this.orderBy == orderby) {
        this.orderBy = orderby;
        this.orderType = this.orderType == "desc" ? "asc" : "desc";
      } else {
        this.orderBy = orderby;
        this.orderType = "desc";
      }
    },
    fileTypeClick($event) {
      var value = $event.currentTarget.dataset.value;
      this.filterFileType = value;
    },
    getFroms() {
      this.$axios.get(this.$urls.file.getFromList).then((response) => {
        if (response.code === 0) this.froms = response.result;
      });
    },
    delFile(id) {
      this.$axios.get(this.$urls.file.remove + "/" + id).then((response) => {
        if (response.code != 0) {
          this.showInfo(response.result);
          this.getData();
        }
      });
    },
  },
};
</script>

<style>
.block-title {
  margin-top: 5px;
}
.sheet-modal {
  min-height: 50%;
}
.chip {
  margin-left: 5px;
}
.filemanage .list .item-media {
  height: 80px;
  width: 80px;
  justify-content: center;
}
</style>
