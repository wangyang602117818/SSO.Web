<template>
  <div>
    <a-input-search placeholder="input search text" style="width: 200px" @search="onSearch" />
    <a-button type="primary" icon="plus" @click="showDrawer"></a-button>
    <a-button type="danger" icon="delete"></a-button>
    <a-table
      :columns="columns"
      :dataSource="data"
      :rowSelection="rowSelection"
      :pagination="{ pageSize: 10 }"
    />
    <a-drawer
      title="添加角色"
      :width="360"
      @close="onClose"
      :visible="visible"
      :wrapStyle="{height: 'calc(100% - 108px)',overflow: 'auto',paddingBottom: '108px'}"
    >
      <a-form :form="form" layout="vertical" hideRequiredMark>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Name">
              <a-input
                v-decorator="['name', {
                  rules: [{ required: true, message: '请填写' }]
                }]"
                placeholder="角色名称"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Description">
              <a-textarea placeholder="角色描述" :autosize="{ minRows: 4, maxRows: 6 }" />
            </a-form-item>
          </a-col>
        </a-row>
      </a-form>
      <div>
        <a-button :style="{marginRight: '8px'}" @click="onClose">取 消</a-button>
        <a-button @click="onClose" type="primary">确 定</a-button>
      </div>
    </a-drawer>
  </div>
</template>
<script>
const data = [];
for (let i = 0; i < 100; i++) {
  data.push({
    key: i,
    id: i,
    name: `Edward King ${i}`,
    description: "备份操作员为了备份或还原文件可以替代安全限制",
    createTime: "2019-08-01 12:00:00"
  });
}
export default {
  name: "role",
  data() {
    return {
      data,
      columns: [
        {
          title: "编号",
          dataIndex: "id"
        },
        {
          title: "角色名称",
          dataIndex: "name"
        },
        {
          title: "角色描述",
          dataIndex: "description"
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
      form: this.$form.createForm(this),
      visible: false
    };
  },
  methods: {
    onSearch() {},
    showDrawer() {
      this.visible = true;
    },
    onClose() {
      this.visible = false;
    }
  }
};
</script>
<style>
</style>