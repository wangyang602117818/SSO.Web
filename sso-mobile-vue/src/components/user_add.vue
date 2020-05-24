<template>
  <f7-page name="user_add">
    <f7-navbar title="用户修改" back-link="返回">
      <f7-link slot="right" @click="saveUser">保存</f7-link>
    </f7-navbar>
    <UserBase :user="user" />
  </f7-page>
</template>

<script>
import UserBase from "./user_base";
export default {
  name: "user_add",
  components: { UserBase },
  data() {
    return {
      user: {
        id: "",
        UserId: "",
        UserName: "",
        Sex: "",
        Mobile: "",
        Email: "",
        IdCard: "",
        CompanyCode: "",
        Departments: [],
        Roles: []
      }
    };
  },
  methods: {
    saveUser(user) {
      this.$axios.post(this.$urls.user.add, user).then(response => {
        if (response.code == 0) {
          this.$f7router.back();
          this.showSuccess();
        }
      });
    }
  }
};
</script>

<style scoped>
</style>
