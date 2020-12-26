---
title: "R"
output: html_document
toc: yes
runtime: shiny
---
# OCR api列表
## 一 . 阿里系OCR
- 账号(xuyufang11 1qaz@WSX3edc_1)
  ```
    APP_Code: fc44569099d947d48b0b234fcc212474  
	API_KEY: 203786946  
	SECRET_KEY: n3z62e5eqkihv61vuvsfdq112ys2um6v
  ```
- 公司付费版子账号(zhushixin@50100887.onaliyun.com zhusx@xspeedtech.cn)
   ```
    APP_Code: c734b1a78f1143b6b79a323ce0a60d69
	API_KEY: 203836244
	SECRET_KEY: ZDpFshtmKrAOFL9c9OVsVQZ2PhcomVc8
  ```
### 阿里云:
    url: https://tysbgpu.market.alicloudapi.com/api/predict/ocr_general
    请求: 把图片的base64放在请求体或者指定url
    验证(header): Authorization : "APPCODE c734b1a78f1143b6b79a323ce0a60d69"
### 淘宝:
    url: https://ocrapi-advanced.taobao.com/ocrservice/advanced
	请求: 把图片的base64放在请求体或者指定url
	验证(header): Authorization : "APPCODE c734b1a78f1143b6b79a323ce0a60d69"
### 淘宝结构化识别:
    url: https://ocrapi-document-structure.taobao.com/ocrservice/documentStructure
	请求: 把图片的base64放在请求体或者指定url
	验证(header): Authorization : "APPCODE c734b1a78f1143b6b79a323ce0a60d69"
## 二 . 腾讯云OCR
    url: https://ocr.tencentcloudapi.com
    appId: AKIDhBQP8copDLErEqqMGyQcBVoshHelwGeq
	appKey: 5vWu3wa0EDwQhjV4PXP1OZVBcnlTv8b4
    请求: 图片的url放在请求体
    验证: 需要签名(Signature)
## 三 . 百度云OCR
- 账号(18738188107 xuyufang521)
  ```
  APP_ID: 18524358
  API_KEY: no2lUtswfGmHQvcrs5deo9nB
  SECRET_KEY: AHP6UaUPiZhYl1pT9f9R3fuAKDsZ4VBx
  ```
### 高精度版图片识别:
	url(已被封装到dll)
	把图片的字节流传入api	
	验证: 已被百度封装好
### 票据识别:
    url(已被封装到dll)
	把图片的字节流传入api	
	验证: 已被百度封装好
### 百度文字识别:
    APP_ID: 14941135
	API_KEY: DjuwFMyrfvohrXIpBPbvIlUm
	SECRET_KEY: BlyWTyEXe7N3q0pBUh145SkiSGIyDROI