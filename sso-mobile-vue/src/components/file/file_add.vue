<template>
  <f7-page name="file_add">
    <f7-navbar :title="$t('navigator.add_file')" :back-link="$t('common.back')"></f7-navbar>
    <div class="image_container">
      <div class="image_list">
        <div
          class="image_item"
          v-for="(item,index) in imageUrls"
          :key="index"
          @click="previewFile"
          :data-url="item"
        >
          <img :src="item" />
          <div class="del">
            <f7-icon f7="xmark" color="white" size="14"></f7-icon>
          </div>
        </div>
        <div class="image_item" @click="chooseImage">
          <input
            type="file"
            ref="fileinput"
            style="width:0px;height:0px"
            multiple
            @change="uploadFile"
          />
          <f7-icon f7="plus" color="gray"></f7-icon>
        </div>
      </div>
      <div class="access_wrap">
        <div class="setting_line" @click="addRole">
          <div class="setting_line_name bold">{{$t("common.role")}}</div>
          <div class="setting_line_data">
            <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
          </div>
        </div>
        <div class="setting_line">
          <div class="setting_line_name">{{$t("common.user")}}</div>
          <div class="setting_line_right">
            <div class="setting_line_data"></div>
            <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
          </div>
        </div>
      </div>
      <f7-button large fill>{{$t("common.add")}}</f7-button>
    </div>
    <role-select
      :show="roleShow"
      @opened="accessOpened"
      @closed="accessClosed"
      @select="selectItem"
      :selected="roles"
    />
  </f7-page>
</template>

<script>
import RoleSelectComponent from "../role-select-component";
export default {
  name: "file_add",
  components: {
    "role-select": RoleSelectComponent,
  },
  data() {
    return {
      imageUrls: [],
      roleShow: false,
      roles: [],
      users: [],
    };
  },
  created() {},
  methods: {
    previewFile() {},
    chooseImage() {
      this.$refs.fileinput.click();
    },
    accessOpened() {
      window.console.log("x");
    },
    accessClosed() {
      this.roleShow = false;
    },
    addRole() {
      this.roleShow = true;
    },
    selectItem(item) {
      var index = -1;
      for (var i = 0; i < this.roles.length; i++) {
        if (this.roles[i].Name == item.Name) index = i;
      }
      if (index == -1) {
        this.roles.push(item);
      } else {
        this.roles.splice(index, 1);
      }
    },
    uploadFile(e) {
      var files = e.target.files;
      for (var i = 0; i < files.length; i++) {
        var file = files[i];
        var img_src = URL.createObjectURL(file);
        this.imageUrls.push(img_src);
      }
      this.$refs.fileinput.value = "";
    },
  },
};
</script>

<style scoped>
.image_container {
  width: 95%;
  display: flex;
  flex-direction: column;
  align-items: center;
  margin: 0 auto;
}
.image_list {
  width: 100%;
  float: left;
}

.image_item {
  display: inline-flex;
  height: 100px;
  width: 100px;
  margin-right: 10px;
  margin-top: 10px;
  overflow: hidden;
  border: 1px solid #ccc;
  align-items: center;
  justify-content: center;
  vertical-align: top;
  position: relative;
}

.image_item img {
  width: 100%;
  height: 100%;
}
.del {
  position: absolute;
  right: 0;
  top: 0;
  width: 18px;
  height: 18px;
  background-color: rgba(0, 0, 0, 0.7);
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
}
.del:hover {
  background-color: rgba(65, 65, 65, 0.6);
}
.convert_wrap,
.access_wrap {
  width: 100%;
  margin-top: 40px;
}
.button {
  width: 100%;
  margin-top: 40px;
}
</style>
