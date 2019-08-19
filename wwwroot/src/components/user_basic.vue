<template>
  <div>
    <a-input-search placeholder="input search text" style="width: 200px" @search="onSearch" />
    <a-button type="primary" icon="plus"></a-button>
    <a-button type="danger" icon="delete"></a-button>
    <a-table
      :columns="columns"
      :dataSource="data"
      :rowSelection="rowSelection"
      :pagination="{ pageSize: 10 }"
    />
  </div>
</template>
<script>
const data = [];
for (let i = 0; i < 100; i++) {
  data.push({
    key: i,
    id: i,
    name: `Edward King ${i}`,
    mobile: "18518317472",
    email: "xxx.cn.pwc.com",
    pic: "...",
    createTime: "2019-08-01 12:00:00"
  });
}
export default {
  name: "user_basic",
  data() {
    return {
      data,
      columns: [
        {
          title: "用户Id",
          dataIndex: "id"
        },
        {
          title: "用户名称",
          dataIndex: "name"
        },
        {
          title: "手机号",
          dataIndex: "mobile"
        },
        {
          title: "邮箱",
          dataIndex: "email"
        },
        {
          title: "照片",
          dataIndex: "pic"
        },
        {
          title: "创建时间",
          dataIndex: "createTime"
        }
      ],
      rowSelection: {
        onChange: (selectedRowKeys, selectedRows) => {
          window.console.log(
            `selectedRowKeys: ${selectedRowKeys}`,
            "selectedRows: ",
            selectedRows
          );
        },
        onSelect: (record, selected, selectedRows) => {
          window.console.log(record, selected, selectedRows);
        },
        onSelectAll: (selected, selectedRows, changeRows) => {
          window.console.log(selected, selectedRows, changeRows);
        }
      },
      visible:false
    };
  },
  methods: {
    onSearch() {},
    showDrawer() {
      this.visible = true
    },
    onClose() {
      this.visible = false
    }
  }
};
</script>
<style scope>

</style>