<template>
  <f7-page
    name="log_manage"
    ptr
    @ptr:refresh="refresh"
    infinite
    :infinite-distance="50"
    :infinite-preloader="loading"
    @infinite="loadMore"
  >
    <f7-navbar :title="$t('manage.log_list')" :back-link="$t('common.back')">
      <f7-nav-right>
        <f7-link icon-f7="hourglass_tophalf_fill" sheet-open=".sheet-top"></f7-link>
      </f7-nav-right>
    </f7-navbar>
    <f7-sheet top backdrop class="sheet-top">
      <f7-toolbar bottom>
        <div class="left">
          <f7-link sheet-close @click="onClear">{{$t('common.clear')}}</f7-link>
        </div>
        <div class="right">
          <f7-link sheet-close @click="onSearch">{{$t('common.ok')}}</f7-link>
        </div>
      </f7-toolbar>
      <f7-page-content>
        <f7-block-title>{{$t('common.from')}}</f7-block-title>
        <f7-block strong>
          <f7-chip
            :color="item.from==fromcheck?'blue':''"
            :text="item.from"
            v-for="item in froms"
            :key="item.from"
            @click="fromClick"
          ></f7-chip>
        </f7-block>
        <f7-block-title>Controller</f7-block-title>
        <f7-block strong>
          <f7-chip
            :color="item.controller==controllercheck?'blue':''"
            :text="item.controller"
            v-for="item in controllers"
            :key="item.controller"
            @click="controllerClick"
          ></f7-chip>
        </f7-block>
        <f7-block-title>Action</f7-block-title>
        <f7-block strong>
          <f7-chip
            :color="item.action==actioncheck?'blue':''"
            :text="item.action"
            v-for="item in actions"
            :key="item.action"
            @click="actionClick"
          ></f7-chip>
        </f7-block>
        <f7-block-title>{{$t('common.time')}}</f7-block-title>
        <f7-block strong>
          <f7-chip
            :color="day==30?'blue':''"
            :text="$t('manage.one_month')"
            data-day="30"
            @click="timeClick"
          ></f7-chip>
          <f7-chip
            :color="day==90?'blue':''"
            :text="$t('manage.three_month')"
            data-day="90"
            @click="timeClick"
          ></f7-chip>
          <f7-chip
            :color="day==180?'blue':''"
            :text="$t('manage.six_month')"
            data-day="180"
            @click="timeClick"
          ></f7-chip>
          <f7-chip
            :color="day==360?'blue':''"
            :text="$t('manage.one_year')"
            data-day="360"
            @click="timeClick"
          ></f7-chip>
        </f7-block>
      </f7-page-content>
    </f7-sheet>
    <f7-searchbar
      disable-button-text
      :placeholder="$t('common.search')"
      :clear-button="true"
      @change="onSearch"
    ></f7-searchbar>
    <f7-list media-list>
      <f7-list-item
        v-for="item in datas"
        :title="item.Controller+'/'+item.Action"
        :subtitle="item.CreateTime"
        :key="item._id"
        :after="item.UserName"
        :link="'/logdetail/'+item._id"
      ></f7-list-item>
    </f7-list>
    <f7-block class="text-align-center" v-if="datas.length===0&&isEnd">{{$t('common.no_data')}}</f7-block>
    <f7-block class="text-align-center" v-if="datas.length>0&&isEnd">---{{$t('common.end')}}---</f7-block>
  </f7-page>
</template>

<script>
import ListBase from "../ListBase";
export default {
  name: "log_manage",
  mixins: [ListBase],
  data() {
    return {
      getlist: this.$urls.log.getlistsimple,
      fromcheck: "",
      froms: [],
      controllercheck: "",
      controllers: [],
      actioncheck: "",
      actions: [],
      day: 30
    };
  },
  created() {
    this.getFroms();
  },
  methods: {
    onClear() {
      this.fromcheck = "";
      this.controllercheck = "";
      this.actioncheck = "";
      this.day = 30;
      this.onSearch();
    },
    getQuerystring() {
      var url = "?pageIndex=" + this.pageIndex + "&pageSize=" + this.pageSize;
      if (this.fromcheck) url += "&from=" + this.fromcheck;
      if (this.controllercheck)
        url += "&controllerName=" + this.controllercheck;
      if (this.actioncheck) url += "&actionName=" + this.actioncheck;
      if (this.filter) url += "&userName=" + this.filter;
      if (this.day)
        url +=
          "&startTime=" +
          this.$funtools.dateAddDays(new Date(), -this.day, "yyyy-MM-dd");
      return url;
    },
    getFroms() {
      this.$axios.get(this.$urls.log.getfromlist).then(response => {
        if (response.code === 0) this.froms = response.result;
      });
    },
    getControllers(from) {
      this.$axios
        .get(this.$urls.log.getControllersByFrom + "?from=" + from)
        .then(response => {
          if (response.code === 0) this.controllers = response.result;
        });
    },
    getActions(from, controller) {
      this.$axios
        .get(
          this.$urls.log.getActionsByController +
            "?from=" +
            from +
            "&controllerName=" +
            controller
        )
        .then(response => {
          if (response.code === 0) this.actions = response.result;
        });
    },
    fromClick($event) {
      var from = $event.currentTarget.innerText;
      if (this.fromcheck == from) {
        this.fromcheck = "";
        return;
      }
      this.fromcheck = from;
      this.actions = [];
      this.getControllers(from);
    },
    controllerClick($event) {
      var controller = $event.currentTarget.innerText;
      if (this.controllercheck == controller) {
        this.controllercheck = "";
        return;
      }
      this.controllercheck = controller;
      this.actions = [];
      this.getActions(this.fromcheck, controller);
    },
    actionClick($event) {
      var action = $event.currentTarget.innerText;
      if (this.actioncheck == action) {
        this.actioncheck = "";
        return;
      }
      this.actioncheck = action;
    },
    timeClick($event) {
      var day = $event.currentTarget.dataset.day;
      this.day = day;
    }
  }
};
</script>

<style scoped>
.block-title {
  margin-top: 5px;
}
.sheet-modal {
  min-height: 50%;
}
.chip {
  margin-left: 5px;
}
</style>
