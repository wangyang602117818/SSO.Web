<template>
  <f7-page name="user_update">
    <f7-navbar :title="$t('manage.update_user')" :back-link="$t('common.back')">
      <f7-nav-right>
        <f7-link @click="saveUser" v-if="user.id">{{
          $t("common.save")
        }}</f7-link>
      </f7-nav-right>
    </f7-navbar>
    <UserBase v-if="user.id" :user="user" />
  </f7-page>
</template>

<script>
import UserBase from "./user_base";
export default {
  name: "user_update",
  components: { UserBase },
  props: {
    f7router: Object,
    userId: String,
  },
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
        Roles: [],
      },
    };
  },
  created() {
    this.getData();
  },
  methods: {
    saveUser() {
      if (this.user.UserId.trim() == "") return;
      if (this.user.UserName.trim() == "") return;
      this.$axios.post(this.$urls.user.update, this.user).then((response) => {
        if (response.code === 0) {
          this.$eventbus.emit('userupdate',this.user);
          this.f7router.back();
          this.showSuccess();
        }
      });
    },
    getData() {
      var userId = this.userId;
      this.$axios
        .get(this.$urls.user.getbyuserid + "?userid=" + userId)
        .then((response) => {
          if (response.code === 0) {
            this.user = {
              id: response.result.Id,
              UserId: response.result.UserId,
              UserName: response.result.UserName,
              Sex: response.result.Sex,
              Mobile: response.result.Mobile,
              Email: response.result.Email,
              IdCard: response.result.IdCard,
              CompanyCode: response.result.CompanyCode,
              Departments: response.result.DepartmentCode,
              Roles: response.result.Role,
            };
          }
        });
    },
  },
};
</script>

<style scoped>
</style>
