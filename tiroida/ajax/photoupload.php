<?php
set_time_limit(180);


error_reporting(E_ERROR | E_PARSE);
if(isset($_POST['imagePath']) && isset($_POST['disease']))
{
	$disease = get_disease($_POST['disease']);
	$result = sendAndRecvResult($_POST['imagePath'], $disease);
	echo 'Rezultatul pozei: ' . $result;
}



function get_disease($number)
{
	switch($number)
	{
		case 1:
			return "pneumonia";
			break;
		case 2:
			return "tuberculoza";
			break;
		case 3:
			return "hemoragie";
			break;
		case 4:
			return "cancersan";
			break;
		case 5:
			return "leucemie";
			break;
		case 6:
			return "malarie";
			break;
		case 7:
			return "cancerpiele";
			break;
		case 8:
			return "parkinson";
			break;
	}
	
}



function getAllData($socket)
{
	$alldata = "";
	while(true)
	{
	
		$data = fread($socket,4096);
		$alldata .= $data;
		if(substr($alldata, -5) == "<EOF>")
		{
			return substr($alldata,0,-5);
		}
		else
		{
			if($data == "")
				return -1;
		}
	
	}
	
}



function sendAndRecvResult($path_to_photo, $disease)
{
	$foo->action = $disease;
	
	$myfile = fopen($path_to_photo, "rb");
	$bytes = fread($myfile, filesize($path_to_photo));
	fclose($myfile);
	
	$foo->imageContent = base64_encode($bytes);
	$foo->cookie = "";

	
	include '../init/config.php';
	$data_to_send = json_encode($foo);
	
$server_name = "MedicalAI";
$context = stream_context_create(array('ssl' => array('allow_self_signed' => true, 'peer_name' => $server_name, 'verify_peer' => false)));


$socket = stream_socket_client("ssl://".$host.":".$port, $errno, $errstr,60,STREAM_CLIENT_CONNECT, $context);
if($socket)
{
	
	$data_to_send .= "<EOF>";
	
	stream_set_timeout($socket,140);
	
	fwrite($socket, $data_to_send);
	$recvdata = getAllData($socket);
	if($recvdata == -1)
	{
		return -1;
	}
	else
	{
		
		$decoded = json_decode($recvdata,true);
		
		return $decoded["rezultat"][0][0] * 100 . "%";
	}
	
}
else
{
	echo "Error " . $errstr;
}
	
}



?>