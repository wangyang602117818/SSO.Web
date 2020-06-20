<template>
  <f7-list v-if="company">
    <f7-list-input
      required
      validate
      :label="$t('common.company')+$t('common.code')+'*'"
      type="text"
      :placeholder="$t('common.company')+$t('common.code')"
      :error-message="$t('valid.company_code_required')"
      clear-button
      :value="company.code"
      @input="($event)=>{company.code=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      required
      validate
      :label="$t('common.company')+$t('common.name')+'*'"
      type="text"
      :placeholder="$t('common.company')+$t('common.name')"
      :error-message="$t('valid.company_name_required')"
      clear-button
      :value="company.name"
      @input="($event)=>{company.name=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      :label="$t('common.company')+$t('common.description')"
      type="textarea"
      :placeholder="$t('common.company')+$t('common.description')"
      clear-button
      :value="company.description"
      @input="($event)=>{company.description=$event.target.value}"
    ></f7-list-input>
  </f7-list>
</template>

<script>
export default {
  name: "company_base",
  props: ["company"],
  data() {
    return {};
  },
  methods: {}
};
</script>

<style scoped>
</style>
