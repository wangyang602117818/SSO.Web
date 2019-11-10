<template>
  <div class="container">
    <div class="top">
      <a-row type="flex" justify="center" align="middle">
        <a-col :span="24" style="text-align:right;padding-right:20px">
          <a-dropdown>
            <a class="ant-dropdown-link" href="#" @click="showLoginModal">登录</a>
          </a-dropdown>
        </a-col>
      </a-row>
    </div>
    <div class="main">
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
            <a
              class="site"
              target="_blank"
              :href="url.BaseUrl"
              v-for="url in urls"
              :key="url.Title"
              :title="url.BaseUrl"
            >
              <div class="logo">
                <img alt="icon" v-bind:src="url.IconUrl" v-if="url.IconUrl" />
                <span v-else>
                  <a-avatar
                    size="large"
                    style="backgroundColor:#3498DB"
                  >{{url.Title.getFileName(1).trimEnd('.')}}</a-avatar>
                </span>
              </div>
              <div class="title" :title="url.Title">{{url.Title.getFileName(3)}}</div>
            </a>
          </a-row>
        </a-col>
      </a-row>
    </div>
    <a-modal
      title="登录"
      :visible="visible"
      :maskClosable='false'
      @ok="login"
      :confirmLoading="confirmLoading"
      @cancel="handleCancel"
    >
      <a-form :form="form" @submit.prevent="handleSubmit">
        <a-form-item>
          <a-input
            v-decorator="['userId',{ rules: [{ required: true, message: 'Please input your userId!' }] }]"
            placeholder="UserId"
            size="large"
          >
            <a-icon slot="prefix" type="user" style="color: rgba(0,0,0,.25)" />
          </a-input>
        </a-form-item>
        <a-form-item>
          <a-input
            v-decorator="['password',{ rules: [{ required: true, message: 'Please input your Password!' }]}]"
            type="password"
            placeholder="Password"
            size="large"
          >
            <a-icon slot="prefix" type="lock" style="color: rgba(0,0,0,.25)" />
          </a-input>
        </a-form-item>
      </a-form>
    </a-modal>
  </div>
</template>

<script>
export default {
  name: "app",
  data() {
    return {
      urls: [],
      visible: false,
      confirmLoading: false,
      form: this.$form.createForm(this)
    };
  },
  created() {
    this.$http.get(this.$urls.geturlmeta).then(response => {
      if (response.body.code == 0) this.urls = response.body.result;
    });
  },
  methods: {
    login() {
      this.form.validateFields((err, values) => {
        if (!err) {
          console.log(values);
        }
      });
    },
    handleCancel() {
      this.visible = false;
    },
    showLoginModal() {
      this.visible = true;
    }
  }
};
</script>
<style scoped>
.ant-row-flex {
  text-align: center;
  width: 100%;
  height: 100%;
}
.top {
  background-color: #e6f7ff;
  height: 30px;
}
.main {
  margin-top: 150px;
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