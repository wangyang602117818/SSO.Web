<template>
  <f7-page name="user_add">
    <f7-navbar :title="$t('manage.add_user')" :back-link="$t('common.back')">
      <f7-nav-right>
        <f7-link @click="saveUser">{{ $t("common.save") }}</f7-link>
      </f7-nav-right>
    </f7-navbar>
    <UserBase :user="user" />
  </f7-page>
</template>

<script>
import UserBase from "./user_base";
export default {
  name: "user_add",
  props: {
    f7router: Object,
  },
  components: { UserBase },
  data() {
    return {
      user: {
        id: "",
        UserId: "",
        UserName: "",
        Sex: "M",
        Mobile: "",
        Email: "",
        IdCard: "",
        CompanyCode: "",
        Departments: [],
        Roles: [],
      },
    };
  },
  methods: {
    saveUser() {
      if (this.user.UserId.trim() == "") return;
      if (this.user.UserName.trim() == "") return;
      this.$axios.post(this.$urls.user.add, this.user).then((response) => {
        if (response.code == 0) {
          this.$eventbus.emit('useradd',this.user.UserId);
          this.f7router.back();
          this.showSuccess();
        }
      });
    },
  },
};
</script>

<style scoped>
</style>
