<template>
  <div class="navigator">
    <div class="nav_top">
      <input type="text" placeholder="search" />
    </div>

    <div class="nav_content">
      <div class="sub_title">导航列表</div>
      <f7-row v-for="(item,index) in datas" :key="index" no-gap>
        <f7-col v-for="(it,i) in item" :key="i">
          <div v-if="it" class="nav_wrap" :data-url="it.BaseUrl" @click="itemClick">
            <img class="nav_logo" :src="it.LogoUrl" alt />
            <div class="nav_title">
              <span>{{it.Title}}</span>
            </div>
          </div>
        </f7-col>
      </f7-row>
    </div>
  </div>
</template>

<script>
export default {
  name: "navigator",
  data() {
    return {
      datas: []
    };
  },
  created() {
    this.$axios.get(this.$urls.navigation.geturlmeta).then(response => {
      if (response.code === 0) {
        this.datas = this.$funtools.reMapArray(response.result, 3);
      }
    });
  },
  methods: {
    itemClick($event) {
      window.open($event.currentTarget.dataset.url);
    }
  }
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
}

.nav_top input {
  width: 100%;
  padding: 10px 20px;
  height: 44px;
  font-size: 16px;
  border: 1px solid #d4d4d4;
  border-radius: 4px;
  background-color: #fff;
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
}
.nav_wrap:active{
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
</style>
