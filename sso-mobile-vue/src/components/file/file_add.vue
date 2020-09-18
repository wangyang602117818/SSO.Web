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
          :data-name="item.name"
        >
          <img :src="item.url" v-if="images.indexOf($funtools.getFileExtension(item.name))>-1" />
          <video
            controls="controls"
            :src="item.url"
            v-else-if="videos.indexOf($funtools.getFileExtension(item.name))>-1"
          ></video>
          <img
            :src="$axios.defaults.baseURL+$urls.file.downloadPic+'/000000000000000000000000/'+item.name"
            v-else
          />
          <div class="del" @click="delFile" :id="index">
            <f7-icon f7="xmark" color="white" size="14"></f7-icon>
          </div>
        </div>
        <div class="image_item" @click="triggerInput">
          <input
            type="file"
            ref="fileinput"
            style="width:0px;height:0px"
            multiple
            @change="choseFile"
          />
          <f7-icon f7="plus" color="gray"></f7-icon>
        </div>
      </div>
      <div class="access_wrap">
        <div class="setting_line" @click="roleShow = true">
          <div class="setting_line_name bold">{{$t("common.role")}}</div>
          <div class="setting_line_right">
            <div
              class="setting_line_data"
              v-if="roles.length>0"
            >{{roles.length+' '+$t("common.role")}}</div>
            <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
          </div>
        </div>
        <div class="setting_line" @click="userShow=true">
          <div class="setting_line_name bold">{{$t("common.user")}}</div>
          <div class="setting_line_right">
            <div
              class="setting_line_data"
              v-if="users.length>0"
            >{{users.length+' '+$t("common.user")}}</div>
            <f7-icon f7="chevron_right" color="gray" size="24"></f7-icon>
          </div>
        </div>
      </div>
      <f7-button large fill @click="uploadFiles" :disabled="buttonDisabled">{{buttonValue}}</f7-button>
    </div>
    <role-select
      :show="roleShow"
      @opened="roleOpened"
      @closed="roleClosed"
      @select="selectRoleItem"
      :selected="roles"
    />
    <user-select
      :show="userShow"
      @opened="userOpened"
      @closed="userClosed"
      @select="selectUserItem"
      :selected="users"
    />
  </f7-page>
</template>

<script>
import RoleSelectComponent from "../role-select-component";
import UserSelectComponent from "../user-select-component";
export default {
  name: "file_add",
  components: {
    "role-select": RoleSelectComponent,
    "user-select": UserSelectComponent
  },
  data() {
    return {
      imageUrls: [],
      roleShow: false,
      userShow: false,
      roles: [],
      users: [],
      buttonDisabled: false,
      buttonValue: this.$t("navigator.upload"),
      images: [],
      videos: [],
    };
  },
  created() {
    this.getFileTypeMapping();
  },
  methods: {
    previewFile() {},
    triggerInput() {
      this.$refs.fileinput.click();
    },
    roleOpened() {},
    roleClosed() {
      this.roleShow = false;
    },
    userOpened() {},
    userClosed() {
      this.userShow = false;
    },
    selectRoleItem(item) {
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
    selectUserItem(item) {
      var index = -1;
      for (var i = 0; i < this.users.length; i++) {
        if (this.users[i].UserId == item.UserId) index = i;
      }
      if (index == -1) {
        this.users.push(item);
      } else {
        this.users.splice(index, 1);
      }
    },
    delFile(e) {
      var id = e.currentTarget.id;
      this.imageUrls.splice(id, 1);
    },
    getFileTypeMapping() {
      this.$axios.get(this.$urls.file.getFileTypeMapping).then((response) => {
        if (response.code == 0) {
          for (var i = 0; i < response.result.length; i++) {
            if (response.result[i].Type == "image")
              this.images = response.result[i].Extensions;
            if (response.result[i].Type == "video")
              this.videos = response.result[i].Extensions;
          }
        }
      });
    },
    choseFile(e) {
      var files = e.target.files;
      for (var i = 0; i < files.length; i++) {
        var file = files[i];
        var img_src = URL.createObjectURL(file);
        this.imageUrls.push({
          name: file.name,
          url: img_src,
          file: file,
        });
      }
      this.$refs.fileinput.value = "";
    },
    uploadFiles(e) {
      if (this.imageUrls && this.imageUrls.length > 0) {
        var roles = [],
          users = [],
          usersDisplay = [];
        this.roles.forEach((item) => {
          roles.push(item.Name);
        });
        this.users.forEach((item) => {
          users.push(item.UserId);
          usersDisplay.push(item.UserName);
        });
        const param = new FormData();
        for (var i = 0; i < this.imageUrls.length; i++) {
          param.append("files", this.imageUrls[i].file);
        }
        if (roles.length > 0) {
          param.append("roles", JSON.stringify(roles));
        }
        if (users.length > 0) {
          param.append("users", JSON.stringify(users));
          param.append("usersDisplay", JSON.stringify(usersDisplay));
        }
        var that = this;
        this.buttonDisabled = true;
        this.$axios
          .post(this.$urls.file.uploads, param, {
            onUploadProgress: function (progressEvent) {
              var precent =
                ((progressEvent.loaded / progressEvent.total) * 100).toFixed() +
                "%";
              that.buttonValue = precent;
            },
          })
          .then((response) => {
            this.buttonDisabled = false;
            this.buttonValue = this.$t("navigator.upload");
            if (response.code == 0) {
              this.$f7router.back("", { force: true });
            } else {
              window.vue.showInfo(response.result);
            }
          });
      } else {
        window.vue.showInfo(this.$t("navigator.no_file_selected"));
      }
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

.image_item img,
.image_item video {
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
