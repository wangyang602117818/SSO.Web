<template>
  <a-layout>
    <a-layout-sider theme="light">
      <a-input-search
        placeholder="search company"
        v-model="companySearchValue"
        @search="onSearchCompany"
      />
      <a-menu mode="inline" :defaultSelectedKeys="defaultSelectedCompany" @select="selectCompany">
        <a-spin size="small" v-if="company_loading" style="width:100%" />
        <a-menu-item
          v-for="item in company"
          v-bind:key="item.Code"
          v-bind:code="item.Code"
        >{{item.Name}}</a-menu-item>
      </a-menu>
    </a-layout-sider>
    <a-layout-content>
      <a-input-search
        placeholder="search department"
        style="width:50%"
        @change="onSearchDepartment"
      />
      <a-button type="default" icon="plus" @click="showDrawer" title="添加顶层部门"></a-button>
      <a-button
        type="default"
        :icon="expandedAll?'fullscreen-exit':'fullscreen'"
        @click="expandAll"
      ></a-button>
      <div class="department_wrap">
        <a-spin size="small" v-if="department_loading" style=" width:100%" />
        <a-tree
          :expandedKeys="expandedKeys"
          :autoExpandParent="autoExpandParent"
          @expand="onExpand"
          :treeData="departmentData"
          @select="selectDepartment"
          style="border:0"
        >
          <template slot="custom" slot-scope="{title}">
            <span v-if="title.indexOf(departmentSearchValue) > -1">
              {{title.substr(0, title.indexOf(departmentSearchValue))}}
              <span
                style="color: #f50"
              >{{departmentSearchValue}}</span>
              {{title.substr(title.indexOf(departmentSearchValue) + departmentSearchValue.length)}}
            </span>
            <span v-else>{{title}}</span>
          </template>
        </a-tree>
      </div>
    </a-layout-content>
    <a-layout-sider theme="light" :width="400" :collapsed="collapsedLeft" :collapsedWidth="0">
      <a-tabs defaultActiveKey="1" @change="changeTab">
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
                :autosize="{ minRows: 3, maxRows: 5 }"
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
                :autosize="{ minRows: 3, maxRows: 5 }"
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
var getParentKey = (key, tree) => {
  let parentKey;
  for (let i = 0; i < tree.length; i++) {
    const node = tree[i];
    if (node.children) {
      if (node.children.some(item => item.key === key)) {
        parentKey = node.key;
      } else if (getParentKey(key, node.children)) {
        parentKey = getParentKey(key, node.children);
      }
    }
  }
  return parentKey;
};
var dataList = [];
var generateList = data => {
  for (let i = 0; i < data.length; i++) {
    const node = data[i];
    const key = node.key;
    dataList.push({ key, title: node.title });
    if (node.children) {
      generateList(node.children, node.key);
    }
  }
};
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
      company_loading: false,
      department_loading: false,
      expandedAll: false,
      autoExpandParent: false,
      collapsedLeft: true,
      expandedKeys: []
    };
  },
  created() {
    this.getCompanyData();
  },
  methods: {
    getCompanyData() {
      this.company_loading = true;
      this.$http
        .get(this.$urls.company.getall + "?filter=" + this.companySearchValue)
        .then(response => {
          if (response.body.code == 0) {
            this.company_loading = false;
            this.company = response.body.result;
            if (response.body.count > 0) {
              this.selectedCompany = response.body.result[0].Code;
              this.defaultSelectedCompany.splice(0, 1, this.selectedCompany);
              this.getDepartmentData(this.selectedCompany);
            }
          }
        });
    },
    onSearchCompany() {
      this.getCompanyData();
    },
    onSearchDepartment(e) {
      const value = e.target.value;
      const expandedKeys = dataList
        .map(item => {
          if (item.title.toLowerCase().indexOf(value.toLowerCase()) > -1) {
            return getParentKey(item.key, this.departmentData);
          }
          return null;
        })
        .filter((item, i, self) => item && self.indexOf(item) === i);
      Object.assign(this, {
        expandedKeys,
        departmentSearchValue: value,
        autoExpandParent: true
      });
    },
    getDepartmentData(companyCode) {
      this.department_loading = true;
      this.departmentData = [];
      dataList = [];
      this.$http
        .get(
          this.$urls.department.getdepartments + "?companyCode=" + companyCode
        )
        .then(response => {
          this.department_loading = false;
          if (response.body.code == 0) {
            if (response.body.result.length > 0) {
              this.departmentData = response.body.result;
              generateList(this.departmentData);
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
        this.collapsedLeft = false;
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
        this.collapsedLeft = true;
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
    showDrawer() {
      this.drawerVisible = true;
      this.getRandomCode();
    },
    addSubDept() {
      this.addform.validateFields((error, values) => {
        if (!error) {
          values.companyCode = this.selectedCompany;
          values.parentCode = this.selectedDepartment;
          values.layer = this.selectedDepartmentLayer + 1;
          this.$http.post(this.$urls.department.add, values).then(response => {
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
          values.id = this.id;
          values.companyCode = this.selectedCompany;
          values.parentCode = values.parentCode || "";
          this.$http
            .post(this.$urls.department.update, values)
            .then(response => {
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
}
.department_wrap {
  background-color: #fff;
  min-height: 30px;
  line-height: 30px;
}
</style>