<template>
  <f7-page name="role_update">
    <f7-navbar :title="$t('manage.update_role')" :back-link="$t('common.back')">
      <f7-link @click="saveRole">{{$t('common.save')}}</f7-link>
    </f7-navbar>
    <RoleBase v-if="role.id>=0" :role="role" />
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
            id: response.result.Id,
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
