<template>
  <f7-page
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
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
      <div class="suggest_warp" v-if="suggests.length > 0">
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
    <f7-list class="searchbar-not-found">
      <f7-list-item title="Nothing found"></f7-list-item>
    </f7-list>
    <f7-list media-list>
      <f7-list-item
        link="#"
        :title="item.title"
        :after="item.create_time"
        :text="item.description"
        v-for="item in datas"
        :key="item.id"
      ></f7-list-item>
      <f7-list-item
        title="Yellow Submarine"
        subtitle="Beatles"
        link="#"
        after="17:14"
      >
        <template #media>
          <img
            src="https://cdn.framework7.io/placeholder/fashion-88x88-1.jpg"
            width="44"
          />
        </template>
      </f7-list-item>
    </f7-list>
    <div class="bg" v-if="suggests.length > 0" @click="suggests = []"></div>
  </f7-page>
</template>
<script>
var inputFuncTimeout = null;
import ListBase from "./ListBase";
export default {
  mixins: [ListBase],
  props: {
    word: String,
  },
  data() {
    return {
      getlist: this.$urls.search.search,
      suggests: [],
      filter: this.word,
    };
  },
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
<style scoped>
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
.suggest_warp {
  top: 53px;
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
</style>
