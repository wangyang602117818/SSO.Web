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
      <a-input-search placeholder="search department" />
      <a-tree
        :defaultExpandedKeys="expandedKeys"
        draggable
        @dragenter="onDragEnter"
        @drop="onDrop"
        :treeData="gData"
      >
        <template slot="title" slot-scope="{title}">
          <span v-if="title.indexOf(searchDepartmentValue) > -1">
            {{title.substr(0, title.indexOf(searchDepartmentValue))}}
            <span
              style="color: #f50"
            >{{searchDepartmentValue}}</span>
            {{title.substr(title.indexOf(searchDepartmentValue) + searchDepartmentValue.length)}}
          </span>
          <span v-else>{{title}}</span>
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
    children: [
      { key: 101, title: "01-01" },
      { key: 102, title: "01-02" },
      { key: 103, title: "01-03" }
    ]
  },
  {
    key: 200,
    title: "02",
    children: [{ key: 201, title: "02-01" }, { key: 202, title: "02-02" }]
  },
  {
    key: 300,
    title: "03",
    children: [{ key: 301, title: "03-01" }, { key: 302, title: "03-02" }]
  },
  {
    key: 4,
    title: "04",
    children: [{ key: 400, title: "04-01" }, { key: 402, title: "04-02" }]
  }
];

export default {
  data() {
    return {
      gData,
      searchCompanyValue: "",
      searchDepartmentValue: "04-01",
      expandedKeys: []
    };
  },
  methods: {
    onDragEnter() {},
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
.ant-tree-treenode-selected {
  background-color: #ccc !important;
}
</style>