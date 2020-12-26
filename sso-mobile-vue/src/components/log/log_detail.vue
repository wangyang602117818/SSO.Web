<template>
  <f7-page name="log_detail" ptr @ptr:refresh="getData" :infinite-preloader="loading">
    <f7-navbar :title="$t('manage.log_detail')" :back-link="$t('common.back')"></f7-navbar>
    <div class="list no-hairlines-md" v-if="log">
      <ul>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('common.from')}} :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.From"/>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('common.to')}} :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.To"/>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">Controller :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.Controller" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">Action :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.Action" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">Route :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.Route||''" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('common.time')}}(ms) :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.Time" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('common.query')}} :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.QueryString" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('common.content')}} :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.Content" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('manage.user_id')}} :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.UserId" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('manage.user_name')}} :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.UserName" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('manage.user_agent')}} :</div>
            <div class="item-input-wrap">
              <input type="text" :value="$funtools.getDeviceType(log.UserAgent)" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">Ip</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.UserHost" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('common.create_time')}} :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.CreateTime" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('common.count')+'/'+$t('common.minute')}} :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.CountPerMinute" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-title item-label">{{$t('common.exception')}} :</div>
            <div class="item-input-wrap">
              <input type="text" :value="log.Exception" />
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info"></li>
      </ul>
    </div>
    <f7-block class="text-align-center" v-else>
      <f7-preloader></f7-preloader>
    </f7-block>
  </f7-page>
</template>

<script>
export default {
  name: "log_detail",
  data() {
    return {
      log: null,
      loading: false
    };
  },
  created() {
    this.getData();
  },
  methods: {
    getData(done) {
      this.loading = true;
      var id = this.$f7route.params.id;
      this.$axios.get(this.$urls.log.detail + "/" + id).then(response => {
        this.loading = false;
        if (done) done();
        if (response.code === 0) {
          this.log = response.result;
        }
      });
    }
  }
};
</script>

<style scoped>
</style>
