<template>
  <svg viewBox="0 0 1024 1024" :width="size" :height="size">
    <path
      d="M512 62C263.47167969 62 62 263.47255859 62 512c0 248.52832031 201.47167969 450 450 450 248.52919922 0 450-201.47167969 450-450C962 263.47255859 760.52919922 62 512 62z m0 829.32011719C302.50742187 891.32011719 132.67988281 721.49257813 132.67988281 512S302.50742187 132.67988281 512 132.67988281 891.32011719 302.50742187 891.32011719 512 721.49257813 891.32011719 512 891.32011719z"
      p-id="4520"
    ></path>
    <path d="M371.375 371.375h281.25v281.25H371.375z"></path>
  </svg>
</template>

<script>
export default {
  name: "vue-icon-end",
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
