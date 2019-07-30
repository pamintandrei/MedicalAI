<?php
error_reporting(0);
set_time_limit(180);
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


$data_check = array("Sex", "Age", "on_thyroxine", "query_on_thyroxine", "on_antithyroid_medication", "thyroid_surgery","query_hypothyroid", "query_hyperthyroid", "pregnant", "sick", "tumor", "lithium", "goitre", "TSH_measured","TSH","T3_measured","T3","TT4_measured", "TT4","FTI_measured","FTI","TBG_measured","TBG");
$verify = true;
foreach($data_check as $key)
{
	if(!isset($_POST[$key]))
	{
		$verify = false; 
	}
}

if($verify)
{
$medai->Sex = $_POST['Sex'];
$medai->Age = $_POST['Age'];
$medai->on_thyroxine = $_POST['on_thyroxine'];
$medai->query_on_thyroxine = $_POST['query_on_thyroxine'];
$medai->on_antithyroid_medication = $_POST['on_antithyroid_medication'];
$medai->thyroid_surgery = $_POST['thyroid_surgery'];
$medai->query_hypothyroid = $_POST['query_hypothyroid'];
$medai->query_hyperthyroid = $_POST['query_hyperthyroid'];
$medai->pregnant = $_POST['pregnant'];
$medai->sick = $_POST['sick'];
$medai->tumor = $_POST['tumor'];
$medai->lithium = $_POST['lithium'];
$medai->goitre = $_POST['goitre'];
$medai->TSH_measured = $_POST['TSH_measured'];
$medai->TSH = $_POST['TSH'];
$medai->T3_measured = $_POST['T3_measured'];
$medai->T3 = $_POST['T3'];
$medai->TT4_measured = $_POST['TT4_measured'];
$medai->TT4 = $_POST['TT4'];
$medai->FTI_measured = $_POST['FTI_measured'];
$medai->FTI = $_POST['FTI'];
$medai->TBG_measured = $_POST['TBG_measured'];
$medai->TBG = $_POST['TBG'];
$medai->patient_name = "";
$medai->cookie = "";
$medai->action = "analize";

$data_to_send = json_encode($medai);

include '../init/config.php';
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
		echo "Offline..";
	}
	else
	{
		$decoded = json_decode($recvdata,true);
		
		echo "Rezultat hipotiroidism " . $decoded["rezultat"][0][0] * 100 . "%";
	}
	
}
else
{
	echo "Error " . $errstr;
}


}
?>
