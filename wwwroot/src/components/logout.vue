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
          <a class="site">
            <div class="logo">
              <img alt="example" src="http://localhost:8080/favicon.ico" />
            </div>
            <div class="title">sso.com</div>
          </a>
          <a class="site">
            <div class="logo">
              <a-avatar size="large" style="backgroundColor:#3498DB">SSO</a-avatar>
            </div>
            <div class="title">baidu.com</div>
          </a>
          <a class="site">
            <div class="logo">
              <a-avatar size="large" style="backgroundColor:#3498DB">WY</a-avatar>
            </div>
            <div class="title">sso.com</div>
          </a>
          <a class="site">
            <div class="logo">
              <a-avatar size="large" style="backgroundColor:#3498DB">Ok</a-avatar>
            </div>
            <div class="title">sso.com</div>
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
        urls:[]
    };
  },
  created() {
    
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