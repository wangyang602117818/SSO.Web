<template>
  <div class="container">
    <div class="top">
      <a-row type="flex" justify="center" align="middle">
        <a-col :span="24" style="text-align:right">
          <a-dropdown>
            <a-button size="small" v-if="user">
              {{user.UserName}}
              <a-icon type="down" />
            </a-button>
            <a-button size="small"  @click="login" v-else>登录</a-button>
            <a-menu slot="overlay" v-if="user">
              <a-menu-item key="0">
                <a target="_self" :href="this.$urls.logout">退出</a>
              </a-menu-item>
            </a-menu>
          </a-dropdown>&nbsp;
          <element v-if="user">
            <a-button size="small" v-for="role in user.Role" :key="role">{{role}}</a-button>
          </element>
        </a-col>
      </a-row>
    </div>
    <div class="main">
      <a-tabs defaultActiveKey="1">
        <a-tab-pane key="1" :closable="false">
          <span slot="tab">
            <a-icon type="home" />导航
          </span>
          <a-row>
            <a
              class="site"
              target="_blank"
              :href="url.BaseUrl"
              v-for="url in urls"
              :key="url._id"
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
              <a-popconfirm
                title="Are you sure delete?"
                @confirm="delsite(url._id)"
                okText="Yes"
                cancelText="No"
              >
                <a-icon slot="icon" type="question-circle-o" style="color: red" />
                <div @click.prevent>
                  <a-icon type="close" class="delsite" style="color:#444" v-if="user" />
                </div>
              </a-popconfirm>
              <div
                @click.prevent="updatesite(url._id)"
                :id="url._id"
                class="updatesite"
                v-if="user"
              >
                <a-icon type="edit" style="color:#444" />
              </div>
            </a>
          </a-row>
        </a-tab-pane>
        <a-icon
          v-if="user"
          type="plus"
          slot="tabBarExtraContent"
          :style="{ fontSize: '16px',cursor:'pointer' }"
          @click="()=>{this.addurl_visible = !this.addurl_visible;this.is_update_url=false}"
        />
      </a-tabs>
      <div class="addurl" v-if="addurl_visible">
        <a-form :form="addUrl_form" layout="inline" @submit.prevent="addUrl">
          <a-form-item>
            <a-input
              style="width: 120px"
              v-decorator="['title',{ rules: [{ required: true, message: 'Please input your title!' }] }]"
              placeholder="名称"
            ></a-input>
          </a-form-item>
          <a-form-item>
            <a-input
              style="width: 180px"
              v-decorator="['baseUrl',{ rules: [{ required: true, message: 'Please input your url!' }]}]"
              placeholder="网址"
            ></a-input>
          </a-form-item>
          <a-form-item>
            <a-button type="default" @click="addurl_visible=false">取消</a-button>&nbsp;
            <a-button type="primary" html-type="submit">{{is_update_url?'修改':'添加'}}</a-button>
          </a-form-item>
        </a-form>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "app",
  data() {
    return {
      urls: [],
      user: null,
      login_visible: false,
      addurl_visible: false,
      form: this.$form.createForm(this),
      addUrl_form: this.$form.createForm(this),
      is_update_url: false,
      id: 0
    };
  },
  created() {
    this.getUrlMeta();
    this.getUser();
  },
  methods: {
    login() {
      window.location.href = this.$http.options.root + this.$urls.login;
    },
    getUrlMeta() {
      this.$http.get(this.$urls.geturlmeta).then(response => {
        if (response.body.code == 0) this.urls = response.body.result;
      });
    },
    getUser() {
      this.$http.get(this.$urls.decodetoken).then(response => {
        if (response.body.code == 0 && response.body.result) {
          this.user = response.body.result;
        }
      });
    },
    delsite(id) {
      this.$http
        .post(this.$urls.deletenavigation, { ids: [id] })
        .then(response => {
          if (response.body.code == 0) {
            this.getUrlMeta();
          } else {
            this.$message.warning("删除失败!");
          }
        });
    },
    updatesite(id) {
      this.addurl_visible = true;
      this.is_update_url = true;
      this.id = id;
      this.$http.get(this.$urls.getnavigationbyid + "/" + id).then(response => {
        if (response.body.code == 0) {
          this.addUrl_form.setFieldsValue({
            title: response.body.result.Title,
            baseUrl: response.body.result.BaseUrl
          });
        }
      });
    },
    addUrl() {
      this.addUrl_form.validateFields((err, values) => {
        if (!err) {
          if (this.is_update_url) {
            values.id = this.id;
            this.$http
              .post(this.$urls.updatenavigation, values)
              .then(response => {
                if (response.body.code == 0) {
                  this.addurl_visible = false;
                  this.getUrlMeta();
                } else {
                  this.$message.warning("修改失败!");
                }
              });
          } else {
            this.$http.post(this.$urls.addnavigation, values).then(response => {
              if (response.body.code == 0) {
                this.addurl_visible = false;
                this.getUrlMeta();
              } else {
                this.$message.warning("添加失败!");
              }
            });
          }
        }
      });
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
  margin-top: 100px;
  display: flex;
  align-items: center;
  justify-content: center;
}
.site {
  position: relative;
  display: inline-flex;
  cursor: pointer;
  height: 100px;
  width: 100px;
  margin: 6px;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  color: #000;
}
.site:hover {
  background-color: #e5e7e8;
}
.logo {
  flex: 2;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 50px;
}
.logo img {
  height: 40px;
  width: 40px;
}

.title {
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
  width: 100%;
}
.ant-tabs {
  width: 563px;
}
.ant-tabs-tabpane {
  background-color: #f6f6f6;
  border: 1px solid #e8e8e8;
}
.addurl {
  border: 1px solid #e8e8e8;
  height: 120px;
  position: absolute;
  width: 563px;
  top: 174px;
  background-color: #fff;
  display: flex;
  padding-top: 30px;
  justify-content: center;
}
.delsite {
  display: none;
  position: absolute;
  background-color: #e0e0e0;
  width: 20px;
  height: 20px;
  left: 0;
  top: 0;
}
.delsite:hover {
  background-color: #ccc;
}
.updatesite {
  display: none;
  position: absolute;
  background-color: #e0e0e0;
  width: 20px;
  height: 20px;
  right: 0;
  top: 0;
}
.updatesite:hover {
  background-color: #ccc;
}
.site:hover .delsite {
  display: inline-flex;
  align-items: center;
  justify-content: center;
}
.site:hover .updatesite {
  display: inline-flex;
  align-items: center;
  justify-content: center;
}
</style>