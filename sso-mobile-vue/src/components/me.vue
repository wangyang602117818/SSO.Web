<template>
  <div class="me">
    <div class="me_text">{{ $t('common.me') }}</div>
    <div class="me_title" @click="$f7router.navigate('/personal')">
      <div class="ico">
        <f7-icon f7="person" color="blue" size="35"></f7-icon>
      </div>
      <div class="desc">
        <div class="name" v-if="user">{{user.StaffName}}</div>
        <div class="detail" v-if="user">{{user.unique_name}}</div>
      </div>
      <div class="arrow">
        <f7-icon f7="chevron_right" color="gray" size="25"></f7-icon>
      </div>
    </div>
    <div class="me_settings">
      <div class="setting_line item-link smart-select smart-select-init" data-open-in="popover">
        <div class="setting_line_name">{{ $t('common.language') }}</div>
        <div class="setting_line_right">
          <div class="setting_line_data">{{user.Lang=='zh-cn'?'中':'EN'}}</div>
          <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
        </div>
        <select name="language_select" @change="changeLang">
          <option value="en-us" v-bind:selected="user.Lang=='en-us'">EN</option>
          <option value="zh-cn" v-bind:selected="user.Lang=='zh-cn'">中</option>
        </select>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">{{ $t('me.pic_setting') }}</div>
        <div class="setting_line_right">
          <div class="setting_line_data"></div>
          <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">{{ $t('me.change_password') }}</div>
        <div class="setting_line_right">
          <div class="setting_line_data"></div>
          <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">{{ $t('me.last_login_time') }}</div>
        <div class="setting_line_right">
          <div class="setting_line_data">{{user.LastLoginTime}}</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">{{$t('common.create_time')}}</div>
        <div class="setting_line_right">
          <div class="setting_line_data">{{user.CreateTime}}</div>
        </div>
      </div>
    </div>
    <div class="me_bottom">
      <f7-button large outline color="gray" @click="logOut">{{$t('common.logout')}}</f7-button>
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
  created() {
    // window.console.log(window.token_jwt_data);
  },
  methods: {
    logOut() {
      window.location.href =
        this.$axios.defaults.baseURL +
        this.$urls.logout +
        "?returnUrl=" +
        window.location.href;
    },
    changeLang($event) {
      var lang = $event.target.value;
      this.$axios
        .get(this.$urls.settings.setLang + "?lang=" + lang)
        .then(response => {
          if (response.code == 0) {
            this.$funtools.setCookie(this.$cookieName, response.result);
            this.$funtools.parseTokenSetMessage(response.result);
            this.user = window.token_jwt_data;
            this.$i18n.locale = lang;
          }
        });
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
