<?php
/**
 * Created by PhpStorm.
 * User: xiaoshang
 * Date: 2018/6/22
 * Time: 15:39
 */

//导出excel
/*
$picture_name="病历.jpg";
$json_text = file_get_contents("data_json.json");// 从文件中读取数据到PHP变量
$json_data = json_decode($json_text,true);// 把JSON字符串转成PHP数组
$picture_name=substr($picture_name,0,strpos($picture_name,'.'));
$picture_name = $picture_name.date('YmdHis');
echo $picture_name;
$header = array("标本类型","病理号","病案号","门诊号","性别","年龄","科室","收到日期","诊断日期","姓名","肿瘤大小1","肿瘤大小2","肿瘤大小3","大网膜大小1","大网膜大小2","大网膜大小3","肿瘤位置","病理类型","肿瘤最大径",
    "核分裂","危险度分级","Vimentin","CD117","CD34","DOG1","SDHB","Desmin","SMA","Calponin","EMA","Myogenin","MyoD1","ALK","S-100","Ki_67");
$index = array("标本类型","病理号","病案号","门诊号","性别","年龄","科室","收到日期","诊断日期","姓名","肿瘤大小1","肿瘤大小2","肿瘤大小3","大网膜大小1","大网膜大小2","大网膜大小3","肿瘤位置","病理类型","肿瘤最大径",
    "核分裂","危险度分级","Vimentin","CD117","CD34","DOG1","SDHB","Desmin","SMA","Calponin","EMA","Myogenin","MyoD1","ALK","S-100","Ki_67");
createtable($json_data,$picture_name,$header,$index);

$json_text = file_get_contents("jsontest.json");// 从文件中读取数据到PHP变量
$json_data = json_decode($json_text,true);// 把JSON字符串转成PHP数组
$header = array("名称","网址");
$index = array("name","url");
createtable($json_data,"123",$header,$index);

$json_text = file_get_contents("data_json.json");// 从文件中读取数据到PHP变量
$json_data = json_decode($json_text,true);// 把JSON字符串转成PHP数组
$excel_name=date('YmdHis');
$header = array("标本类型","病理号","病案号","门诊号","性别","年龄","科室","收到日期","诊断日期","姓名","肿瘤大小1","肿瘤大小2","肿瘤大小3","大网膜大小1","大网膜大小2","大网膜大小3","肿瘤位置","病理类型","肿瘤最大径",
    "核分裂","危险度分级","Vimentin","CD117","CD34","DOG1","SDHB","Desmin","SMA","Calponin","EMA","Myogenin","MyoD1","ALK","S_100","Ki_67");
$index = array("标本类型","病理号","病案号","门诊号","性别","年龄","科室","收到日期","诊断日期","姓名","肿瘤大小1","肿瘤大小2","肿瘤大小3","大网膜大小1","大网膜大小2","大网膜大小3","肿瘤位置","病理类型","肿瘤最大径",
    "核分裂","危险度分级","Vimentin","CD117","CD34","DOG1","SDHB","Desmin","SMA","Calponin","EMA","Myogenin","MyoD1","ALK","S-100","Ki_67");
createtable($json_data,"123",$header,$index);
*/
function createtable($list,$filename,$header=array(),$index = array()){
    ob_end_clean();
    ob_start();
    header("Content-type:application/vnd.ms-excel");
    header("Content-Disposition:attachment;filename=".$filename.".xls");
    header('Cache-Control: max-age=0');
    $teble_header = implode("\t",$header);
    $strexport = $teble_header."\r";
    foreach ($list as $row){
        foreach($index as $val){
            $strexport.=$row[$val]."\t";
        }
        $strexport.="\r";
    }
    $strexport=iconv('UTF-8',"GB2312//IGNORE",$strexport);
    exit($strexport);
}