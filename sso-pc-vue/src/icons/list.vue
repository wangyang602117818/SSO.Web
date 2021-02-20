<template>
  <svg viewBox="0 0 1024 1024" :width="size" :height="size">
    <path d="M938.666667 725.333333v85.333334H341.333333v-85.333334h597.333334z m0-256v85.333334H341.333333v-85.333334h597.333334z m0-256v85.333334H341.333333V213.333333h597.333334zM170.666667 853.333333a85.333333 85.333333 0 1 1 0-170.666666 85.333333 85.333333 0 0 1 0 170.666666z m0-256a85.333333 85.333333 0 1 1 0-170.666666 85.333333 85.333333 0 0 1 0 170.666666z m0-256a85.333333 85.333333 0 1 1 0-170.666666 85.333333 85.333333 0 0 1 0 170.666666z" p-id="5199"></path>
  </svg>
</template>

<script>
export default {
  name: "vue-icon-list",
  props: { size: { type: String, default: "16px" } },
  data() {
    return {};
  },
  methods: {},
};
</script>

<style scoped>
svg path {
  fill: #444;
}
svg:hover path {
  fill: #000;
}
</style>
