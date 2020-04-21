<template>
  <div class="container">
    <a-row type="flex" justify="center" align="middle">
      <a-col :span="span">
        <a-card title="登录" :bordered="true">
          <a-form :form="form" @submit.prevent="handleSubmit">
            <a-form-item>
              <a-input
                v-decorator="['userId',{ rules: [{ required: true, message: '请输入用户编号' }] }]"
                placeholder="用户编号"
                size="large"
              >
                <a-icon slot="prefix" type="user" style="color: rgba(0,0,0,.25)" />
              </a-input>
            </a-form-item>
            <a-form-item>
              <a-input
                v-decorator="['password',{ rules: [{ required: true, message: '请输入密码' }]}]"
                type="password"
                placeholder="用户密码"
                size="large"
              >
                <a-icon slot="prefix" type="lock" style="color: rgba(0,0,0,.25)" />
              </a-input>
            </a-form-item>
            <a-form-item>
              <a-button type="primary" html-type="submit" :loading="loading">登 录</a-button>
            </a-form-item>
          </a-form>
        </a-card>
      </a-col>
    </a-row>
  </div>
</template>
<script>
export default {
  name: "app",
  data() {
    return {
      form: this.$form.createForm(this),
      loading: false,
      mobile: false,
      clientWidth: 0,
      span: 6,
    };
  },
  mounted() {
    this.mobile = !this.isPc();
    this.clientWidth = document.documentElement.clientWidth;
    this.getSpan(document.documentElement.clientWidth);
    var that = this;
    window.onresize = function() {
      that.clientWidth = document.documentElement.clientWidth;
      that.getSpan(document.documentElement.clientWidth);
    };
  },
  methods: {
    handleSubmit() {
      this.loading = true;
      this.form.validateFields((err, values) => {
        if (!err) {
          values.returnUrl = this.$funtools.getReturnUrl("returnUrl");
          this.$http.post(this.$urls.login, values).then(response => {
            if (response.body.code == 0) {
              var returnUrl = response.body.result;
              window.location.href = decodeURIComponent(returnUrl);
            } else {
              this.$message.warning("用户名或密码不正确!");
              this.loading = false;
            }
          });
        }else{
          this.loading = false;
        }
      });
    },
    getSpan(width){
      var span =  parseInt(width / 50);
      if(span<20){
        span=22
      }else{
        span=6;
      }
      this.span=span;
    },
    isPc() {
      var userAgentInfo = navigator.userAgent;
      var Agents = new Array(
        "Android",
        "iPhone",
        "SymbianOS",
        "Windows Phone",
        "iPad",
        "iPod"
      );
      var flag = true;
      for (var v = 0; v < Agents.length; v++) {
        if (userAgentInfo.indexOf(Agents[v]) > 0) {
          flag = false;
          break;
        }
      }
      return flag;
    }
  }
};
</script>
<style scoped>
.ant-row-flex {
  height: 100%;
}
.ant-btn-primary {
  width: 100%;
  margin-left: 0px !important;
}
.container {
  position: absolute;
  left: 0;
  bottom: 0;
  right: 0;
  top: 0;
  background-color: rgba(226, 226, 226, 1);
}
</style>