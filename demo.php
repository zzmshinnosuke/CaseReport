<?php
header("Content-type:text/html;charset=utf-8");
require_once 'AipOcr.php';
require 'json_to_excel.php';

// ��� APPID AK SK
const APP_ID = '11348926';
const API_KEY = 'f5ZnerULuNzCIV9gZ4h5hXtf';
const SECRET_KEY = 'Oz0nGhsO3poXgAxcos77zNzXerIRcGDk';

$client = new AipOcr(APP_ID, API_KEY, SECRET_KEY);
//json文件是否存在,清除生成新的空白文件
$data_json_name = 'data_json.json';
if (file_exists($data_json_name)) {
//    echo "haha";
    unlink($data_json_name);
}
file_put_contents('data_json.json', '');

$handler = opendir('./tmp');
$picture_name='';
$picture_num=0;
while( ($filename = readdir($handler)) !== false )
{
    //略过linux目录的名字为'.'和‘..'的文件
    if($filename != "." && $filename != "..") {
        //输出文件名
        //echo $filename;
        //echo "<br><br>";
        $image = file_get_contents("tmp/" . $filename);
        $result = $client->basicGeneral($image);
        ////echo json_encode($result,JSON_UNESCAPED_UNICODE);
        ////echo "<br><br>";
        ////echo $result["words_result"][0]["words"];
        ////echo "<br><br>";
        $wordsnum = $result["words_result_num"];
        $record_part = 0;
        ////echo $wordsnum;
        $doc = '';
        $diagnose_words = '';//病历诊断描述

        for ($i = 0; $i < $wordsnum; $i++) {
            //按行读取数据
            $current_words = $result["words_result"][$i]["words"];
            $doc = $doc . $current_words . '<br>';

            //病历分块处理
            if ($record_part == 0) {
                $record_part = data_extraction($current_words,$picture_num);
            } else {
                $diagnose_words = $diagnose_words . $current_words;
                //diagnose_extraction($current_words);
            }
            //$new_content = str_replace($key_word, '<font color=red>'.$key_word.'</font>',$content);
        }
        diagnose_extraction($diagnose_words,$picture_num);

        $title_array = array("标本类型","病理号","病案号","门诊号","性别","年龄","科室","收到日期","诊断日期","姓名","肿瘤大小1","肿瘤大小2","肿瘤大小3","大网膜大小1","大网膜大小2","大网膜大小3","肿瘤位置","病理类型","肿瘤最大径",
            "核分裂","危险度分级","Vimentin","CD117","CD34","DOG1","SDHB","Desmin","SMA","Calponin","EMA","Myogenin","MyoD1","ALK","S-100","Ki_67");
        $json_string = file_get_contents("data_json.json");// 从文件中读取数据到PHP变量
        $data = json_decode($json_string,true);// 把JSON字符串转成PHP数组
        $title_count=count($title_array);
        for($i=0;$i<$title_count;$i++)
        {
            if(!isset( $data[$picture_num][$title_array[$i]]))
                $data[$picture_num][$title_array[$i]]="无";
        }
        $picture_name = $filename;
        $picture_num++;
    }
}
//导出excel
$json_text = file_get_contents("data_json.json");// 从文件中读取数据到PHP变量
$json_data = json_decode($json_text,true);// 把JSON字符串转成PHP数组
//$picture_name=substr($picture_name,0,strpos($picture_name,'.'));
$picture_name = date('YmdHis');
$header = array("标本类型","病理号","病案号","门诊号","性别","年龄","科室","收到日期","诊断日期","姓名","肿瘤大小1","肿瘤大小2","肿瘤大小3","大网膜大小1","大网膜大小2","大网膜大小3","肿瘤位置","病理类型","肿瘤最大径",
    "核分裂","危险度分级","Vimentin","CD117","CD34","DOG1","SDHB","Desmin","SMA","Calponin","EMA","Myogenin","MyoD1","ALK","S_100","Ki_67");
$index = array("标本类型","病理号","病案号","门诊号","性别","年龄","科室","收到日期","诊断日期","姓名","肿瘤大小1","肿瘤大小2","肿瘤大小3","大网膜大小1","大网膜大小2","大网膜大小3","肿瘤位置","病理类型","肿瘤最大径",
    "核分裂","危险度分级","Vimentin","CD117","CD34","DOG1","SDHB","Desmin","SMA","Calponin","EMA","Myogenin","MyoD1","ALK","S-100","Ki_67");
createtable($json_data,$picture_name,$header,$index);
closedir($handler);
header("Refresh:0;url=file_test.html");

//数据提取函数
function data_extraction($current_words,$picture_num)
{
    $part=0;
    $wordslen=strlen($current_words);
    $json_string = file_get_contents("data_json.json");// 从文件中读取数据到PHP变量
    $data = json_decode($json_string,true);// 把JSON字符串转成PHP数组
    ////echo $json_string;
    ////echo 'JSON_TEST:'.$data["标本类型"].'<br>';

    //标本类型？？
    $data[$picture_num]["标本类型"]="无";

    //病理号
    $pathology_startpos=strpos($current_words,"病理号");
    if($pathology_startpos!==FALSE)
    {
        $start=$pathology_startpos+strlen("病理号:");
        $pathology_len=0;
        $pathology_num_pos=$start;
        for(;$pathology_num_pos<$wordslen&&$current_words[$pathology_num_pos]>='0'&&$current_words[$pathology_num_pos]<='9';$pathology_num_pos++)
            $pathology_len++;
        $num=substr($current_words,$start,$pathology_len);
        $data[$picture_num]["病理号"]=$num;
        //echo $num.'<br>';
    }

    //病案号
    $case_startpos=strpos($current_words,"病案号");
    if($case_startpos!==FALSE)
    {
        $start=$case_startpos+strlen("病案号:");
        $case_len=0;
        $case_num_pos=$start;
        for(;$case_num_pos<$wordslen&&$current_words[$case_num_pos]>='0'&&$current_words[$case_num_pos]<='9';$case_num_pos++)
            $case_len++;
        $num=substr($current_words,$start,$case_len);
        $data[$picture_num]["病案号"]=$num;
        //echo $num.'<br>';
    }

    //门诊号？？
    $data[$picture_num]["门诊号"]="无";

    //性别
    $sex_startpos=strpos($current_words,"性别");
    if($sex_startpos!==FALSE)
    {
        $start=$sex_startpos+strlen("性别:");
        $sex=substr($current_words,$start,3);
        if($sex=='女')
            $data[$picture_num]["性别"]="f";
        else if($sex=='男')
            $data[$picture_num]["性别"]="m";
        else
            $data[$picture_num]["性别"]="无";
        //echo $data[$picture_num]["性别"].'<br>';
    }

    //年龄
    $age_startpos=strpos($current_words,"年龄");
    if($age_startpos!==FALSE)
    {
        $start=$age_startpos+strlen("年龄:");
        $age_len=0;
        $age_num_pos=$start;
        for(;$age_num_pos<$wordslen&&$current_words[$age_num_pos]>='0'&&$current_words[$age_num_pos]<='9';$age_num_pos++)
            $age_len++;
        $num=substr($current_words,$start,$age_len);
        $data[$picture_num]["年龄"]=$num;
        //echo $num.'<br>';
    }

    //科室
    $section_startpos=strpos($current_words,"科室");
    if($section_startpos!==FALSE)
    {
        if(strpos($current_words,"胰胃外科"))
            $data[$picture_num]["科室"]="1";
        else if(strpos($current_words,"结直肠外科"))
            $data[$picture_num]["科室"]="2";
        else if(strpos($current_words,"肝胆外科"))
            $data[$picture_num]["科室"]="3";
        else if(strpos($current_words,"高干病房"))
            $data[$picture_num]["科室"]="4";
        else if(strpos($current_words,"外院腔镜科"))
            $data[$picture_num]["科室"]="7";
        else if(strpos($current_words,"腔镜科"))
            $data[$picture_num]["科室"]="5";
        else if(strpos($current_words,"外院外科"))
            $data[$picture_num]["科室"]="6";
        else if(strpos($current_words,"其它"))
            $data[$picture_num]["科室"]="8";
        else
            $data[$picture_num]["科室"]="无";
        //echo $data[$picture_num]["科室"].'<br>';
    }

    //收到日期
    $received_date_startpos=strpos($current_words,"收到日期");
    if($received_date_startpos!==FALSE)
    {
        $start=$received_date_startpos+strlen("收到日期:");
        $date_len=10;
        $received_date=substr($current_words,$start,$date_len);
        $data[$picture_num]["收到日期"]=$received_date;
        //echo $received_date.'<br>';
    }

    //   "诊断日期": "无",？？？
    $data[$picture_num]["诊断日期"]="无";

    // "姓名": "无",
    $name_startpos=strpos($current_words,"姓名");
    if($name_startpos!==FALSE)
    {
        $start=$name_startpos+strlen("姓名:");
        $end=strpos($current_words,"性别");
        $name_len=$end-$start;
        $name=substr($current_words,$start,$name_len);
        $data[$picture_num]["姓名"]=$name;
        //echo $name.'<br>';
    }

    // "肿瘤大小123// "大网膜大小123
    $tumour_startpos=strpos($current_words,"大小");
    $tumour_endpos=strpos($current_words,"cm",$tumour_startpos);
    if($tumour_startpos!==FALSE)
    {
        //if($data[$picture_num]["肿瘤大小1"]=='无')
        if(!isset( $data[$picture_num]['肿瘤大小1'] ))
        {
            $size_substr= substr($current_words,$tumour_startpos,$tumour_endpos-$tumour_startpos);
            ////echo $size_substr;
            preg_match_all('/[0-9]+(.[0-9]+)?/',$size_substr,$num_arr);
            //print_r( $num_arr);
            $data[$picture_num]["肿瘤大小1"]=$num_arr[0][0];
            $data[$picture_num]["肿瘤大小2"]=$num_arr[0][1];
            $data[$picture_num]["肿瘤大小3"]=$num_arr[0][2];
        }
        else
        {
            $size_substr= substr($current_words,$tumour_startpos,$tumour_endpos-$tumour_startpos);
            ////echo $size_substr;
            preg_match_all('/[0-9]+(.[0-9]+)?/',$size_substr,$num_arr);
            //print_r( $num_arr);
            $data[$picture_num]["大网膜大小1"]=$num_arr[0][0];
            $data[$picture_num]["大网膜大小2"]=$num_arr[0][1];
            $data[$picture_num]["大网膜大小3"]=$num_arr[0][2];
        }
    }

    //拼接病理诊断后面的文字
    $otherparts_startpos=strpos($current_words,"病理诊断");
    if($otherparts_startpos!==FALSE)
        $part=1;

    $json_strings = json_encode($data,JSON_UNESCAPED_UNICODE);
    file_put_contents("data_json.json",$json_strings);//写入

    return $part;
}

//病理诊断 数据提取
function diagnose_extraction($current_words,$picture_num)
{
    $wordslen=strlen($current_words);
    $current_pos=0;
    $json_string = file_get_contents("data_json.json");// 从文件中读取数据到PHP变量
    $data = json_decode($json_string,true);// 把JSON字符串转成PHP数组

    //"肿瘤位置": "无", ？？格式规范？？
    $data[$picture_num]["肿瘤位置"]="无";
    /*
    $case_startpos=strpos($current_words,"病案号");
    if($pathology_startpos!==FALSE)
    {
        $start=$pathology_startpos+strlen("病理号:");
        $pathology_len=0;
        $pathology_num_pos=$start;
        for(;$pathology_num_pos<$wordslen&&$current_words[$pathology_num_pos]>='0'&&$current_words[$pathology_num_pos]<='9';$pathology_num_pos++)
            $pathology_len++;
        $num=substr($current_words,$start,$pathology_len);
        $data["病理号"]=$num;
        //echo $num.'<br>';
    }
    */
    //"病理类型": "无",
    $pathological_type1=strpos($current_words,"梭形细胞");
    $pathological_type2=strpos($current_words,"上皮样细胞");
    //$pathological_type3=strpos($current_words,"混合型");
    if($pathological_type1!==FALSE&&$pathological_type2!==FALSE)
    {
        $data[$picture_num]["病理类型"]=3;
    }
    else if($pathological_type1!==FALSE)
        $data[$picture_num]["病理类型"]=1;
    else if($pathological_type2!==FALSE)
        $data[$picture_num]["病理类型"]=2;
    else
        $data[$picture_num]["病理类型"]="无";

    //"肿瘤最大径": "无",
    $maxdiameter_startpos=strpos($current_words,"肿瘤最大径");
    if($maxdiameter_startpos!==FALSE)
    {
        $start=$maxdiameter_startpos+strlen("肿瘤最大径");
        $maxdiameter_endpos=strpos($current_words,"cm",$maxdiameter_startpos);
        $maxdiameter= substr($current_words,$start,$maxdiameter_endpos-$start);
        $data[$picture_num]["肿瘤最大径"]=$maxdiameter;
    }
    else
        $data[$picture_num]["肿瘤最大径"]="无";

    //"核分裂": "无",
    $core_startpos=strpos($current_words,"核分裂");
    if($maxdiameter_startpos!==FALSE)
    {
        if($current_words[$core_startpos+strlen("核分裂")]=='<')
            $data[$picture_num]["核分裂"]=1;
        else if($current_words[$core_startpos+strlen("核分裂")]=='>')
            $data[$picture_num]["核分裂"]=2;
        else
            $data[$picture_num]["核分裂"]="无";
    }

    //"危险度分级": "无",
    $danger_startpos=strpos($current_words,"危险度分级");
    if($danger_startpos!==FALSE)
    {
        $start=$danger_startpos+strlen("危险度分级:");
        $danger_endpos=strpos($current_words,"。",$maxdiameter_startpos);
        $danger= substr($current_words,$start,$danger_endpos-$start);
        //echo $danger;
        if($danger=='极低危')
            $data[$picture_num]["危险度分级"]=1;
        if($danger=='低危')
            $data[$picture_num]["危险度分级"]=2;
        if($danger=='中危')
            $data[$picture_num]["危险度分级"]=3;
        if($danger=='高危')
            $data[$picture_num]["危险度分级"]=4;
        else
            $data[$picture_num]["危险度分级"]="无";
    }

    //"Vimentin" //"CD117"  //"CD30" //"DOG1": //"SDHB" //"Desmin": //"SMA": //"Calponin":  //"EMA":  //"Myogenin":  //"MyoD1": //"ALK":  //"S_100": "无",
    $target_array=array("Vimentin","CD117","CD34","DOG1","SDHB","Desmin","SMA","Calponin","EMA","Myogenin","MyoD1","ALK","S-100");
    $target_count=count($target_array);
    for($i=0;$i<$target_count;$i++)
    {
        $target_startpos=strpos($current_words,$target_array[$i]);
        $start=$target_startpos+strlen($target_array[$i])+strlen("(");
        $target_endpos=strpos($current_words,")",$start);
        $target=substr($current_words,$start,$target_endpos-$start);
        if($target=='-')
            $data[$picture_num][$target_array[$i]]=0;
        else if($target=='1+')
            $data[$picture_num][$target_array[$i]]=1;
        else if($target=='2+')
            $data[$picture_num][$target_array[$i]]=2;
        else if($target=="3+")
            $data[$picture_num][$target_array[$i]]=3;
        else
            $data[$picture_num][$target_array[$i]]="无";
    }

    //"Ki_67": "无"
    $Ki_67_startpos=strpos($current_words,"Ki-67");
    if($Ki_67_startpos!==FALSE)
    {
        $start=$Ki_67_startpos+strlen("Ki-67(");
        $Ki_67_endpos=strpos($current_words,")",$start);
        $Ki_67=substr($current_words,$start,$Ki_67_endpos-$start);
        $data[$picture_num]["Ki_67"]=$Ki_67;
    }
    else
        $data[$picture_num]["Ki_67"]="无";

    $json_strings = json_encode($data,JSON_UNESCAPED_UNICODE);
    file_put_contents("data_json.json",$json_strings);//写入
}

//$current_num=preg_match_all('/[0-9]+(.[0-9]+)?/',$current_words,$num_arr);
////echo count($num_arr).'   ';
//print_r($num_arr);
/*
if(count($num_arr[0])>0)
{
    $num_count=count($num_arr[0]);
    for($n=0;$n<$num_count;$n++)
    {
        ////echo $num_arr[0][$n].'<br>';
        $start_pos=strpos($current_words,$num_arr[0][$n]);
        $end_pos=$start_pos+strlen($num_arr[0][$n]);
        //echo 'currentwords:'.$current_words.'  '.'num:'.$num_arr[0][$n].'  '.'position:'.$start_pos.' '.$end_pos.'<br>';
        /*
         * 数字标红
        //substr_replace($current_words,"<font color='red'>",$start_pos);//
        //substr_replace($current_words,"</font>",$end_pos);
        *
        *
        //echo $current_words.'<br>';
    }
}
*/
//if($i%5==0)
// $doc=$doc."<font color='red'>".$current_words."</font>".'<br>';
// else







