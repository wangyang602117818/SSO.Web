<template>
  <div class="container" v-on:keyup.enter.stop="handleLogin">
    <a class="help" href="doc/index" target="_blank">文档</a>
    <div class="login_wrap">
      <div class="login_title">登录</div>
      <div class="login_content">
        <div class="input_wrap">
          <div :class="userErrorLine ? 'input_content error' : 'input_content'">
            <svg viewBox="64 64 896 896" width="14" height="14" fill="#aaaaaa">
              <path
                d="M858.5 763.6a374 374 0 0 0-80.6-119.5 375.63 375.63 0 0 0-119.5-80.6c-.4-.2-.8-.3-1.2-.5C719.5 518 760 444.7 760 362c0-137-111-248-248-248S264 225 264 362c0 82.7 40.5 156 102.8 201.1-.4.2-.8.3-1.2.5-44.8 18.9-85 46-119.5 80.6a375.63 375.63 0 0 0-80.6 119.5A371.7 371.7 0 0 0 136 901.8a8 8 0 0 0 8 8.2h60c4.4 0 7.9-3.5 8-7.8 2-77.2 33-149.5 87.8-204.3 56.7-56.7 132-87.9 212.2-87.9s155.5 31.2 212.2 87.9C779 752.7 810 825 812 902.2c.1 4.4 3.6 7.8 8 7.8h60a8 8 0 0 0 8-8.2c-1-47.8-10.9-94.3-29.5-138.2zM512 534c-45.9 0-89.1-17.9-121.6-50.4S340 407.9 340 362c0-45.9 17.9-89.1 50.4-121.6S466.1 190 512 190s89.1 17.9 121.6 50.4S684 316.1 684 362c0 45.9-17.9 89.1-50.4 121.6S557.9 534 512 534z"
              />
            </svg>
            <input
              type="text"
              id="userId"
              placeholder="用户编号"
              v-model="userId"
              @input="userChange"
            />
          </div>
          <div class="input_error">
            {{ userIdExists ? "" : "用户编号是必填项" }}
          </div>
        </div>
        <div class="input_wrap">
          <div
            :class="passWordErrorLine ? 'input_content error' : 'input_content'"
          >
            <svg
              viewBox="64 64 896 896"
              width="14px"
              height="14px"
              fill="#aaaaaa"
            >
              <path
                d="M832 464h-68V240c0-70.7-57.3-128-128-128H388c-70.7 0-128 57.3-128 128v224h-68c-17.7 0-32 14.3-32 32v384c0 17.7 14.3 32 32 32h640c17.7 0 32-14.3 32-32V496c0-17.7-14.3-32-32-32zM332 240c0-30.9 25.1-56 56-56h248c30.9 0 56 25.1 56 56v224H332V240zm460 600H232V536h560v304zM484 701v53c0 4.4 3.6 8 8 8h40c4.4 0 8-3.6 8-8v-53a48.01 48.01 0 1 0-56 0z"
              />
            </svg>
            <input
              type="password"
              id="passWord"
              placeholder="用户密码"
              v-model="passWord"
              @input="passWordChange"
            />
          </div>
          <div class="input_error">
            {{ passWordExists ? "" : "用户密码是必填项" }}
          </div>
        </div>
      </div>
      <div class="login_button">
        <input
          type="button"
          class="submit_button"
          :disabled="loading == true"
          value="登 录"
          @click="handleLogin"
        />
      </div>
    </div>
    <div class="footer">
      <a href="https://beian.miit.gov.cn" target="_blank"
        >冀ICP备2021023234号</a
      >
      <a href="doc/index" target="_blank">文档中心</a>
    </div>
    <transition name="move">
      <div class="toast" v-show="mssage_show">
        <svg
          viewBox="0 0 1024 1024"
          version="1.1"
          p-id="2179"
          width="18"
          height="18"
        >
          <path
            d="M512 85.333333a426.666667 426.666667 0 1 0 426.666667 426.666667A426.666667 426.666667 0 0 0 512 85.333333z m42.666667 597.333334a42.666667 42.666667 0 0 1-85.333334 0v-213.333334a42.666667 42.666667 0 0 1 85.333334 0z m-42.666667-298.666667a42.666667 42.666667 0 1 1 42.666667-42.666667 42.666667 42.666667 0 0 1-42.666667 42.666667z"
            fill="#eb8c00"
          />
        </svg>
        <span>用户名或密码不正确！</span>
      </div>
    </transition>
  </div>
</template>
<script>
export default {
  name: "login",
  data() {
    return {
      loading: false,
      userId: "",
      passWord: "",
      userIdExists: true,
      passWordExists: true,
      userErrorLine: false,
      passWordErrorLine: false,
      mssage_show: false,
    };
  },
  mounted() {},
  methods: {
    help() {
      window.open("doc/index");
    },
    userChange(e) {
      var value = e.currentTarget.value;
      this.userIdExists = value == "" ? false : true;
      this.userErrorLine = value == "" ? true : false;
    },
    passWordChange(e) {
      var value = e.currentTarget.value;
      this.passWordExists = value == "" ? false : true;
      this.passWordErrorLine = value == "" ? true : false;
    },
    handleLogin() {
      this.loading = true;
      if (this.userId.trim().length > 0 && this.passWord.trim().length > 0) {
        var returnUrl = this.$funtools.getReturnUrl("returnUrl");
        this.$axios
          .post(this.$urls.login, {
            userId: this.userId,
            passWord: this.passWord,
            returnUrl: returnUrl,
          })
          .then((response) => {
            if (response.code == 0) {
              var returnUrl = response.result;
              window.location.href = decodeURIComponent(returnUrl);
            } else {
              this.mssage_show = true;
              setTimeout(() => {
                this.mssage_show = false;
              }, 2000);
              this.loading = false;
            }
          });
      } else {
        if (this.userId.trim().length == 0) {
          this.userIdExists = false;
          this.userErrorLine = true;
        }
        if (this.passWord.trim().length == 0) {
          this.passWordExists = false;
          this.passWordErrorLine = true;
        }
        this.loading = false;
      }
    },
  },
};
</script>
<style scoped>
html,
body {
  margin: 0;
  padding: 0;
  font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", "PingFang SC",
    "Hiragino Sans GB", "Microsoft YaHei", "Helvetica Neue", Helvetica, Arial,
    sans-serif, "Apple Color Emoji", "Segoe UI Emoji", "Segoe UI Symbol";
}

.container {
  position: absolute;
  left: 0;
  bottom: 0;
  right: 0;
  top: 0;
  background-color: rgba(226, 226, 226, 1);
  display: flex;
  justify-content: center;
  align-items: center;
}
.help {
  position: absolute;
  height: 50px;
  width: 50px;
  right: 15px;
  top: 15px;
  background-color: #fff;
  border-radius: 25px;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #096dd9;
  color: #fff;
  text-decoration: none;
  box-shadow: 0px 2px 4px rgba(0,0,0,0.4);
}
.help:hover {
  background-color: #40a9ff;
  cursor: pointer;
}
.login_wrap {
  background-color: #fff;
  width: 315px;
  height: 300px;
  display: flex;
  flex-direction: column;
  border-radius: 2px;
  box-sizing: border-box;
  border: 1px solid #e8e8e8;
}

.login_title {
  height: 57px;
  border-bottom: 1px solid #ddd;
  display: flex;
  align-items: center;
  padding: 0 24px;
}

.login_content {
  padding: 20px 24px 10px 24px;
  flex: 1;
  display: flex;
  flex-direction: column;
}

.input_wrap {
  flex: 1;
  display: flex;
  align-items: center;
  flex-direction: column;
  position: relative;
}

.input_wrap svg {
  position: absolute;
  left: 10px;
  margin-top: -1px;
}

.input_wrap input {
  width: 100%;
  border: 1px solid #d9d9d9;
  box-sizing: border-box;
  height: 40px;
  border-radius: 4px;
  padding: 6px 11px;
  padding-left: 28px;
  font-size: 15px;
}

.input_wrap input:focus {
  border-color: #40a9ff;
  border-right-width: 1px !important;
  outline: 0;
  -webkit-box-shadow: 0 0 0 2px rgba(24, 144, 255, 0.2);
  box-shadow: 0 0 0 2px rgba(24, 144, 255, 0.2);
}

.input_wrap .error input {
  border-color: #ff4d4f;
  border-right-width: 1px !important;
  outline: 0;
}

.input_wrap .error input:focus {
  background-color: #fff;
  border-color: #f5222d;
  -webkit-box-shadow: 0 0 0 2px rgba(245, 34, 45, 0.2);
  box-shadow: 0 0 0 2px rgba(245, 34, 45, 0.2);
}

.input_wrap .input_content {
  width: 100%;
  flex: 1;
  display: flex;
  justify-content: center;
  align-items: center;
}

.input_wrap .input_error {
  width: 100%;
  height: 25px;
  line-height: 25px;
  font-size: 12px;
  color: red;
}

.login_button {
  height: 90px;
  padding: 0 24px;
}

.submit_button {
  color: #fff;
  background-color: #1890ff;
  text-shadow: 0 -1px 0 rgba(0, 0, 0, 0.12);
  -webkit-box-shadow: 0 2px 0 rgba(0, 0, 0, 0.045);
  box-shadow: 0 2px 0 rgba(0, 0, 0, 0.045);
  cursor: pointer;
  height: 36px;
  padding: 0 15px;
  width: 100%;
  outline: 0;
  border: 1px solid transparent;
  line-height: 1.499;
  border-radius: 4px;
  font-weight: 600;
  font-size: 14px;
}

.submit_button:hover {
  background-color: #40a9ff;
  border-color: #40a9ff;
}

.submit_button:active {
  background-color: #096dd9;
  border-color: #096dd9;
}

.submit_button[disabled] {
  background-color: #c0c0c0;
  border-color: #c0c0c0;
  cursor: default;
}
.footer {
  height: 35px;
  font-size: 12px;
  background-color: #fff;
  width: 100%;
  position: fixed;
  bottom: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  color: #bbb;
}
.footer a {
  color: #bbb;
  text-decoration: none;
  margin-right: 30px;
}
.footer a:hover {
  color: #000;
}
.toast {
  position: absolute;
  background-color: #fff;
  top: 20px;
  padding: 10px 16px;
  border-radius: 4px;
  color: rgba(0, 0, 0, 0.65);
  font-size: 14px;
  font-weight: 100;
  -webkit-box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  display: flex;
  align-items: center;
}

.toast span {
  margin-left: 5px;
}

.move-enter,
.move-leave-to {
  transform: translateY(-200%);
}
.move-enter-active,
.move-leave-active {
  transition: 200ms;
}
.move-enter-to {
  transform: translateY(0);
}
</style>
