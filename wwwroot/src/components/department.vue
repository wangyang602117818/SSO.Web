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
      <a-button type="default" icon="plus" @click="showDrawer" title="添加顶层部门"></a-button>
      <a-button
        type="default"
        :icon="expandedAll?'fullscreen-exit':'fullscreen'"
        @click="expandAll"
      ></a-button>
      <a-tree
        :expandedKeys="expandedKeys"
        @expand="onExpand"
        :treeData="departmentData"
        @select="selectDepartment"
      >
        <template slot="custom" slot-scope="item">
          <span :layer="item.Layer">{{ item.title }}</span>
        </template>
      </a-tree>
    </a-layout-content>
    <a-layout-sider theme="light" :width="450" :collapsed="collapsedLeft" :collapsedWidth="0">
      <a-tabs defaultActiveKey="1" @change="changeTab" >
        <a-tab-pane tab="添加子部门" key="1" forceRender>
          <a-form :form="addform" @submit.prevent="addSubDept">
            <a-form-item label="Code" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
              <a-input
                placeholder="部门编号"
                v-decorator="['code', {rules: [{ required: true, message: 'Please input your code!' }]}]"
              >
                <a-icon slot="addonAfter" type="reload" @click="getRandomCodeSub" />
              </a-input>
            </a-form-item>
            <a-form-item label="Name" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
              <a-input
                placeholder="部门名称"
                v-decorator="['Name', {rules: [{ required: true, message: 'Please input your name!' }]}]"
              ></a-input>
            </a-form-item>
            <a-form-item label="Order" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
              <a-input-number v-decorator="['order', { initialValue: 0 }]" />
            </a-form-item>
            <a-form-item label="Description" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
              <a-textarea
                placeholder="部门描述"
                :autosize="{ minRows: 4, maxRows: 5 }"
                v-decorator="['description',{rules: [{ required: false, message: 'Description is required!' }]}]"
              />
            </a-form-item>
            <a-form-item :wrapper-col="{ span: 12, offset: 6 }">
              <a-button @click="addform.resetFields();">取 消</a-button>
              <a-button type="primary" html-type="submit">确 定</a-button>
            </a-form-item>
          </a-form>
        </a-tab-pane>
        <a-tab-pane tab="修改部门" key="2" forceRender>
          <a-form :form="updateform" @submit.prevent="updateSubDept">
            <a-form-item label="Code" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
              <a-input
                placeholder="部门编号"
                v-decorator="['code', {rules: [{ required: true, message: 'Please input code!' }]}]"
              >
                <a-icon slot="addonAfter" type="reload" @click="getRandomCodeUpdate" />
              </a-input>
            </a-form-item>
            <a-form-item label="Name" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
              <a-input
                placeholder="部门名称"
                v-decorator="['name', {rules: [{ required: true, message: 'Please input name!' }]}]"
              ></a-input>
            </a-form-item>
            <a-form-item label="Company" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
              <a-select
                v-decorator="[ 'companyCode', {rules: [{ required: true, message: 'Please select company!' }]}]"
                placeholder="所属公司"
              >
                <a-select-option
                  :value="item.Code"
                  v-for="item in company"
                  v-bind:key="item._id"
                >{{item.Name}}</a-select-option>
              </a-select>
            </a-form-item>
            <a-form-item label="上级部门" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
              <a-tree-select
                :treeData="departmentData"
                v-decorator="[ 'parentCode', {rules: [{ required: false }]}]"
                placeholder="上级部门"
                treeDefaultExpandAll
                allowClear
              ></a-tree-select>
            </a-form-item>
            <a-form-item label="Order" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
              <a-input-number v-decorator="['order', { initialValue: 0 }]" />
            </a-form-item>
            <a-form-item label="description" :label-col="{ span: 6 }" :wrapper-col="{ span: 12 }">
              <a-textarea
                placeholder="部门描述"
                :autosize="{ minRows: 4, maxRows: 5 }"
                v-decorator="['description',{rules: [{ required: false, message: 'Description is required!' }]}]"
              />
            </a-form-item>
            <a-form-item :wrapper-col="{ span: 12, offset: 6 }">
              <a-button type="primary" html-type="submit">确 定</a-button>
            </a-form-item>
          </a-form>
        </a-tab-pane>
        <a-tab-pane tab="删除部门" key="3" forceRender>
          <a-popconfirm
            title="Are you sure delete this department?"
            @confirm="delDept"
            okText="Yes"
            cancelText="No"
          >
            <a-button type="danger">删 除</a-button>
          </a-popconfirm>
        </a-tab-pane>
      </a-tabs>
    </a-layout-sider>
    <a-drawer
      :title="'添加顶层部门'"
      :width="360"
      handle="slot"
      @close="drawerVisible=false"
      :visible="drawerVisible"
    >
      <a-form :form="form" layout="vertical" @submit.prevent="handleTopSubmit">
        <a-row :gutter="0">
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
        <a-row :gutter="0">
          <a-col :span="24">
            <a-form-item label="Name">
              <a-input
                placeholder="部门名称"
                v-decorator="['name',{rules: [{ required: true, message: 'Name is required!' }]}]"
              />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="0">
          <a-col :span="24">
            <a-form-item label="Order">
              <a-input-number v-decorator="['order', { initialValue: 0 }]" />
            </a-form-item>
          </a-col>
        </a-row>
        <a-row :gutter="0">
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
        <a-row :gutter="0">
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
export default {
  data() {
    return {
      company: [],
      companyPageIndex: 1,
      companySearchValue: "",
      selectedCompany: "",
      defaultSelectedCompany: [],

      id: "", //选中的部门id
      selectedDepartment: "", //选中部门的code
      selectedDepartmentLayer: 0, //选中部门的层级
      defaultExpandDepartment: true,

      form: this.$form.createForm(this),
      addform: this.$form.createForm(this),
      updateform: this.$form.createForm(this),

      departmentData: [],
      departmentSearchValue: "",
      drawerVisible: false,
      loading: false,
      expandedAll: false,
      collapsedLeft: true,
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
            this.company = response.body.result;
            if (response.body.count > 0) {
              this.selectedCompany = response.body.result[0].Code;
              this.defaultSelectedCompany.push(this.selectedCompany);
              this.getDepartmentData(this.selectedCompany);
            }
          }
        });
    },
    getDepartmentData(companyCode) {
      this.loading = true;
      this.$http
        .get(
          this.$urls.department.getdepartments + "?companyCode=" + companyCode
        )
        .then(response => {
          this.loading = false;
          if (response.body.code == 0) {
            if (response.body.result.length > 0) {
              this.departmentData = response.body.result;
            } else {
              this.departmentData = [];
            }
          }
        });
    },
    deepTraversa(datas, list = []) {
      datas.forEach(item => {
        if (item.children.length > 0) {
          list.push(item.key);
          for (let i = 0; i < item.children.length; i++) {
            this.deepTraversa(item.children, list);
          }
        }
      });
      return list;
    },
    expandAll() {
      var keys = this.deepTraversa(this.departmentData);
      this.expandedAll = !this.expandedAll;
      if (this.expandedAll) {
        this.expandedKeys = keys;
      } else {
        this.expandedKeys = [];
      }
    },
    onExpand(expandedKeys, obj) {
      var key = obj.node.value;
      if (this.expandedKeys.indexOf(key) == -1) {
        this.expandedKeys.push(key);
      } else {
        this.$common.removeArrayItem(this.expandedKeys, key);
      }
    },
    getRandomCode() {
      this.form.setFieldsValue({ code: this.$common.randomWord(12, 12) });
    },
    getRandomCodeSub() {
      this.addform.setFieldsValue({ code: this.$common.randomWord(12, 12) });
    },
    getRandomCodeUpdate() {
      this.updateform.setFieldsValue({ code: this.$common.randomWord(12, 12) });
    },
    selectCompany(item) {
      this.selectedCompany = item.key;
      this.getDepartmentData(this.selectedCompany);
    },
    selectDepartment(selectedKeys) {
      if (selectedKeys.length > 0) {
        this.getRandomCodeSub();
        this.addform.setFieldsValue({ description: "" });
        this.collapsedLeft=false;
        this.$http
          .get(this.$urls.department.get + "?code=" + selectedKeys[0])
          .then(response => {
            if (response.body.code == 0) {
              this.id = response.body.result._id;
              this.selectedDepartment = response.body.result.key;
              this.selectedDepartmentLayer = response.body.result.Layer;
              this.updateform.setFieldsValue({
                code: response.body.result.key,
                name: response.body.result.title,
                order: response.body.result.Order,
                companyCode: this.selectedCompany,
                parentCode: response.body.result.ParentCode,
                description: response.body.result.Description
              });
            }
          });
      } else {
        this.collapsedLeft=true;
        this.id = "";
        this.selectedDepartment = "";
        this.selectedDepartmentLayer = 0;
        this.updateform.setFieldsValue({
          code: "",
          name: "",
          order: "",
          description: ""
        });
      }
    },
    changeTab() {},
    onDragEnter() {},
    showDrawer() {
      this.drawerVisible = true;
      this.getRandomCode();
    },
    addSubDept() {
      this.addform.validateFields((error, values) => {
        if (!error) {
          this.loading = true;
          values.companyCode = this.selectedCompany;
          values.parentCode = this.selectedDepartment;
          values.layer = this.selectedDepartmentLayer + 1;
          this.$http.post(this.$urls.department.add, values).then(response => {
            this.loading = false;
            if (response.body.code == 0) {
              this.getDepartmentData(this.selectedCompany);
            } else {
              this.$message.warning("记录已存在!");
            }
          });
        }
      });
    },
    updateSubDept() {
      this.updateform.validateFields((error, values) => {
        if (!error) {
          this.loading = true;
          values.id = this.id;
          values.companyCode = this.selectedCompany;
          values.parentCode = values.parentCode || "";
          this.$http
            .post(this.$urls.department.update, values)
            .then(response => {
              this.loading = false;
              if (response.body.code == 0) {
                this.getDepartmentData(this.selectedCompany);
              } else {
                this.$message.warning("记录已存在!");
              }
            });
        }
      });
    },
    delDept() {
      if (this.id) {
        this.$http
          .get(this.$urls.department.delete + "/" + this.id)
          .then(response => {
            if (response.body.code == 0) {
              this.id = "";
              this.selectedDepartment = "";
              this.getDepartmentData(this.selectedCompany);
            }
          });
      }
    },
    handleTopSubmit() {
      this.form.validateFields((err, values) => {
        if (!err) {
          values.companyCode = this.selectedCompany;
          values.layer = 0;
          values.parentCode = "";
          this.$http.post(this.$urls.department.add, values).then(response => {
            if (response.body.code == 400) {
              this.$message.warning("记录已存在!");
            }
            if (response.body.code == 0) {
              this.form.resetFields();
              this.getDepartmentData(this.selectedCompany);
            }
          });
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

.anticon-reload {
  cursor: pointer;
}
.anticon-reload:hover {
  color: #000;
}
.ant-tabs {
  background-color: #fff;
  margin-top: 10px;
  padding-bottom: 20px;
  overflow: hidden;
}
.ant-card {
  margin-top: 10px;
}
</style>