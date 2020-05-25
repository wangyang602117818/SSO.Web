<template>
  <f7-page name="role_add">
    <f7-navbar title="角色修改" back-link="返回">
      <f7-link slot="right" @click="saveRole">保存</f7-link>
    </f7-navbar>
    <RoleBase v-if="role.id" :role="role" />
  </f7-page>
</template>

<script>
import RoleBase from "./role_base";
export default {
  name: "role_update",
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
  created() {
    this.getData();
  },
  methods: {
    saveRole() {
      if (this.role.name.trim() == "") return;
      if (this.role.description.trim() == "") return;
      this.$axios.post(this.$urls.role.update, this.role).then(response => {
        if (response.code == 0) {
          this.$f7router.back();
          this.showSuccess();
        }
      });
    },
    getData() {
      var id = this.$f7route.params.id;
      this.$axios.get(this.$urls.role.getById + "/" + id).then(response => {
        if (response.code === 0) {
          this.role = {
            id: response.result._id,
            name: response.result.Name,
            description: response.result.Description
          };
        }
      });
    }
  }
};
</script>

<style scoped>
</style>
