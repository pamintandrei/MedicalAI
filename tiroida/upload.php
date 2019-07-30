<?php
error_reporting(E_ERROR | E_PARSE);
if (empty($base = $_POST['base']))
    die("missing string base64");


function base64ToJpeg($base64_string) {
	
	
	
    $data = explode(';', $base64_string);
    $dataa = explode(',', $base64_string);
    $part = explode("/", $data[0]);
    if (empty($part))
        return false;
    $file = md5(uniqid(rand(), true)) . ".{$part[1]}";
    if (!is_dir(realpath(__DIR__) . "/uploads/"))
        mkdir(realpath(__DIR__) . "/uploads/");
    $ifp = fopen(realpath(__DIR__) . "/uploads/{$file}", 'wb');
    fwrite($ifp, base64_decode($dataa[1]));
    fclose($ifp);
    return $file;
}



$nr = 0;
$file_Name = '';
foreach ($base as $index => $base64) {
	if(!empty($base64))
	{
	$file_name = sprintf('%1$s/uploads/%2$s', realpath(__DIR__), base64ToJpeg($base64));
    if (!empty($base64) && !file_exists($file_name))
        die("Upload error {$finalFile} on index : {$index}");
	
	if(!empty($base64))
	{
		$nr++;
		$file_Name = $file_name;
	}
	}
}

if($nr <= 0)
{
	include 'photolink.php';
}
else
{
	if(is_numeric($_POST['disease']))
	{
	include 'photolink.php';
	include 'spinner.php';
	include 'utility/sendphoto.php';
	$file_Name = addslashes($file_Name);
	echo '<script>sendData("'. $file_Name . '",'. $_POST['disease'] .');</script>';
	
	}
}


?>