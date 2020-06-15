<template>
  <f7-page name="personal">
    <f7-navbar title="个人信息" back-link="返回"></f7-navbar>
    <div class="me_content">
      <div class="setting_line">
        <div class="setting_line_name">头像</div>
        <div class="setting_line_right">
          <div class="setting_line_data">
            <f7-icon f7="chevron_right" color="gray" size="25"></f7-icon>
          </div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">用户编号</div>
        <div class="setting_line_right">
          <div class="setting_line_data">CN445379</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">性别</div>
        <div class="setting_line_right">
          <div class="setting_line_data">男</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">手机号码</div>
        <div class="setting_line_right">
          <div class="setting_line_data">18518317472</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">邮箱</div>
        <div class="setting_line_right">
          <div class="setting_line_data">602117818@qq.com</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">证件号</div>
        <div class="setting_line_right">
          <div class="setting_line_data">420983198911107875</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">公司信息</div>
        <div class="setting_line_right">
          <div class="setting_line_data">xx公司</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">部门信息</div>
        <div class="setting_line_right">
          <div class="setting_line_data">xx部门,xx部门</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">上次登录时间</div>
        <div class="setting_line_right">
          <div class="setting_line_data">2019-12-01:13:12:11</div>
        </div>
      </div>
      <div class="setting_line">
        <div class="setting_line_name">创建时间</div>
        <div class="setting_line_right">
          <div class="setting_line_data">2019-12-01:13:12:11</div>
        </div>
      </div>
    </div>
    
  </f7-page>
</template>

<script>
export default {
  name: "personal",
  data() {
    return {};
  },
  methods: {}
};
</script>

<style scoped>
.me_content {
  width: 90%;
  overflow-y: scroll;
  padding-bottom: 30px;
}
.setting_line {
  display: flex;
  height: 50px;
  width: 100%;
  align-items: center;
  border-bottom: 1px solid #e3e4e4;
}

.setting_line .setting_line_name {
  flex: 1;
  display: flex;
}

.setting_line .setting_line_right {
  text-align: center;
  display: flex;
  align-items: center;
}

.setting_line .setting_line_right .setting_line_data {
  display: inline-block;
  margin-right: 10px;
  color: #888;
}

.setting_line_v {
  display: flex;
  width: 100%;
  flex-direction: column;
  align-items: left;
}

.setting_line_v .setting_line_name {
  flex: 1;
  width: 100%;
  display: flex;
  align-items: center;
}

.setting_line_v .setting_line_data {
  flex: 1;
  width: 100%;
  display: flex;
  align-items: flex-end;
}
</style>
