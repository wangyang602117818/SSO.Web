<template>
  <div class="navigator">
    <div class="nav_top">
      <input type="text" :placeholder="$t('common.search')" />
      <div class="suggest_warp">
        <f7-link class="suggest_item" href="/search">百度文库</f7-link>
        <f7-link class="suggest_item">头条知乎</f7-link>
        <f7-link class="suggest_item">头条知乎</f7-link>
      </div>
    </div>
    <div class="nav_content">
      <div class="sub_title">{{ $t("navigator.navigators") }}</div>
      <f7-row no-gap>
        <f7-col>
          <f7-link class="nav_wrap" href="/filemanage/">
            <img class="nav_logo" src="../assets/file.png" alt />
            <div class="nav_title">
              <span>{{ $t("navigator.file_manage") }}</span>
            </div>
          </f7-link>
        </f7-col>
        <f7-col> </f7-col>
        <f7-col> </f7-col>
      </f7-row>
    </div>
    <!-- <div class="bg"></div> -->
  </div>
</template>

<script>
export default {
  name: "navigator",
  data() {
    return {};
  },
  created() {},
  methods: {},
};
</script>

<style scoped>
.navigator {
  height: 100%;
  display: flex;
  flex-direction: column;
}

.nav_top {
  height: 100px;
  width: 90%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto;
  position: relative;
}
.suggest_warp {
  position: absolute;
  width: 100%;
  box-sizing: border-box;
  z-index: 99;
  max-height: 300px;
  min-height: 170px;
  overflow: auto;
  top: 71px;
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
.nav_top input {
  flex: 1;
  padding: 10px 15px;
  height: 44px;
  font-size: 16px;
  border: 1px solid #d4d4d4;
  border-radius: 4px 4px 0px 0px;
  background-color: #fff;
  position: absolute;
  z-index: 99;
  width: 100%;
}

.nav_content {
  width: 90%;
  margin: 0 auto;
  background-color: #fff;
  border-radius: 4px;
  border: 1px solid #f0f0f0;
  border-bottom: 0;
}

.sub_title {
  height: 30px;
  line-height: 30px;
  background-color: #fff;
  padding-left: 6px;
  border-bottom: 1px solid #f0f0f0;
}
.nav_wrap {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 15px;
  border-right: 1px solid #f0f0f0;
  border-bottom: 1px solid #f0f0f0;
  color: #000;
}
.nav_wrap:active {
  background-color: #ddd;
}
.nav_logo {
  height: 45px;
  width: 45px;
  display: inline-block;
}

.nav_title {
  height: 30px;
  line-height: 30px;
  text-align: center;
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
