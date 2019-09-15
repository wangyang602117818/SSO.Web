<template>
  <a-tree-select
    style="width: 300px"
    :dropdownStyle="{ maxHeight: '400px', overflow: 'auto' }"
    :treeData="treeData"
    placeholder="Please select"
    treeDefaultExpandAll
    v-model="value"
  >
    <span
      style="color: #08c"
      slot="title"
      slot-scope="{key, value}"
      v-if="key='0-0-1'"
    >Child Node1 {{value}}</span>
  </a-tree-select>
</template>
<script>
const treeData = [{
  title: 'Node1',
  value: '0-0',
  key: '0-0',
  children: [{
    value: '0-0-1',
    key: '0-0-1',
    scopedSlots: { // custom title
      title: 'title',
    },
  }, {
    title: 'Child Node2',
    value: '0-0-2',
    key: '0-0-2',
  }],
}, {
  title: 'Node2',
  value: '0-1',
  key: '0-1',
}]
export default {
  name: "user_basic",
  data () {
    return {
      value: undefined,
      treeData,
    }
  },
  watch: {
    value (value) {
      console.log(value)
    },
  },
};
</script>
<style scoped>
h1 {
  color: RED;
}
</style>