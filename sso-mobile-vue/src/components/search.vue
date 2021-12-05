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
        id="search"
      />
      <div class="suggest_warp">
        <f7-link class="suggest_item" href="/search">百度文库</f7-link>
        <f7-link class="suggest_item" >头条知乎</f7-link>
        <f7-link class="suggest_item" >头条知乎</f7-link>
      </div>
    </div>
    <f7-list class="searchbar-not-found">
      <f7-list-item title="Nothing found"></f7-list-item>
    </f7-list>
    <f7-list media-list>
      <f7-list-item
        link="#"
        title="Facebook"
        after="17:14"
        text="Lorem ipsum dolor sit amet, "
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
    <div class="bg"></div>
  </f7-page>
</template>
<script>
import ListBase from "./ListBase";
export default {
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.search.search,
    };
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
  },
};
</script>
<style>
.search_wrap{
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
  position: absolute;
  width: 100%;
  box-sizing: border-box;
  z-index: 99;
  max-height: 300px;
  min-height: 170px;
  overflow: auto;
  top: 53px;
  background-color: #fff;
  border: 1px solid #d4d4d4;
  border-radius: 0px 0px 4px 4px;
}
.suggest_warp .suggest_item {
  padding: 10px 15px;
  display: block;
  color: #444;
  font-weight: bold;
}
.suggest_item:hover,
.suggest_item:active {
  background-color: #f0f0f0;
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
