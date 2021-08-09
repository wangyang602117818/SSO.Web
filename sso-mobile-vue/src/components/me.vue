<template>
  <div class="me" v-if="$store.state.currentUser">
    <div class="me_text">{{ $t('common.me') }}</div>
    <div class="me_title" @click="$f7router.navigate('/personal')">
      <div class="ico">
        <img
          :src="$axios.defaults.baseURL+$urls.file.downloadPic+'/'+$store.state.currentUser.FileId+'/'+$store.state.currentUser.FileName"
          v-if="$store.state.currentUser.FileId"
        />
        <f7-icon f7="person" color="blue" size="35" v-else></f7-icon>
      </div>
      <div class="desc">
        <div class="name">{{$store.state.currentUser.UserName}}</div>
        <div class="detail">{{$store.state.currentUser.UserId}}</div>
      </div>
      <div class="arrow">
        <f7-icon f7="chevron_right" color="gray" size="25"></f7-icon>
      </div>
    </div>
    <div class="me_settings">
      <div class="setting_line item-link smart-select smart-select-init" data-open-in="popover">
        <div class="setting_line_name">{{ $t('common.language') }}</div>
        <div class="setting_line_right">
          <div class="setting_line_data">{{lang=='zh-cn'?'中':'EN'}}</div>
          <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
        </div>
        <select name="language_select" @change="changeLang">
          <option value="en-us" v-bind:selected="lang=='en-us'">EN</option>
          <option value="zh-cn" v-bind:selected="lang=='zh-cn'">中</option>
        </select>
      </div>
      <div class="setting_line" @click="clickFile">
        <input
          type="file"
          ref="fileinput"
          accept="image/*"
          style="width:0px;height:0px"
          width="0px"
          height="0px"
          @change="uploadFile"
        />
        <div class="setting_line_name">{{ $t('me.pic_setting') }}</div>
        <div class="setting_line_right">
          <div class="setting_line_data"></div>
          <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
        </div>
      </div>
      <div class="setting_line" @click="$f7router.navigate('/changepassword')">
        <div class="setting_line_name">{{ $t('me.change_password') }}</div>
        <div class="setting_line_right">
          <div class="setting_line_data"></div>
          <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">{{ $t('me.last_login_time') }}</div>
        <div class="setting_line_right">
          <div class="setting_line_data">{{$store.state.currentUser.LastLoginTime}}</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">{{$t('common.create_time')}}</div>
        <div class="setting_line_right">
          <div class="setting_line_data">{{$store.state.currentUser.CreateTime}}</div>
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
      lang: window.token_jwt_data.Lang,
      uploading: false,
      fileId: null,
    };
  },
  created() {
    this.$store.commit("getUser");
  },
  methods: {
    logOut() {
      window.location.href =
        this.$axios.defaults.baseURL +
        this.$urls.logout +
        "?returnUrl=" +
        window.location.href;
    },
    uploadFile(e) {
      var files = e.target.files;
      if (files && files.length > 0) {
        const param = new FormData();
        param.append("file", files[0]);
        this.uploading = true;
        this.$axios.post(this.$urls.file.upload, param).then((response) => {
          if (response.code == 0) {
            this.getFileState(response.result);
          }
          this.$refs.fileinput.value = "";
          this.uploading = false;
        });
      }
    },
    getFileState(fileId) {
      var that = this;
      var interval = window.setInterval(function () {
        that.$axios
          .get(that.$urls.file.fileState + "/" + fileId)
          .then((response) => {
            if (response.code == 0 && response.result == 100) {
              that.$store.commit("getUser");
              window.clearInterval(interval);
            }
          });
      }, 500);
    },
    clickFile(e) {
      this.$refs.fileinput.click();
    },
    changeLang($event) {
      var lang = $event.target.value;
      this.$axios
        .get(this.$urls.settings.setLang + "?lang=" + lang)
        .then((response) => {
          if (response.code == 0) {
            this.$funtools.setCookie(this.$cookieName, response.result);
            this.$funtools.parseTokenSetMessage(response.result);
            this.lang = window.token_jwt_data.Lang;
            this.$i18n.locale = lang;
          }
        });
    },
  },
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
  height: 45px;
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
img {
  width: 90%;
  height: 90%;
  border-radius: 50%;
}
</style>
