<template>
  <a-layout>
    <a-layout-sider theme="light">
      <a-input-search placeholder="search company" v-model="companySearchValue" />
      <a-menu mode="inline" :defaultSelectedKeys="defaultSelectedCompany" @select="selectCompany">
        <a-menu-item
          v-for="item in company"
          v-bind:key="item.Code"
          v-bind:code="item.Code"
        >{{item.Name}}</a-menu-item>
      </a-menu>
    </a-layout-sider>
    <a-layout-content>
      <a-input-search placeholder="search department" style="width:50%" />
      <a-button type="default" icon="plus" @click="addTopDept" title="添加顶层部门"></a-button>
      <a-tree
        :defaultExpandedKeys="expandedKeys"
        draggable
        @drop="onDrop"
        :treeData="departmentData"
      >
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
    <a-drawer
      :title="isUpdate?'更新部门':'添加部门'"
      :width="360"
      handle="slot"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" layout="vertical" @submit.prevent="handleSubmit">
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Code">
              <a-input
                placeholder="部门编号"
                v-decorator="['code',{rules: [{ required: true, message: 'Code is required!' }]}]"
              >
                <a-icon slot="addonAfter" type="reload" @click="getRandomCode" />
              </a-input>
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Name">
              <a-input
                placeholder="部门名称"
                v-decorator="['name',{rules: [{ required: true, message: 'Name is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-form-item label="Description">
              <a-textarea
                placeholder="部门描述"
                :autosize="{ minRows: 4, maxRows: 6 }"
                v-decorator="['description',{rules: [{ required: false, message: 'Description is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="16">
          <a-col :span="24">
            <a-button @click="form.resetFields();">取 消</a-button>
            <a-button type="primary" html-type="submit">确 定</a-button>
          </a-col>
        </a-row>
      </a-form>
    </a-drawer>
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
      company: [],
      companyPageIndex: 1,
      companySearchValue: "",
      selectedCompany: "",
      defaultSelectedCompany: [],
      form: this.$form.createForm(this),
      layer: 0,
      parentId: 0,
      departmentData: gData,
      departmentSearchValue: "",
      isUpdate: false,
      drawerVisible: false,
      loading: false,
      expandedKeys: []
    };
  },
  created() {
    this.getCompanyData();
  },
  methods: {
    getCompanyData() {
      this.loading = true;
      this.$http
        .get(
          this.$urls.company.getlist +
            "?pageIndex=" +
            this.companyPageIndex +
            "&filter=" +
            this.companySearchValue
        )
        .then(response => {
          this.loading = false;
          if (response.body.code == 0) {
            if (response.body.count > 0) {
              this.selectedCompany = response.body.result[0].Code;
              this.defaultSelectedCompany.push(response.body.result[0].Code);
            }
            this.company = response.body.result;
          }
        });
    },
    getDepartmentData(companyCode) {
      window.console.log(companyCode);
    },
    getRandomCode() {
      this.form.setFieldsValue({ code: this.$common.randomWord(12, 12) });
    },
    selectCompany(item) {
      this.selectedCompany = item.key;
      this.getDepartmentData(this.selectedCompany);
    },
    onDragEnter() {},
    addTopDept() {
      this.drawerVisible = true;
      this.isUpdate = false;
      this.layer = 0;
      this.parentId = 0;
      this.form.setFieldsValue({ code: this.$common.randomWord(12, 12) });
    },
    addDepartment(dept) {
      dept.CompanyCode = this.selectedCompany;
      this.$http.post(this.$urls.department.add, dept).then(response => {
        if (response.body.code == 400) {
          this.$message.warning("记录已存在!");
        }
        if (response.body.code == 0) {
          this.form.resetFields();
          this.getDepartmentData(this.selectedCompany);
        }
      });
    },
    addDept() {},
    editDept() {},
    updateDepartment(dept) {
      window.console.log(dept);
    },
    delDept() {},
    handleSubmit() {
      var that = this;
      that.form.validateFields((err, values) => {
        if (!err) {
          if (that.isUpdate) that.updateDepartment(values);
          if (!that.isUpdate) that.addDepartment(values);
        }
      });
    },
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
      const data = [...this.departmentData];

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
      this.departmentData = data;
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
.anticon-reload {
  cursor: pointer;
}
.anticon-reload:hover {
  color: #000;
}
</style>