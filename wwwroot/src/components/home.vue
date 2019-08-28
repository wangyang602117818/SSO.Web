<template>
  <div class="top_layout">
    <a-layout>
      <a-layout-sider :trigger="null" collapsible v-model="collapsed">
        <div class="logo" theme="light" />
        <a-menu
          theme="dark"
          mode="inline"
          :inlineCollapsed="collapsed"
          :defaultSelectedKeys="[routename]"
          :defaultOpenKeys="openKeys"
        >
          <a-menu-item key="overview">
            <router-link to="/overview">
              <a-icon type="pie-chart" />
              <span>概况统计</span>
            </router-link>
          </a-menu-item>
          <a-sub-menu key="user">
            <span slot="title">
              <a-icon type="user" />
              <span>用户管理</span>
            </span>
            <a-menu-item key="userbasic">
              <router-link to="/userbasic">
                <a-icon type="info-circle" />
                <span>基本信息</span>
              </router-link>
            </a-menu-item>
            <a-menu-item key="userauthority">
              <router-link to="/userauthority">
                <a-icon type="info-circle" />
                <span>权限信息</span>
              </router-link>
            </a-menu-item>
          </a-sub-menu>
          <a-menu-item key="role">
            <router-link to="/role">
              <a-icon type="lock" />
              <span>角色管理</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="company">
            <router-link to="/company">
              <a-icon type="lock" />
              <span>公司管理</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="department">
            <router-link to="/department">
              <a-icon type="gold" />
              <span>部门管理</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="log">
            <router-link to="/log">
              <a-icon type="align-left" />
              <span>日志列表</span>
            </router-link>
          </a-menu-item>
          <a-menu-item key="settings">
            <router-link to="/settings">
              <a-icon type="setting" />
              <span>系统设置</span>
            </router-link>
          </a-menu-item>
        </a-menu>
      </a-layout-sider>
      <a-layout>
        <a-layout-header>
          <a-button type="link" @click="toggleCollapsed">
            <a-icon :type="collapsed ? 'menu-unfold' : 'menu-fold'" />
          </a-button>
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
      openKeys:
        this.$route.name == "userbasic" || this.$route.name == "userauthority"
          ? ["user"]
          : []
    };
  },
  created() {},
  methods: {
    toggleCollapsed() {
      this.collapsed = !this.collapsed;
    }
  }
};
</script>

<style>
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
  padding-left: 2px;
  background-color: #fff;
  border-bottom: 1px solid #ccc;
}

.logo {
  height: 50px;
  background: #002140;
}
.top_layout .ant-layout-content {
  padding: 0px 15px;
}
.ant-table-thead > tr > th,
.ant-table-tbody > tr > td {
  padding: 8px 8px !important;
  border: 1px solid #e8e8e8;
}
.ant-input-search {
  margin: 10px 0 !important;
}
.ant-btn-primary,
.ant-btn-default,
.ant-btn-danger {
  margin-left: 10px !important;
}
</style>
