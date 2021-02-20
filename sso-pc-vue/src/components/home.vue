<template>
  <div class="top_layout">
    <a-layout>
      <a-layout-sider
        :trigger="null"
        collapsible
        v-model="collapsed"
        theme="light"
      >
        <div class="logo" theme="light">
          <svg
            t="1578718503704"
            class="icon"
            viewBox="0 0 1060 1024"
            version="1.1"
            p-id="8796"
            data-spm-anchor-id="a313x.7781069.0.i5"
            width="32"
            height="32"
          >
            <path
              d="M246.713379 350.419862l-117.583448-13.029517-70.408828 102.258759 47.174621 115.252966 117.583448 13.029517 70.408828-102.258759L246.713379 350.419862zM192.617931 490.813793l-39.229793-4.625655-15.924966-38.488276 23.304828-33.862621 39.229793 4.625655 15.924966 38.488276L192.617931 490.813793z"
              p-id="8797"
              data-spm-anchor-id="a313x.7781069.0.i7"
              class="selected"
              fill="#0074FF"
            />
            <path
              d="M633.291034 125.881379l-137.110069-58.297379-123.127172 95.620414 13.982897 153.953103 137.110069 58.297379 123.127172-95.620414L633.291034 125.881379zM516.943448 298.584276l-59.038897-30.155034-6.850207-77.082483 52.18869-46.892138 59.038897 30.155034 6.850207 77.082483L516.943448 298.584276z"
              p-id="8798"
              data-spm-anchor-id="a313x.7781069.0.i1"
              class
              fill="#d4237a"
            />
            <path
              d="M940.06731 412.530759l-196.537379-37.393655-135.627034 155.506759 60.910345 192.865103 196.537379 37.393655 135.627034-155.506759L940.06731 412.530759zM840.704 683.431724l-117.583448-22.881103-36.758069-115.358897 80.790069-92.513103 117.583448 22.881103 36.758069 115.394207L840.704 683.431724z"
              p-id="8799"
              data-spm-anchor-id="a313x.7781069.0.i0"
              class
              fill="#00C8FF"
            />
            <path
              d="M459.705379 606.17269l-176.622345 21.751172-67.795862 173.832828 108.826483 152.046345 176.622345-21.715862 67.795862-173.832828L459.705379 606.17269zM452.678621 863.867586l-98.304 12.217379-60.204138-83.897379 38.135172-96.114759 98.304-12.217379 60.204138 83.862069L452.678621 863.867586z"
              p-id="8800"
              data-spm-anchor-id="a313x.7781069.0.i6"
              class
              fill="#0074FF"
            />
          </svg>
          <div class="logo_admin" v-if="!collapsed">SSOAdmin</div>
        </div>
        <a-menu
          theme="light"
          mode="inline"
          :inlineCollapsed="collapsed"
          :defaultSelectedKeys="[routename]"
          :defaultOpenKeys="openKeys"
        >
          <a-menu-item key="overview">
            <router-link to="/overview">
              <a-icon type="pie-chart" />
              <span>{{ $t("overview") }}</span>
            </router-link>
          </a-menu-item>
          <a-sub-menu key="user">
            <span slot="title">
              <a-icon type="user" />
              <span>{{ $t("user") }}</span>
            </span>
            <a-menu-item key="userbasic">
              <router-link to="/userbasic">
                <a-icon type="info-circle" />
                <span>{{ $t("basic") }}</span>
              </router-link>
            </a-menu-item>
          </a-sub-menu>
          <a-menu-item key="role">
            <router-link to="/role">
              <a-icon type="team" />
              <span>{{ $t("role") }}</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="company">
            <router-link to="/company">
              <a-icon type="home" />
              <span>{{ $t("company") }}</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="department">
            <router-link to="/department">
              <a-icon type="cluster" />
              <span>{{ $t("department") }}</span>
            </router-link>
          </a-menu-item>
          <a-sub-menu key="taskmanage">
            <span slot="title">
              <a-icon type="clock-circle" />
              <span>{{ $t("task_scheduling") }}</span>
            </span>
            <a-menu-item key="taskscheduling">
              <router-link to="/taskscheduling">
                <a-icon type="info-circle" />
                <span>{{ $t("scheduling_list") }}</span>
              </router-link>
            </a-menu-item>
            <a-menu-item key="tasktrigger">
              <router-link to="/tasktrigger">
                <a-icon type="info-circle" />
                <span>{{ $t("task_trigger") }}</span>
              </router-link>
            </a-menu-item>
            <a-menu-item key="taskschedulinghistory">
              <router-link to="/taskschedulinghistory">
                <a-icon type="info-circle" />
                <span>{{ $t("task_scheduling_history") }}</span>
              </router-link>
            </a-menu-item>
          </a-sub-menu>
          <a-menu-item key="log">
            <router-link to="/log">
              <a-icon type="align-left" />
              <span>{{ $t("log") }}</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="settings">
            <router-link to="/settings">
              <a-icon type="setting" />
              <span>{{ $t("system_settings") }}</span>
            </router-link>
          </a-menu-item>
        </a-menu>
      </a-layout-sider>
      <a-layout>
        <a-layout-header>
          <a-row>
            <a-col :span="12">
              <a-button type="link" @click="toggleCollapsed">
                <a-icon :type="collapsed ? 'menu-unfold' : 'menu-fold'" />
              </a-button>
            </a-col>
            <a-col :span="12" style="text-align: right">
              <a-dropdown :trigger="['click']">
                <a-button size="small">
                  {{ this.user.StaffName }}
                  <a-icon type="down" />
                </a-button>
                <a-menu slot="overlay">
                  <a-menu-item key="0">
                    <a target="_self" href="/settings">{{ $t("settings") }}</a>
                  </a-menu-item>
                  <a-menu-item key="1">
                    <a target="_self" :href="logOutUrl">{{ $t("logout") }}</a>
                  </a-menu-item>
                </a-menu> </a-dropdown
              >&nbsp;
              <a-button
                size="small"
                @click="changeLang"
                :data-lang="lang == 'zh-cn' ? 'en-us' : 'zh-cn'"
                >{{ $t("lang") }}</a-button
              >&nbsp;
              <a-button size="small">
                <a target="_self" :href="this.$baseUrl">{{ $t("home") }}</a>
              </a-button>
            </a-col>
          </a-row>
        </a-layout-header>
        <a-layout-content>
          <router-view></router-view>
        </a-layout-content>
      </a-layout>
    </a-layout>
  </div>
</template>

<script>
export default {
  name: "home",
  components: {},
  data() {
    return {
      collapsed: false,
      routename: this.$route.name || "overview",
      openKeys: [],
      logOutUrl:
        this.$axios.defaults.baseURL +
        this.$urls.logout +
        "?returnUrl=" +
        window.location.href,
      user: window.token_jwt_data,
      lang: window.token_jwt_data.Lang,
    };
  },
  created() {
    var routeName = this.$route.name;
    var openKeys = [];
    switch (routeName) {
      case "userbasic":
        openKeys = ["user"];
        break;
      case "taskscheduling":
      case "tasktrigger":
      case "taskschedulinghistory":
        openKeys = ["taskmanage"];
        break;
    }
    this.openKeys = openKeys;
  },
  methods: {
    toggleCollapsed() {
      this.collapsed = !this.collapsed;
    },
    changeLang($event) {
      var lang = $event.currentTarget.dataset.lang;
      this.$axios
        .get(this.$urls.settings.setLang + "?lang=" + lang)
        .then((response) => {
          if (response.code == 0) {
            this.$funtools.setCookie(this.$cookieName, response.result);
            this.$funtools.parseTokenSetMessage(response.result);
            this.lang = window.token_jwt_data.Lang;
            this.$i18n.locale = lang;
            //this.$i18n.setLocaleMessage(lang);
          }
        });
    },
  },
};
</script>

<style scope>
.top_layout {
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  display: flex;
  position: absolute;
  top: 0;
  bottom: 0;
  right: 0;
  left: 0;
}
.top_layout .ant-layout-header {
  display: flex;
  height: 50px;
  align-items: center;
  padding-left: 0px;
  padding-right: 10px;
  background-color: #fff;
  border-bottom: 1px solid #e8e8e8;
}
.logo {
  height: 50px;
  display: flex;
  justify-content: center;
  align-items: center;
  border-bottom: 1px solid #e8e8e8;
}
.logo .logo_admin {
  font-weight: 600;
  font-size: 18px;
  margin-left: 5px;
  font-family: -apple-system, BlinkMacSystemFont, Segoe UI, PingFang SC,
    Hiragino Sans GB, Microsoft YaHei, Helvetica Neue, Helvetica, Arial,
    sans-serif, Apple Color Emoji, Segoe UI Emoji, Segoe UI Symbol;
}
.top_layout .ant-layout-content {
  padding: 0px 15px;
}
.ant-layout-sider-light {
  background-color: #f0f2f5;
}
.ant-menu,
.ant-menu-submenu > .ant-menu {
  background-color: #f0f2f5;
}
</style>
