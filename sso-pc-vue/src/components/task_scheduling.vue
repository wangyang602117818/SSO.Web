<template>
  <div>
    <a-row type="flex" align="middle">
      <a-col>
        <a-input-search
          :placeholder="$t('search')"
          style="width: 200px"
          @search="onSearch"
          v-model="searchValue"
        />
        <!-- <a-button
          type="primary"
          icon="plus"
          :title="$t('add')"
          @click="showDrawer()"
        ></a-button> -->
        <a-button
          type="default"
          icon="redo"
          @click="reload"
          :title="$t('refresh')"
        ></a-button>
        <a-button
          type="default"
          icon="edit"
          @click="editScheduling"
          :disabled="selectedRowKeys.length != 1"
          :title="$t('edit')"
        ></a-button>
        <!-- <a-popconfirm
          :title="
            selectedRows.length == 1 && selectedRows[0].Enable
              ? $t('disabled')
              : $t('enable')
          "
          @confirm="enableScheduling"
          :okText="$t('yes')"
          :cancelText="$t('no')"
        >
          <a-button
            type="default"
            icon="stop"
            :title="$t('enable') + '/' + $t('disabled')"
            :disabled="selectedRowKeys.length != 1"
          ></a-button>
        </a-popconfirm> -->
        <a-popconfirm
          :title="$t('confirm_delete')"
          @confirm="removeScheduling"
          :okText="$t('yes')"
          :cancelText="$t('no')"
          v-if="this.showDelete == false"
        >
          <a-button
            type="danger"
            icon="delete"
            :title="$t('delete')"
            :disabled="selectedRowKeys.length == 0"
          ></a-button>
        </a-popconfirm>
      </a-col>
    </a-row>
    <a-table
      :columns="columns"
      :rowKey="(record) => record.Id"
      :dataSource="data"
      :rowSelection="{
        selectedRowKeys: selectedRowKeys,
        onChange: onSelectChange,
      }"
      :loading="loading"
      :pagination="pagination"
      :rowClassName="tableRowClass"
      @change="handleTableChange"
    >
      <a-tag
        slot="Status"
        slot-scope="text, record"
        :color="record.Status == 0 ? '#108ee9' : '#f50'"
        v-if="record.Enable"
        >{{ record.Status == 0 ? $t("running") : $t("stoped") }}</a-tag
      >
      <template slot="Operation" slot-scope="text, record" v-if="record.Enable && record.Crons">
        <a-popconfirm
          v-if="data.length"
          :okText="$t('yes')"
          :cancelText="$t('no')"
          :title="
            (record.Status == -1
              ? $t('start_scheduling')
              : $t('stop_scheduling')) + '?'
          "
          @confirm="() => statusOperation(record.Id, record.Status)"
        >
          <span :title="$t('start_scheduling')" v-if="record.Status == -1">
            <start size="18px"/>
            </span>
          <span v-else :title="$t('stop_scheduling')">
            <end size="18px" />
          </span>
        </a-popconfirm>
        <!-- <span :title="$t('view_scheduling_history')" style="margin-left: 8px">
          <list size="18px" />
        </span> -->
      </template>
    </a-table>

    <a-drawer
      :title="isUpdate ? $t('update_scheduling') : $t('add_scheduling')"
      :width="550"
      @close="drawerVisible = false"
      :visible="drawerVisible"
    >
      <a-form :form="form" @submit.prevent="handleSubmit">
        <a-form-item
          :label="$t('name')"
          :label-col="{ span: 4 }"
          :wrapper-col="{ span: 20 }"
        >
          <a-input
            disabled
            :placeholder="$t('name')"
            v-decorator="[
              'name',
              {
                rules: [{ required: true, message: $t('name_required') }],
              },
            ]"
          />
        </a-form-item>
        <a-form-item
          :label="$t('description')"
          :label-col="{ span: 4 }"
          :wrapper-col="{ span: 20 }"
        >
          <a-textarea
            :placeholder="$t('description')"
            :autoSize="{ minRows: 3, maxRows: 6 }"
            v-decorator="[
              'description',
              {
                rules: [
                  {
                    required: true,
                    message: $t('description_required'),
                  },
                ],
              },
            ]"
          />
        </a-form-item>
        <a-form-item
          :label="$t('trigger')"
          :label-col="{ span: 4 }"
          :wrapper-col="{ span: 20 }"
        >
          <a-select
            show-search
            allowClear
            v-decorator="[
              'trigger',
              { rules: [{ required: true, message: $t('trigger_required') }] },
            ]"
          >
            <a-select-option
              :value="item.Id"
              v-for="item in triggers"
              v-bind:key="item.Id"
              >{{ item.Description }}</a-select-option
            >
          </a-select>
        </a-form-item>
        <a-divider />
        <a-button @click="form.resetFields()">{{ $t("reset") }}</a-button>
        <a-button type="primary" html-type="submit">{{
          $t("submit")
        }}</a-button>
      </a-form>
    </a-drawer>
  </div>
</template>

<script>
import base from "./Base";
import start from "../icons/start";
import end from "../icons/end";
// import list from "../icons/list";
export default {
  name: "taskscheduling",
  components: { start, end },
  mixins: [base],
  data() {
    return {
      getlist: this.$urls.taskscheduling.getSchedulingList,
      triggers: [],
      showDelete: false,
    };
  },
  computed: {
    columns: function () {
      return [
        {
          title: this.$t("id"),
          dataIndex: "Id",
          width: "5%",
        },
        {
          title: this.$t("name"),
          dataIndex: "Name",
          width: "10%",
        },
        {
          title: this.$t("description"),
          dataIndex: "Description",
          width: "15%",
          ellipsis: true,
        },
        {
          title: this.$t("task_trigger"),
          dataIndex:
            this.$i18n.locale == "zh-cn" ? "CronsZh" : "CronsCh",
          width: "15%",
          ellipsis: true,
        },
        {
          title: this.$t("state"),
          dataIndex: "Status",
          width: "5%",
          scopedSlots: { customRender: "Status" },
        },
        {
          title: this.$t("last_run_time"),
          dataIndex: "LastRunTime",
          width: "13%",
        },
        {
          title: this.$t("next_run_time"),
          dataIndex: "NextRunTime",
          width: "13%",
        },
        {
          title: this.$t("last_run_result"),
          dataIndex: "LastRunResult",
          width: "34%",
          ellipsis: true,
        },
        {
          title: this.$t("operation"),
          width: "5%",
          ellipsis: true,
          scopedSlots: { customRender: "Operation" },
        },
      ];
    },
  },
  created() {},
  mounted() {
    this.getTriggerList();
  },
  methods: {
    getQuerystring() {
      var url =
        "?pageIndex=" +
        this.pagination.current +
        "&pageSize=" +
        this.pagination.pageSize;
      if (this.searchValue) url += "&searchValue=" + this.searchValue;
      return url;
    },
    tableRowClass(record, index) {
      if (!record.Enable) return "disabled";
      return "";
    },
    getTriggerList() {
      this.$axios
        .get(this.$urls.taskscheduling.gettriggerlist + "?pageSize=1000")
        .then((response) => {
          if (response.code == 0) {
            this.triggers = response.result;
          }
        });
    },
    handleSubmit() {
      this.form.validateFields((err, values) => {
        if (!err) {
          if (this.isUpdate) this.updateScheduling(values);
          if (!this.isUpdate) this.addScheduling(values);
        }
      });
    },
    updateScheduling(values) {
      values.id = this.selectedRows[0].Id;
      this.$axios
        .post(this.$urls.taskscheduling.updateScheduling, values)
        .then((response) => {
          if (response.code == 0) {
            this.getData();
            this.$message.warning(this.$t("modify_success"));
          }
        });
    },
    enableScheduling() {
      var id = this.selectedRows[0].Id;
      var enable = !this.selectedRows[0].Enable;
      this.$axios
        .get(
          this.$urls.taskscheduling.enableScheduling +
            "?id=" +
            id +
            "&enable=" +
            enable
        )
        .then((response) => {
          if (response.code == 0) {
            this.getData();
            this.selectedRows = [];
            this.selectedRowKeys = [];
            this.$message.warning(this.$t("modify_success"));
          }
        });
    },
    addScheduling(values) {
      this.$axios
        .post(this.$urls.taskscheduling.addScheduling, values)
        .then((response) => {
          if (response.code == 0) {
            this.getData();
          }
        });
    },
    showDrawer(id) {
      if (id) {
        this.isUpdate = true;
      } else {
        this.isUpdate = false;
      }
      this.drawerVisible = true;
    },
    editScheduling(e) {
      this.$axios
        .get(
          this.$urls.taskscheduling.getSchedulingById +
            "/" +
            this.selectedRowKeys[0]
        )
        .then((response) => {
          if (response.code == 0) {
            this.showDrawer(response.result.Id);
            this.$nextTick(function () {
              this.form.setFieldsValue({
                name: response.result.Name,
                description: response.result.Description,
                api: response.result.Api,
                trigger: response.result.TriggerId,
              });
            });
          }
        });
    },
    removeScheduling(e) {
      this.loading = true;
      this.$axios
        .post(this.$urls.taskscheduling.deleteScheduling, {
          ids: this.selectedRowKeys,
        })
        .then((response) => {
          if (response.code == 0) {
            this.selectedRowKeys = [];
            this.selectedRows = [];
            this.getData();
          }
          this.loading = false;
        });
    },
    statusOperation(id, status) {
      var url = "";
      if (status == -1) url = this.$urls.taskscheduling.startScheduling;
      if (status == 0) url = this.$urls.taskscheduling.stopScheduling;
      this.loading = true;
      this.$axios.get(url + "/" + id).then((response) => {
        if (response.code == 0) {
          this.selectedRowKeys = [];
          this.selectedRows = [];
          this.getData();
        }
        this.loading = false;
      });
    },
  },
};
</script>

<style>
svg {
  cursor: pointer;
}
.disabled {
  color: #ccc;
}
</style>