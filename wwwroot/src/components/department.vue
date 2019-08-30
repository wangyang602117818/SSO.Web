<template>
  <a-layout>
    <a-layout-sider theme="light">
      <a-input-search placeholder="search company" />
      <a-menu mode="inline" :defaultSelectedKeys="['1']">
        <a-menu-item key="1">option1</a-menu-item>
        <a-menu-item key="2">option2</a-menu-item>
        <a-menu-item key="3">option3</a-menu-item>
        <a-menu-item key="4">option4</a-menu-item>
      </a-menu>
    </a-layout-sider>
    <a-layout-content>
      <a-input-search placeholder="search department" style="width:50%" />
      <a-button type="default" icon="plus" @click="addTopDept" title="添加顶层部门"></a-button>
      <a-tree :defaultExpandedKeys="expandedKeys" draggable @drop="onDrop" :treeData="gData">
        <template slot="custom" slot-scope="item">
          <span>{{ item.title }}</span>
          <span class="tree_action_wrap">
            <a-icon type="plus" class="tree_action" @click.prevent="addDept" />
            <a-icon type="edit" class="tree_action" @click.prevent="editDept" />
            <a-icon type="close" class="tree_action" @click.prevent="delDept" />
          </span>
        </template>
      </a-tree>
    </a-layout-content>
    <a-layout-content></a-layout-content>
  </a-layout>
</template>
<script>
const gData = [
  {
    key: 100,
    title: "01",
    scopedSlots: { title: "custom" },
    children: [
      { key: 101, title: "01-01", scopedSlots: { title: "custom" } },
      { key: 102, title: "01-02", scopedSlots: { title: "custom" } },
      { key: 103, title: "01-03", scopedSlots: { title: "custom" } }
    ]
  },
  {
    key: 200,
    title: "02",
    scopedSlots: { title: "custom" },
    children: [
      { key: 201, title: "02-01", scopedSlots: { title: "custom" } },
      ,
      { key: 202, title: "02-02", scopedSlots: { title: "custom" } }
    ]
  },
  {
    key: 300,
    title: "03",
    scopedSlots: { title: "custom" },
    children: [
      { key: 301, title: "03-01", scopedSlots: { title: "custom" } },
      { key: 302, title: "03-02", scopedSlots: { title: "custom" } }
    ]
  },
  {
    key: 4,
    title: "04",
    scopedSlots: { title: "custom" },
    children: [
      { key: 400, title: "04-01", scopedSlots: { title: "custom" } },
      { key: 402, title: "04-02", scopedSlots: { title: "custom" } }
    ]
  }
];

export default {
  data() {
    return {
      gData,
      searchCompanyValue: "",
      searchDepartmentValue: "04-01",
      showTools: false,
      expandedKeys: []
    };
  },
  methods: {
    onDragEnter() {},
    addTopDept() {},
    addDept() {},
    editDept() {},
    delDept() {},
    onDrop(info) {
      //放的位置的key
      const dropKey = info.node.eventKey;
      window.console.log("dropKey:" + dropKey);
      //拖动的项的key
      const dragKey = info.dragNode.eventKey;
      window.console.log("dragKey:" + dragKey);

      const dropPos = info.node.pos.split("-");
      window.console.log("dropPos:" + dropPos);

      const dropPosition =
        info.dropPosition - Number(dropPos[dropPos.length - 1]);
      window.console.log("dropPosition:" + dropPosition);

      const loop = (data, key, callback) => {
        data.forEach((item, index, arr) => {
          if (item.key === key) {
            return callback(item, index, arr);
          }
          if (item.children) {
            return loop(item.children, key, callback);
          }
        });
      };
      const data = [...this.gData];

      // 查找被拖动的项
      let dragObj;
      loop(data, dragKey, (item, index, arr) => {
        arr.splice(index, 1);
        dragObj = item;
      });
      window.console.log("dragObj:" + JSON.stringify(dragObj));

      if (!info.dropToGap) {
        //放在了正文上
        // Drop on the content
        loop(data, dropKey, item => {
          item.children = item.children || [];
          // where to insert 示例添加到尾部，可以是随意位置
          item.children.push(dragObj);
        });
      } else if (
        (info.node.children || []).length > 0 && // Has children
        info.node.expanded && // Is expanded
        dropPosition === 1 // On the bottom gap
      ) {
        loop(data, dropKey, item => {
          item.children = item.children || [];
          // where to insert 示例添加到尾部，可以是随意位置
          item.children.unshift(dragObj);
        });
      } else {
        let ar;
        let i;
        loop(data, dropKey, (item, index, arr) => {
          ar = arr;
          i = index;
        });
        if (dropPosition === -1) {
          ar.splice(i, 0, dragObj);
        } else {
          ar.splice(i + 1, 0, dragObj);
        }
      }
      this.gData = data;
    }
  }
};
</script>
<style scoped>
.ant-layout-sider {
  background-color: transparent;
}
.ant-tree {
  border: 1px solid #e8e8e8;
  background-color: #fff;
}
.ant-layout-content {
  overflow: hidden;
}
.ant-tree-node-content-wrapper:hover .tree_action_wrap,
.ant-tree-treenode-selected .tree_action_wrap {
  display: inline-block;
}
.tree_action_wrap {
  display: none;
  margin-left: 10px;
}
.tree_action {
  margin-left: 10px;
}
.tree_action:hover {
  color: red;
}
</style>