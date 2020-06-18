<template>
  <div class="me">
    <div class="me_text">我的</div>
    <div class="me_title" @click="$f7router.navigate('/personal/'+user.unique_name)">
      <div class="ico">
        <f7-icon f7="person" color="blue" size="35"></f7-icon>
      </div>
      <div class="desc">
        <div class="name" v-if="user">{{user.StaffName}}</div>
        <div class="detail" v-if="user">{{user.role.join(',')}}</div>
      </div>
      <div class="arrow">
        <f7-icon f7="chevron_right" color="gray" size="25"></f7-icon>
      </div>
    </div>
    <div class="me_settings">
      <div class="setting_line">
        <div class="setting_line_name">语言设置</div>
        <div class="setting_line_right">
          <div class="setting_line_data">中</div>
          <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">头像设置</div>
        <div class="setting_line_right">
          <div class="setting_line_data"></div>
          <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">修改密码</div>
        <div class="setting_line_right">
          <div class="setting_line_data"></div>
          <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">上次登录时间</div>
        <div class="setting_line_right">
          <div class="setting_line_data">{{user.LastLoginTime}}</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">创建时间</div>
        <div class="setting_line_right">
          <div class="setting_line_data">{{user.CreateTime}}</div>
        </div>
      </div>
    </div>
    <div class="me_bottom">
      <f7-button large outline color="gray" @click="logOut">退出</f7-button>
    </div>
  </div>
</template>

<script>
export default {
  name: "me",
  data() {
    return {
      user: window.token_jwt_data
    };
  },
  created() {},
  methods: {
    logOut() {
      window.location.href =
        this.$axios.defaults.baseURL +
        this.$urls.logout +
        "?returnUrl=" +
        window.location.href;
    }
  }
};
</script>

<style scoped>
.me {
  margin: 0 auto;
  width: 90%;
  align-items: center;
  display: flex;
  flex-direction: column;
  /* border: 1px solid green; */
}
.me_text {
  font-size: 16px;
  height: 35px;
  line-height: 35px;
  padding-left: 5px;
  width: 100%;
}
.me_title {
  height: 50px;
  margin-top: 5px;
  overflow: hidden;
  display: flex;
  flex-direction: row;
  border-bottom: 1px solid #e3e4e4;
  width: 100%;
}
.me_title:active {
  background-color: #ddd;
}
.me_title .ico {
  width: 45px;
  display: flex;
  align-items: center;
  justify-content: flex-start;
}
.me_title .desc {
  flex: 1;
  display: flex;
  flex-direction: column;
}

.me_title .arrow {
  display: flex;
  justify-content: flex-start;
  align-items: center;
  width: 30px;
}
.desc .name {
  height: 25px;
  line-height: 25px;
  font-weight: bold;
}
.desc .detail {
  flex: 1;
  display: flex;
}
.me_settings {
  flex: 1;
  width: 100%;
}
.me_bottom {
  width: 100%;
  height: 100px;
  display: flex;
  justify-content: center;
  align-items: center;
  border-bottom: 0;
}
.me_bottom .button {
  width: 100%;
}
</style>
