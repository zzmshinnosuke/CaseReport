<?php
$handler = opendir('./tmp');
$filename_list='';
while( ($filename = readdir($handler)) !== false ) {
    //略过linux目录的名字为'.'和‘..'的文件
    if ($filename != "." && $filename != "..") {
        //输出文件名
        $filename_list=$filename_list.'<br>'.$filename;
    }
}
echo "document.write('$filename_list');\n";
?>