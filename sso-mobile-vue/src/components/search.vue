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
        :subtitle="item.create_time + ' | ' + $funtools.removeHTML(item.description)"
        :text="getTypeName(item.id)"
        v-for="item in datas"
        :key="item.id"
        @click="navSearch(item.id)"
      >
        <template #media v-if="getType(item.id)[1] == 'fileswrap'">
          <img
            :src="
              $axios.defaults.baseURL +
              $urls.file.downloadPic +
              '/' +
              getType(item.id)[2] +
              '/' +
              $funtools.removeHTML(item.title)
            "
          />
        </template>
      </f7-list-item>
      <f7-block-footer>
        <p style="text-align: center">end</p>
      </f7-block-footer>
    </f7-list>
    <div class="bg" v-if="suggests.length > 0" @click="suggests = []"></div>
  </f7-page>
</template>
<script>
var inputFuncTimeout = null;
import ListBase from "./ListBase";
import { nextTick } from "vue";
export default {
  mixins: [ListBase],
  props: {
    word: String,
    f7router: Object,
  },
  data() {
    return {
      getlist: this.$urls.search.search,
      suggests: [],
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
    getType(id) {
      var index1 = id.indexOf("_");
      var index2 = id.indexOf("_", index1 + 1);
      return [
        id.substring(0, index1),
        id.substring(index1 + 1, index2),
        id.substring(index2 + 1, id.length),
      ];
    },
    getTypeName(id) {
      var table = this.getType(id)[1];
      switch (table) {
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
    navSearch(id) {
      var list = this.getType(id);
      if (list[1] == "user") {
        this.getUser(list[2]);
      } else if (list[1] == "company") {
        this.f7router.navigate("/companyupdate/" + list[2]);
      } else if (list[1] == "role") {
        this.f7router.navigate("/roleupdate/" + list[2]);
      } else if (list[1] == "department") {
        this.getDepartment(list[2]);
      } else if (list[1] == "fileswrap") {
        this.getFile(list[2]);
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
      this.getData(true);
    },
    selectItem(e) {
      var text = e.target.dataset.text;
      this.suggests = [];
      this.filter = text;
      this.getData(true);
    },
    suggestInput(e) {
      var value = e.target.value;
      if (!value) {
        this.suggests = [];
        return;
      }
      clearTimeout(inputFuncTimeout);
      var that = this;
      inputFuncTimeout = setTimeout(function () {
        that.loadSuggest(value);
      }, 500);
    },
    loadSuggest(value) {
      this.$axios
        .get(this.$urls.search.suggest + "?word=" + value)
        .then((response) => {
          if (response.code == 0) {
            this.suggests = response.result;
          }
        });
    },
    suggestFocus(e) {
      var value = e.target.value;
      this.loadSuggest(value);
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
