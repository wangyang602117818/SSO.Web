<template>
  <div class="container">
    <a-row type="flex" justify="center" align="middle">
      <a-col :span="6">
        <a-card title="Login" :bordered="true">
          <a-form :form="form" class="login-form">
            <a-form-item>
              <a-input
                v-decorator="[
          'userName',
          { rules: [{ required: true, message: 'Please input your username!' }] }
        ]"
                placeholder="Username"
                size="large"
              >
                <a-icon slot="prefix" type="user" style="color: rgba(0,0,0,.25)" />
              </a-input>
            </a-form-item>
            <a-form-item>
              <a-input
                v-decorator="[
          'password',
          { rules: [{ required: true, message: 'Please input your Password!' }] }
        ]"
                type="password"
                placeholder="Password"
                size="large"
              >
                <a-icon slot="prefix" type="lock" style="color: rgba(0,0,0,.25)" />
              </a-input>
            </a-form-item>
            <a-form-item>
              <a-button type="primary" html-type="submit" class="login-form-button">Log in</a-button>
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
      form: this.$form.createForm(this)
    };
  }
};
</script>
<style scoped>
.ant-row-flex {
  height: 100%;
}
.ant-btn-primary {
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