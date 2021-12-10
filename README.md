# CaseReport-php
病例报告分析，显示中的病例都是纸质打印或者手写的，需要医生手动输入到系统中，工作量很大。该项目通过调用百度ocr api接口，上传图片识别里边的文字，返回后进行格式整理为表格。  
[wpf版本](https://github.com/zzmshinnosuke/CaseReport) 

安装xampp  目前设置为8080端口，xampp\apache\conf\httpd.conf可配置  启动apache 
代码文件放置在 xampp\htdocs 目录下


访问 http://localhost:8080/image_recognize/file_test.html 

demo.php 调用百度的api进行图像识别，对返回的json文本进行处理存为需要的格式化json存储

json_to_excel.php 将json生成Excel文件  目前只能用excel打开，spss打开有问题，需要生成能用spss打开的Excel文件


