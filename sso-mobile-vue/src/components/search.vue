<template>
  <f7-page
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
    class="search"
  >
    <f7-navbar :title="$t('common.search')" :back-link="$t('common.back')">
    </f7-navbar>
    <div class="search_wrap">
      <input
        type="text"
        :placeholder="$t('common.search')"
        class="search"
        v-model="filter"
        autocomplete="off"
        id="search"
        @focus="suggestFocus"
        @input="suggestInput"
        @keyup.enter="search"
      />
      <div class="suggest_warp" v-if="suggests.length > 0" style="top: 53px">
        <f7-link
          class="suggest_item"
          v-for="item in suggests"
          :data-text="item.text"
          :key="item.id"
          @click="selectItem"
          >{{ item.text }}</f7-link
        >
      </div>
    </div>
    <f7-photo-browser
      :photos="photos"
      theme="dark"
      type="page"
      :swiper="{ preloadImages: true, lazy: { enabled: false } }"
      ref="standaloneSearch"
    ></f7-photo-browser>
    <f7-list class="searchbar-not-found">
      <f7-list-item title="Nothing found"></f7-list-item>
    </f7-list>
    <f7-list media-list>
      <f7-list-item
        link="#"
        :title="$funtools.removeHTML(item.title)"
        :subtitle="
          item.create_time + ' | ' + $funtools.removeHTML(item.description)
        "
        :text="getTypeName(item.table)"
        v-for="item in datas"
        :key="item.id"
        @click="navSearch(item.table.toLowerCase(), item.key)"
      >
        <template #media v-if="item.table.toLowerCase() == 'fileswrap'">
          <img
            :src="
              $axios.defaults.baseURL +
              $urls.file.downloadPic +
              '/' +
              item.key +
              '/' +
              $funtools.removeHTML(item.title)
            "
          />
        </template>
      </f7-list-item>
      <f7-block-footer v-if="datas.length === 0 && isEnd">
        <p style="text-align: center">---{{ $t("common.no_data") }}---</p>
      </f7-block-footer>
      <f7-block-footer v-if="datas.length > 0 && isEnd">
        <p style="text-align: center">---{{ $t("common.end") }}---</p>
      </f7-block-footer>
    </f7-list>
    <div class="bg" v-if="suggests.length > 0" @click="suggests = []"></div>
  </f7-page>
</template>
<script>
import ListBase from "./ListBase";
import Suggest from "./Suggest";
export default {
  mixins: [ListBase, Suggest],
  props: {
    word: String,
    f7router: Object,
  },
  data() {
    return {
      getlist: this.$urls.search.search,
      filter: this.word,
      photos: [],
    };
  },
  computed: {},
  methods: {
    getQuerystring() {
      var url =
        "?pageIndex=" +
        this.pageIndex +
        "&pageSize=" +
        this.pageSize +
        "&word=" +
        this.filter;
      return url;
    },
    getTypeName(table) {
      switch (table.toLowerCase()) {
        case "user":
          return this.$t("common.user");
        case "company":
          return this.$t("common.company");
        case "department":
          return this.$t("common.department");
        case "role":
          return this.$t("common.role");
        case "fileswrap":
          return this.$t("common.file");
      }
    },
    navSearch(tb, id) {
      if (tb == "user") {
        this.getUser(id);
      } else if (tb == "company") {
        this.f7router.navigate("/companyupdate/" + id);
      } else if (tb == "role") {
        this.f7router.navigate("/roleupdate/" + id);
      } else if (tb == "department") {
        this.getDepartment(id);
      } else if (tb == "fileswrap") {
        this.getFile(id);
      }
    },
    getUser(id) {
      this.$axios.get(this.$urls.user.getbyid + "/" + id).then((response) => {
        if (response.code == 0) {
          this.f7router.navigate("/userupdate/" + response.result.UserId);
        }
      });
    },
    getDepartment(id) {
      this.$axios
        .get(this.$urls.department.getbyid + "/" + id)
        .then((response) => {
          if (response.code == 0) {
            this.f7router.navigate(
              "/departmentupdate/" +
                response.result.CompanyCode +
                "/" +
                response.result.Code
            );
          }
        });
    },
    getFile(id) {
      var photos = [];
      this.$axios
        .get(this.$urls.file.getfileInfos + "?ids[0]=" + id)
        .then((response) => {
          if (response.code == 0) {
            var item = response.result[0];
            var url =
              this.$axios.defaults.baseURL +
              this.$urls.file.downloadFile +
              "/" +
              item.Id +
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
            }
            this.photos = photos;
            this.$nextTick(function () {
              this.$refs.standaloneSearch.open(0);
            });
          }
        });
    },
    search(e) {
      var value = e.target.value;
      this.suggests = [];
      this.filter = value;
      this.pageIndex = 1;
      this.getData(true);
    },
    selectItem(e) {
      var text = e.target.dataset.text;
      this.suggests = [];
      this.filter = text;
      this.getData(true);
    },
  },
};
</script>
<style>
.search_wrap {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 95%;
  height: 60px;
  margin: 0 auto;
  position: relative;
}
#search {
  height: 44px;
  font-size: 16px;
  border: 1px solid #d4d4d4;
  background-color: #fff;
  flex: 1;
  position: absolute;
  z-index: 99;
  width: 100%;
  border-radius: 4px;
  padding: 8px 15px;
}
.page-content .list {
  width: 95%;
  margin: 0 auto;
}
.bg {
  position: absolute;
  z-index: 10;
  top: 0;
  bottom: 0;
  left: 0;
  right: 0;
  background: rgba(0, 0, 0, 0.2);
}
.search .list .item-media {
  height: 80px;
  width: 80px;
  justify-content: center;
}
</style>
