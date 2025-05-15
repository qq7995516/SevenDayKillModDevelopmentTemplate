### 七日杀Dll类Mod开发模板
直接克隆项目到VS里面就能用了,提示缺少引用时可以重新添加一遍引用.
![image](https://github.com/user-attachments/assets/112e1d63-3b65-4fd8-a932-bb80de76656d)


编写好之后,新建一个文件夹,随便叫什么名字

新建一个文件,名称必须叫:ModInfo.xml
把以下内容修改为你的信息,再粘贴到ModInfo.xml文件里

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<xml>
    <Name value="Mod名称" />
    <DisplayName value="显示的Mod名称" />
    <Version value="1.0.0" compat="1.0.0"/>
    <Description value="简介" />
    <Author value="作者名" />
    <Website value="联系方式" />
</xml>
```
