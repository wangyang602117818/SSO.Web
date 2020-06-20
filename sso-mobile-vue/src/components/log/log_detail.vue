<template>
  <f7-page name="log_detail" ptr @ptr:refresh="getData" :infinite-preloader="loading">
    <f7-navbar :title="$t('manage.log_detail')" :back-link="$t('common.back')"></f7-navbar>
    <div class="list no-hairlines-md" v-if="log">
      <ul>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="log.From" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">{{$t('common.from')}}</div>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="log.Controller" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">Controller</div>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="log.Action" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">Action</div>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="log.Route||''" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">Route</div>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="log.QueryString" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">{{$t('common.query')}}</div>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="log.Content" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">{{$t('common.content')}}</div>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="log.UserId" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">{{$t('manage.user_id')}}</div>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="log.UserName" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">{{$t('manage.user_name')}}</div>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="$funtools.getDeviceType(log.UserAgent)" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">{{$t('manage.user_agent')}}</div>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="log.UserHost" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">Ip</div>
            </div>
          </div>
        </li>
        <li class="item-content item-input item-input-with-info">
          <div class="item-inner">
            <div class="item-input-wrap">
              <input type="text" :value="log.CreateTime" />
              <span class="input-clear-button"></span>
              <div class="item-input-info">{{$t('common.create_time')}}</div>
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
