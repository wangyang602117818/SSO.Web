<template>
  <f7-page name="change_password">
    <f7-navbar :title="$t('me.change_password')" :back-link="$t('common.back')">
      <f7-nav-right>
      <f7-link @click="save">{{$t('common.save')}}</f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-list inline-labels>
      <f7-list-input
        :label="$t('me.old_password')+'*'"
        type="password"
        :placeholder="$t('me.old_password')"
        :error-message="$t('valid.old_password_required')"
        required
        validate
        clear-button
        :value="oldPassword"
        @input="($event)=>{oldPassword=$event.target.value}"
      ></f7-list-input>
      <f7-list-input
        :label="$t('me.new_password')+'*'"
        type="password"
        :placeholder="$t('me.new_password')"
        :error-message="$t('valid.password_pattern_invalid')"
        pattern="(?=.*[0-9])(?=.*[a-zA-Z]).{6,30}"
        required
        validate
        clear-button
        :value="newPassword"
        @input="($event)=>{newPassword=$event.target.value}"
      ></f7-list-input>
      <f7-list-input
        :label="$t('me.new_password_confirm')+'*'"
        type="password"
        :placeholder="$t('me.new_password_confirm')"
        :error-message="$t('valid.password_pattern_invalid')"
        pattern="(?=.*[0-9])(?=.*[a-zA-Z]).{6,30}"
        required
        validate
        clear-button
        :value="newPasswordConfirm"
        @input="($event)=>{newPasswordConfirm=$event.target.value}"
      ></f7-list-input>
    </f7-list>
  </f7-page>
</template>

<script>
export default {
  name: "change_password",
  props: {
    f7router: Object
  },
  data() {
    return {
      oldPassword: "",
      newPassword: "",
      newPasswordConfirm: ""
    };
  },
  methods: {
    save() {
      if (!this.oldPassword) return;
      if (!this.newPassword) return;
      if (!this.newPasswordConfirm) return;
      if (this.newPassword != this.newPasswordConfirm) {
        this.showInfo(this.$t("me.two_passwords_not_match"));
        return;
      }
      this.$axios
        .post(this.$urls.user.updatepassword, {
          oldPassword: this.oldPassword,
          newPassword: this.newPassword
        })
        .then(response => {
          if (response.code == 0) {
            this.showSuccess();
            window.location.href =
              this.$axios.defaults.baseURL +
              this.$urls.logout +
              "?returnUrl=" +
              window.location.href;
          }
        });
    }
  }
};
</script>

<style scoped>
</style>
