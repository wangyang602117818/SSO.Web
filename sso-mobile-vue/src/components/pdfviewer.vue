<template>
  <f7-page>
    <f7-navbar :back-link="filename"> </f7-navbar>
    <f7-card>
      <f7-card-content :padding="false">
        <div id="pdf-content"></div>
      </f7-card-content>
    </f7-card>
  </f7-page>
</template>

<script>
import pdf from "pdfobject";
export default {
  name: "pdfviewer",
  components: { pdf },
  data() {
    return {};
  },
  props: {
    fileid: String,
    filename: String,
  },
  computed: {
    pdfurl() {
      return this.$urls.preview + "/" + this.fileid + "/" + this.filename;
    },
  },
  created() {},
  mounted() {
    pdf.embed(this.pdfurl, "#pdf-content",{height: "50rem",fallbackLink: false});
  },
  methods: {},
};
</script>

<style scoped>
#pdf-content{
    /* height: 600px;
    min-height: 800px; */
}
</style>