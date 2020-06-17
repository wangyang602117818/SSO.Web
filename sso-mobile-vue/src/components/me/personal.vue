<template>
  <f7-page name="personal">
    <f7-navbar title="个人信息" back-link="返回">
      <f7-link slot="right" @click="save">保存</f7-link>
    </f7-navbar>
    <f7-list inline-labels>
      <f7-list-item title="用户编号" after="CN115936"></f7-list-item>
      <f7-list-item title="性别" link="#" after="男"></f7-list-item>
      <f7-list-input label="手机号码" type="text" value="18518317472"></f7-list-input>
      <f7-list-input label="邮箱" type="text" value="602117818@qq.com"></f7-list-input>
      <f7-list-input label="证件号" type="text" value="420983198911107875"></f7-list-input>
      <f7-list-item title="公司" link="#" after="xx公司"></f7-list-item>
      <f7-list-item title="部门" link="#" after="xx部门"></f7-list-item>
      <f7-list-item title="上次登录时间" after="2019-12-01:13:12:11"></f7-list-item>
      <f7-list-item title="创建时间" after="2019-12-01:13:12:11"></f7-list-item>
    </f7-list>
  </f7-page>
</template>

<script>
export default {
  name: "personal",
  data() {
    return {};
  },
  methods: {
    save() {
      
    }
  }
};
</script>

<style scoped>
.me_content {
  width: 90%;
  margin: 0 auto;
  overflow-y: scroll;
  padding-bottom: 30px;
}
</style>
