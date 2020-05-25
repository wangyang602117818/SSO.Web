<template>
  <f7-page name="role_add">
    <f7-navbar title="角色添加" back-link="返回">
      <f7-link slot="right" @click="saveRole">保存</f7-link>
    </f7-navbar>
    <RoleBase :role="role" />
  </f7-page>
</template>

<script>
import RoleBase from "./role_base";
export default {
  name: "role_add",
  components: { RoleBase },
  data() {
    return {
      role: {
        id: "",
        name: "",
        description: ""
      }
    };
  },
  methods: {
    saveRole() {
      if (this.role.name.trim() == "") return;
      if (this.role.description.trim() == "") return;
      this.$axios.post(this.$urls.role.add, this.role).then(response => {
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
