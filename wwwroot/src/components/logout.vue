<template>
  <div class="container">
    <div class="top">
      <a-row type="flex" justify="center" align="middle">
        <a-col :span="24" style="text-align:right;padding-right:20px">
          <a-dropdown>
            <a class="ant-dropdown-link" href="#" v-if="user">
              {{user}}
              <a-icon type="down" />
            </a>
            <a class="ant-dropdown-link" href="#" @click="()=>this.login_visible = true" v-else>登录</a>
            <a-menu slot="overlay" v-if="user">
              <a-menu-item key="0">
                <a target="_self" :href="this.$urls.logout">退出</a>
              </a-menu-item>
            </a-menu>
          </a-dropdown>
        </a-col>
      </a-row>
    </div>
    <div class="main">
      <a-row justify="center" align="middle">
        <a-col :span="8" :offset="8">
          <a-tabs defaultActiveKey="1">
            <a-tab-pane key="1" :closable="false">
              <span slot="tab">
                <a-icon type="home" />导航
              </span>
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
            </a-tab-pane>
            <a-icon
              type="plus"
              slot="tabBarExtraContent"
              :style="{ fontSize: '16px',cursor:'pointer' }"
              @click="()=>this.addurl_visible = true"
            />
          </a-tabs>
        </a-col>
      </a-row>
    </div>
    <a-modal
      title="登录"
      :visible="login_visible"
      :maskClosable="false"
      @ok="login"
      :confirmLoading="false"
      @cancel="()=>this.login_visible=false"
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
    <a-modal
      title="添加网址"
      :visible="addurl_visible"
      :maskClosable="false"
      @ok="addUrl"
      :confirmLoading="false"
      @cancel="()=>this.addurl_visible=false"
    >
      <a-form :form="addUrl_form" @submit.prevent="handleSubmit">
        <a-form-item>
          <a-input
            v-decorator="['title',{ rules: [{ required: true, message: 'Please input your title!' }] }]"
            placeholder="名称"
          ></a-input>
        </a-form-item>
        <a-form-item>
          <a-input
            v-decorator="['url',{ rules: [{ required: true, message: 'Please input your url!' }]}]"
            placeholder="网址"
          ></a-input>
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
      user: "",
      login_visible: false,
      addurl_visible: false,
      form: this.$form.createForm(this),
      addUrl_form: this.$form.createForm(this)
    };
  },
  created() {
    this.$http.get(this.$urls.geturlmeta).then(response => {
      if (response.body.code == 0) this.urls = response.body.result;
    });
    this.$http.get(this.$urls.getuser).then(response => {
      if (response.body.code == 0 && response.body.result) {
        this.user = response.body.result;
      }
      // this.user = "Yang X Wang";
    });
  },
  methods: {
    login() {
      this.form.validateFields((err, values) => {
        if (!err) {
          this.$http.post(this.$urls.login, values).then(response => {
            if (response.body.code == 0) {
              this.login_visible = false;
            } else {
              this.$message.warning("用户名或密码不正确!");
            }
          });
        }
      });
    },
    addUrl() {}
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