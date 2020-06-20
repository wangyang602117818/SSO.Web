<template>
  <f7-list v-if="navigation">
    <f7-list-input
      required
      validate
      :label="$t('common.navigator')+$t('common.name')+'*'"
      type="text"
      :placeholder="$t('common.navigator')+$t('common.name')"
      :error-message="$t('valid.navigator_name_required')"
      clear-button
      :value="navigation.title"
      @input="($event)=>{navigation.title=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      required
      validate
      :label="$t('manage.link')+'*'"
      type="text"
      :placeholder="$t('manage.link')"
      :error-message="$t('valid.link_required')"
      clear-button
      :value="navigation.baseUrl"
      @input="($event)=>{navigation.baseUrl=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      label="Logo"
      type="text"
      placeholder="Logo"
      clear-button
      :value="navigation.logoUrl"
      @input="($event)=>{navigation.logoUrl=$event.target.value}"
    ></f7-list-input>
  </f7-list>
</template>

<script>
export default {
  name: "navigation_base",
  props: ["navigation"],
  data() {
    return {};
  },
  methods: {}
};
</script>

<style scoped>
</style>
