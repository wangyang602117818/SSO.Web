<template>
  <div class="container">
    <a-row type="flex" justify="center" align="middle">
      <a-col :span="8">
        <h2>页面导航</h2>
      </a-col>
    </a-row>
    <a-row type="flex" justify="center" align="middle">
      <a-col :span="8">
        <a-divider></a-divider>
      </a-col>
    </a-row>
    <a-row type="flex" justify="center" align="middle">
      <a-col :span="8">
        <a-row type="flex" justify="space-between">
          <a class="site" target="_blank" :href="url.Url" v-for="url in data.urls" :key="url.Title">
            <div class="logo">
              <img alt="icon" v-bind:src="url.IconUrl" v-if="url.IconUrl" />
              <span v-else>
                <a-avatar size="large" style="backgroundColor:#3498DB">{{url.Title.getFileName(1).trimEnd('.')}}</a-avatar>
              </span>
            </div>
            <div class="title" :title="url.Title">{{url.Title.getFileName(3)}}</div>
          </a>
        </a-row>
      </a-col>
    </a-row>
  </div>
</template>

<script>
export default {
  name: "app",
  data() {
    return {
      data: {
        urls: []
      }
    };
  },
  created() {
    this.$http.get(this.$urls.geturlmeta).then(response => {
      if (response.body.code == 0) this.data.urls = response.body.result;
    });
  },
  methods: {}
};
</script>
<style scoped>
.ant-row-flex {
  text-align: center;
  width: 100%;
}
.container {
  padding-top: 150px;
}
.site {
  cursor: pointer;
  height: 100px;
  width: 100px;
  margin-top: 10px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  color: #000;
}
.logo {
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 50px;
}
.logo img {
  height: 40px;
  width: 40px;
}
.logo:hover {
  background-color: #e5e7e8;
}
.title {
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
}
</style>