<?php
header("Content-type:text/html;charset=utf-8");
/*
 * 判断文件存在
if(!isset($_FILES['upfile'])){
    exit('choose files');
}

if(!file_exists($_FILES['upfile']['tmp_name'])){
    exit('files not exit');
}
*/
$file_dir=dirname(__FILE__).'/tmp';
/*临时存储文件夹存在
if(!is_file($file_dir)){
    @mkdir($file_dir,0777,true);
}
 */
$file_save_path=$file_dir.'/'.$_FILES['upfile']['name'];
@rename($_FILES['upfile']['tmp_name'],$file_save_path);

if(!file_exists($file_save_path)){
    echo('上传失败');
}
else
{
    echo('上传成功');
    header("Refresh:0.5;url=file_test.html");
}
