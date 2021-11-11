<template>
  <f7-page name="role_add">
    <f7-navbar :title="$t('manage.add_role')" :back-link="$t('common.back')">
      <f7-link @click="saveRole">{{$t('common.save')}}</f7-link>
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
          this.$f7router.back("",{force:true});
          this.showSuccess();
        }
      });
    }
  }
};
</script>

<style scoped>
</style>
