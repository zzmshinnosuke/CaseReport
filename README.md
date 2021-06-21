# CaseReport
病例报告分析，显示中的病例都是纸质打印或者手写的，需要医生手动输入到系统中，工作量很大。该项目通过调用百度ocr api接口，上传图片识别里边的文字，返回后进行格式整理为表格。  

[php版本](https://github.com/zzmshinnosuke/CaseReport-php) 属于半成品，方法类似  

# Environments
win8.1（vmware）  
vs2021  
.net4.5  

# Requirments
AipSdk.dll 百度ocr api c# sdk 接口    
Newtonsoft.Json.dll 解析json文件  
NPOI 解析xml文件，保存xls文件 
CaseFormat.xml 病例的格式，需要放到Release中才可以 

## Run
在百度云申请ocr识别 ID和key，修改相应的MyCalss/PublicData的对应参数。  
点击运行，可以设置每重病例的格式，上传图片进行识别，识别后的内容保存在excel中。  
