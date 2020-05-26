<template>
  <f7-list v-if="navigation">
    <f7-list-input
      required
      validate
      label="导航名称*"
      type="text"
      placeholder="导航名称"
      error-message="导航名称是必填项!"
      clear-button
      :value="navigation.title"
      @input="($event)=>{navigation.title=$event.target.value}"
    ></f7-list-input>
    <f7-list-input
      required
      validate
      label="链接*"
      type="text"
      placeholder="链接"
      error-message="链接是必填项!"
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
