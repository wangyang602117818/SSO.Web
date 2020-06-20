<template>
  <f7-list v-if="role">
    <f7-list-input
      required
      validate
      :label="$t('common.role')+$t('common.name')+'*'"
      type="text"
      :placeholder="$t('common.role')+$t('common.name')"
      :error-message="$t('valid.role_name_required')"
      clear-button
      :value="role.name"
      @input="($event)=>{role.name=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      required
      validate
      :label="$t('common.role')+$t('common.description')"
      type="textarea"
      :placeholder="$t('common.role')+$t('common.description')"
      :error-message="$t('valid.role_description_required')"
      clear-button
      :value="role.description"
      @input="($event)=>{role.description=$event.target.value}"
    ></f7-list-input>
  </f7-list>
</template>

<script>
export default {
  name: "role_base",
  props: ["role"],
  data() {
    return {};
  },
  methods: {}
};
</script>

<style scoped>
</style>
