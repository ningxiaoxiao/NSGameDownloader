# NSGameDownloader
![](https://img.shields.io/github/downloads/freedom10086/NSGameDownloader/total.svg?label=%E7%82%B9%E5%87%BB%E4%B8%8B%E8%BD%BD&style=flat)  ![](https://img.shields.io/github/release/freedom10086/NSGameDownloader.svg?style=flat&label=%E6%9C%80%E6%96%B0%E7%89%88%E6%9C%AC)  

从百度云查找NS游戏  
这个软件并不能直接从百度云下载.只是能简化查找游戏的过程

![](https://raw.githubusercontent.com/freedom10086/NSGameDownloader/master/screenshot.png)

### 基本使用

- 输入游戏名，游戏id查找游戏
- 双击游戏列表单元格复制查找结果
- 可自行更新游戏数据库
- 可过滤是否有中文支持
- 点击游戏图片可以访问eshop网页

### 配置

配置文件 `config.json`

```json
{
  "panUrlXci": "riggzh分享的百度云资源XCI游戏地址",
  "panUrlNsp": "riggzh分享的百度云资源NSP游戏地址",
  "panUrlUpd": "riggzh分享的百度云资源更新和DLC地址",
  "nutDbUrl": "nutDb地址",
  "excelDbUrl": "riggzh整理的excel表格地址",
  "localGameDir": "本地下载的游戏资源目录"
}
```

`localGameDir`  为本地下载的游戏父目录可以为空，设置此的话可以查看已下载游戏目录，目录和[riggzh分享的百度云盘](https://www.91wii.com/thread-116074-1-1.html)目录结构相同，子目录为游戏id前五位\游戏完整id，如`\01000\0100000000010000`，将下载的游戏本体，dlc，更新都放在此目录即可。



> 特别感谢@91wil.riggzh
