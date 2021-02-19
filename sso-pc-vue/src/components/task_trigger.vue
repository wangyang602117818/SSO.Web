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
        <a-button
          type="primary"
          icon="plus"
          :title="$t('add')"
          @click="showDrawer()"
        ></a-button>
        <a-button
          type="default"
          icon="redo"
          @click="reload"
          :title="$t('refresh')"
        ></a-button>
        <a-button
          type="default"
          icon="edit"
          @click="editTrigger"
          :disabled="selectedRowKeys.length != 1"
          :title="$t('edit')"
        ></a-button>
        <a-popconfirm
          :title="$t('confirm_delete')"
          @confirm="removeTrigger"
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
      @change="handleTableChange"
    />
    <a-drawer
      :title="isUpdate ? $t('update_trigger') : $t('add_trigger')"
      :width="660"
      @close="drawerVisible = false"
      :visible="drawerVisible"
    >
      <a-form :form="form" @submit.prevent="handleSubmit">
        <a-form-item
          :label="$t('start_up_time')"
          :label-col="{ span: 3 }"
          :wrapper-col="{ span: 21 }"
        >
          <input type="date" class="date_input" v-model="currentDate" />
          <input type="time" class="time_input" v-model="currentTime" />
          <a-tooltip placement="topLeft" :title="$t('trigger_start_tip')">
            <a-icon type="question-circle" style="margin-left: 10px" />
          </a-tooltip>
        </a-form-item>
        <a-form-item
          :label="$t('settings')"
          :label-col="{ span: 3 }"
          :wrapper-col="{ span: 21 }"
        >
          <a-radio-group v-model="triggerType">
            <a-radio :value="1">{{ $t("second") }}</a-radio>
            <a-radio :value="2">{{ $t("minute") }}</a-radio>
            <a-radio :value="3">{{ $t("hour") }}</a-radio>
            <a-radio :value="4">{{ $t("day") }}</a-radio>
            <a-radio :value="5">{{ $t("week") }}</a-radio>
            <a-radio :value="6">{{ $t("month") }}</a-radio>
            <a-radio :value="7">{{ $t("year") }}</a-radio>
          </a-radio-group>
          <div v-if="triggerType == 1">
            <a-radio-group v-model="secondType">
              <a-radio :value="1" @click="seconds = []">{{
                $t("not_set")
              }}</a-radio>
              <a-radio :value="2">{{ $t("settings") }}</a-radio>
            </a-radio-group>
            <a-checkbox-group v-model="seconds" v-if="secondType == 2">
              <a-checkbox :value="0">{{ $funtools.formatMonth(0) }}</a-checkbox>
              <a-checkbox :value="i" v-for="i in 59" :key="i">{{
                $funtools.formatMonth(i)
              }}</a-checkbox>
            </a-checkbox-group>
          </div>
          <div v-if="triggerType == 2">
            <a-radio-group v-model="minuteType">
              <a-radio :value="1" @click="minutes = []">{{
                $t("not_set")
              }}</a-radio>
              <a-radio :value="2">{{ $t("settings") }}</a-radio>
            </a-radio-group>
            <a-checkbox-group v-model="minutes" v-if="minuteType == 2">
              <a-checkbox :value="0">{{ $funtools.formatMonth(0) }}</a-checkbox>
              <a-checkbox :value="i" v-for="i in 59" :key="i">{{
                $funtools.formatMonth(i)
              }}</a-checkbox>
            </a-checkbox-group>
          </div>
          <div v-if="triggerType == 3">
            <a-radio-group v-model="hourType">
              <a-radio :value="1" @click="hour = []">{{
                $t("not_set")
              }}</a-radio>
              <a-radio :value="2">{{ $t("settings") }}</a-radio>
            </a-radio-group>
            <a-checkbox-group v-model="hour" v-if="hourType == 2">
              <a-checkbox :value="0">{{ $funtools.formatMonth(0) }}</a-checkbox>
              <a-checkbox :value="i" v-for="i in 23" :key="i">{{
                $funtools.formatMonth(i)
              }}</a-checkbox>
            </a-checkbox-group>
          </div>
          <div v-if="triggerType == 4">
            <a-radio-group v-model="dayType">
              <a-radio :value="1" @click="day = []">{{
                $t("not_set")
              }}</a-radio>
              <a-radio :value="2" @click="day = []">{{
                $t("settings")
              }}</a-radio>
              <a-radio :value="3" @click="day = ['L']">{{
                $t("last_day_of_month")
              }}</a-radio>
            </a-radio-group>
            <a-checkbox-group v-model="day" v-if="dayType == 2">
              <a-checkbox :value="i" v-for="i in 31" :key="i">{{
                $funtools.formatMonth(i)
              }}</a-checkbox>
            </a-checkbox-group>
          </div>
          <div v-if="triggerType == 5">
            <a-radio-group v-model="weekType">
              <a-radio :value="1" @click="week = []">{{
                $t("not_set")
              }}</a-radio>
              <a-radio :value="2">{{ $t("settings") }}</a-radio>
            </a-radio-group>
            <a-checkbox-group v-model="week" v-if="weekType == 2">
              <a-checkbox :value="2">{{ $t("monday") }}</a-checkbox>
              <a-checkbox :value="3">{{ $t("tuesday") }}</a-checkbox>
              <a-checkbox :value="4">{{ $t("wednesday") }}</a-checkbox>
              <a-checkbox :value="5">{{ $t("thursday") }}</a-checkbox>
              <a-checkbox :value="6">{{ $t("friday") }}</a-checkbox>
              <a-checkbox :value="7">{{ $t("saturday") }}</a-checkbox>
              <a-checkbox :value="1">{{ $t("sunday") }}</a-checkbox>
            </a-checkbox-group>
          </div>
          <div v-if="triggerType == 6">
            <a-radio-group v-model="monthType">
              <a-radio :value="1" @click="month = []">{{
                $t("not_set")
              }}</a-radio>
              <a-radio :value="2">{{ $t("settings") }}</a-radio>
            </a-radio-group>
            <a-checkbox-group v-model="month" v-if="monthType == 2">
              <a-checkbox :value="i" v-for="i in 12" :key="i">{{
                $funtools.formatMonth(i)
              }}</a-checkbox>
            </a-checkbox-group>
          </div>
          <div v-if="triggerType == 7">
            <a-radio-group v-model="yearType">
              <a-radio :value="1" @click="year = []">{{
                $t("not_set")
              }}</a-radio>
              <a-radio :value="2">{{ $t("settings") }}</a-radio>
            </a-radio-group>
            <a-checkbox-group v-model="year" v-if="yearType == 2">
              <a-checkbox :value="item" v-for="item in getYears" :key="item">{{
                item
              }}</a-checkbox>
            </a-checkbox-group>
          </div>
        </a-form-item>
        <a-form-item
          :label="$t('end_time')"
          :label-col="{ span: 3 }"
          :wrapper-col="{ span: 21 }"
        >
          <input type="date" class="date_input" v-model="endDate" />
          <input type="time" class="time_input" v-model="endTime" />

          <a-tooltip placement="topLeft" :title="$t('trigger_end_tip')">
            <a-icon type="question-circle" style="margin-left: 10px" />
          </a-tooltip>
        </a-form-item>
        <a-form-item
          :label="$t('crons')"
          :label-col="{ span: 3 }"
          :wrapper-col="{ span: 21 }"
        >
          {{ getCrons }}
        </a-form-item>
        <a-form-item
          :label="$t('description')"
          :label-col="{ span: 3 }"
          :wrapper-col="{ span: 21 }"
        >
          {{ $i18n.locale == "zh-cn" ? descriptions[1] : descriptions[0] }}
          {{ $t("execute") }}
        </a-form-item>
        <a-form-item
          
          :label-col="{ span: 3 }"
          :wrapper-col="{ span: 21 }"
        >
          <span slot="label">
            {{$t('example')}}
              <a-tooltip placement="topLeft" :title="$t('recent_execution_datetimes')">
            <a-icon type="question-circle" />
          </a-tooltip>
          </span>
          <div>
            <a-row>
              <a-col :span="8" v-for="(item, index) in examples" :key="index">
                <div>({{ index + 1 }}):{{ item }}</div>
              </a-col>
            </a-row>
          </div>
        </a-form-item>
        <a-divider />
        <a-form-item
          :label="$t('')"
          :label-col="{ span: 3 }"
          :wrapper-col="{ span: 21 }"
        >
          <a-button type="primary" html-type="submit">{{
            $t("submit")
          }}</a-button>
        </a-form-item>
      </a-form>
    </a-drawer>
  </div>
</template>

<script>
import base from "./Base";
export default {
  name: "task_trigger",
  mixins: [base],
  data() {
    return {
      getlist: this.$urls.taskscheduling.gettriggerlist,
      triggerType: 0,
      secondType: 1,
      minuteType: 1,
      hourType: 1,
      dayType: 1,
      weekType: 1,
      monthType: 1,
      yearType: 1,
      seconds: [],
      minutes: [],
      hour: [],
      day: [],
      week: [],
      month: [],
      year: [],
      showDelete: false,
      currentDate: this.getCurrentDate(),
      currentTime: this.getCurrentTime(),
      endDate: "",
      endTime: "",
      examples: [],
      descriptions: [],
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
          title: this.$t("description"),
          dataIndex:
            this.$i18n.locale == "zh-cn" ? "Description" : "Description1",
          width: "25%",
          ellipsis: true,
        },
        {
          title: this.$t("crons"),
          dataIndex: "Crons",
          width: "18%",
        },
        {
          title: this.$t("activate"),
          dataIndex: "Activate",
          width: "13%",
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        },
        {
          title: this.$t("expire"),
          dataIndex: "Expire",
          width: "13%",
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        },
        {
          title: this.$t("update_time"),
          dataIndex: "UpdateTime",
          width: "13%",
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        },
        {
          title: this.$t("create_time"),
          dataIndex: "CreateTime",
          width: "13%",
          customRender: (val) => {
            return this.$funtools.parseIsoDateTime(val);
          },
        },
      ];
    },
    getCrons: function () {
      var crons = "* * * * * ? *";
      var crons_arr = crons.split(" ");
      if (this.seconds.length > 0) crons_arr[0] = this.seconds.join();
      if (this.minutes.length > 0) crons_arr[1] = this.minutes.join();
      if (this.hour.length > 0) crons_arr[2] = this.hour.join();
      if (this.day.length > 0) crons_arr[3] = this.day.join();
      if (this.week.length > 0) {
        crons_arr[5] = this.week.join();
        crons_arr[3] = "?";
      }
      if (this.month.length > 0) crons_arr[4] = this.month.join();
      if (this.year.length > 0) crons_arr[6] = this.year.join();
      return crons_arr.join(" ");
    },
    getYears: function () {
      var years = [];
      var start = this.currentDate.split("-")[0];
      for (var i = start; i <= 2099; i++) years.push(i);
      return years;
    },
    getStartEndTime: function () {
      return (
        this.currentDate +
        " " +
        this.currentTime +
        "," +
        this.endDate +
        " " +
        this.endTime
      );
    },
  },
  watch: {
    getCrons(val) {
      this.getExampleTimes();
    },
    getStartEndTime(val) {
      this.getExampleTimes();
    },
  },
  created() {},
  mounted() {},
  methods: {
    parseCrons: function (crons) {
      var crons_arr = crons.split(" ");
      this.triggerType = 0;
      if (crons_arr[0] == "*") {
        this.secondType = 1;
        this.seconds = [];
      } else {
        this.secondType = 2;
        this.seconds = crons_arr[0].split(",").map(Number);
      }
      if (crons_arr[1] == "*") {
        this.minuteType = 1;
        this.minutes = [];
      } else {
        this.minuteType = 2;
        this.minutes = crons_arr[1].split(",").map(Number);
      }
      if (crons_arr[2] == "*") {
        this.hourType = 1;
        this.hour = [];
      } else {
        this.hourType = 2;
        this.hour = crons_arr[2].split(",").map(Number);
      }
      if (crons_arr[3] == "*" || crons_arr[3] == "?") {
        this.dayType = 1;
        this.day = [];
      } else {
        if (crons_arr[3] == "L") {
          this.dayType = 3;
        } else {
          this.dayType = 2;
          this.day = crons_arr[3].split(",").map(Number);
        }
      }
      if (crons_arr[5] == "*" || crons_arr[5] == "?") {
        this.weekType = 1;
        this.week = [];
      } else {
        this.weekType = 2;
        this.week = crons_arr[5].split(",").map(Number);
      }
      if (crons_arr[4] == "*") {
        this.monthType = 1;
        this.month = [];
      } else {
        this.monthType = 2;
        this.month = crons_arr[4].split(",").map(Number);
      }
      if (crons_arr[6] == "*") {
        this.yearType = 1;
        this.year = [];
      } else {
        this.yearType = 2;
        this.year = crons_arr[6].split(",").map(Number);
      }
    },
    getQuerystring() {
      var url =
        "?pageIndex=" +
        this.pagination.current +
        "&pageSize=" +
        this.pagination.pageSize;
      if (this.searchValue) url += "&searchValue=" + this.searchValue;
      return url;
    },
    getCurrentDate() {
      return this.$funtools.getCurrentDateTime().split(" ")[0];
    },
    getCurrentTime() {
      var time = this.$funtools.getCurrentDateTime().split(" ")[1].split(":");
      return time[0] + ":" + time[1];
    },
    getExampleTimes() {
      var start = this.currentDate + " " + this.currentTime;
      var end = this.$funtools.trimStartChar(this.endDate + " " + this.endTime);
      var crons = this.getCrons;
      var url =
        this.$urls.taskscheduling.getExamples +
        "?start=" +
        start +
        "&crons=" +
        crons;
      if (end) url += "&end=" + end;
      this.$axios.get(url).then((response) => {
        if (response.code == 0) {
          this.examples = response.result.Examples;
          this.descriptions = response.result.CronsDescriptions;
        }
      });
    },
    showDrawer(id) {
      if (id) {
        this.isUpdate = true;
      } else {
        this.isUpdate = false;
        this.currentDate = this.getCurrentDate();
        this.currentTime = this.getCurrentTime();
      }
      this.drawerVisible = true;
      this.getExampleTimes();
    },
    editTrigger(e) {
      this.$axios
        .get(
          this.$urls.taskscheduling.getTriggerById +
            "/" +
            this.selectedRowKeys[0]
        )
        .then((response) => {
          if (response.code == 0) {
            this.showDrawer(response.result.Id);
            this.parseCrons(response.result.Crons);
            this.currentDate = response.result.Activate.split(" ")[0];
            this.currentTime = response.result.Activate.split(" ")[1];
            if (response.result.Expire) {
              this.endDate = response.result.Expire.split(" ")[0];
              this.endTime = response.result.Expire.split(" ")[1];
            }
          }
        });
    },
    removeTrigger(e) {
      this.loading = true;
      this.$axios
        .post(this.$urls.taskscheduling.deleteTrigger, {
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
    handleSubmit() {
      this.form.validateFields((err, values) => {
        if (!err) {
          if (this.isUpdate) this.updateTrigger(values);
          if (!this.isUpdate) this.addTrigger(values);
        }
      });
    },
    updateTrigger(values) {
      var obj = {
        id: this.selectedRows[0].Id,
        activate: this.currentDate + " " + this.currentTime,
        crons: this.getCrons,
      };
      if (this.endDate && this.endTime)
        obj.expire = this.endDate + " " + this.endTime;
      this.$axios
        .post(this.$urls.taskscheduling.updateTrigger, obj)
        .then((response) => {
          if (response.code == 0) {
            this.getData();
            this.$message.warning(this.$t("modify_success"));
          }
        });
    },
    addTrigger(values) {
      var obj = {
        activate: this.currentDate + " " + this.currentTime,
        crons: this.getCrons,
      };
      if (this.endDate && this.endTime)
        obj.expire = this.endDate + " " + this.endTime;
      this.$axios
        .post(this.$urls.taskscheduling.addTrigger, obj)
        .then((response) => {
          if (response.code == 0) {
            this.getData();
          }
        });
    },
  },
};
</script>

<style>
.date_input,
.time_input {
  height: 30px;
  overflow: hidden;
  border: 1px solid #e8e8e8;
}
.time_input {
  margin-left: 10px;
}
span.ant-radio + * {
  padding-right: 4px;
  padding-left: 4px;
}
.ant-checkbox + span {
  padding-right: 10px;
  padding-left: 5px;
}
</style>