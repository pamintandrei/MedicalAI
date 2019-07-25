<?php 

$base_name = basename($_SERVER['PHP_SELF']);
?>

<!DOCTYPE html>
<html lang="en">
<head>
  <title>MedicalAI Center</title>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
  <link rel="stylesheet" href="css/styles.css">
  <link rel="icon" href="images/MedicalAI.ico">
  <link href="https://fonts.googleapis.com/css?family=Merriweather&display=swap" rel="stylesheet"> 
  <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js" integrity="sha384-q8i/X+965DzO0rT7abK41JStQIAqVgRVzpbzo5smXKp4YfRvH+8abtTE1Pi6jizo" crossorigin="anonymous"></script>
  <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.7/umd/popper.min.js" integrity="sha384-UO2eT0CpHqdSJQ6hJty5KVphtPhzWj9WO1clHTMGa3JDZwrnQq4sF86dIHNDz0W1" crossorigin="anonymous"></script>
  <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.min.js" integrity="sha384-JjSmVgyd0p3pXB1rRibZUAYoIIy6OrQ6VrjIEaFf/nJGzIxFDsf4x0xIM+B07jRM" crossorigin="anonymous"></script>
  <script>
  
  
  
  
	function changeDisableProp(sentid,changerid)
	{
		
		var sentidvalue = document.getElementById(sentid).value;
		if(sentidvalue.localeCompare("t") == 0)
		{
			document.getElementById(changerid).disabled = false;
		}
		else
		{
			if(sentidvalue.localeCompare("f") == 0)
			{
			document.getElementById(changerid).disabled = true;
			}
		}
	}
  
  
	function changePregnantProp()
	{
		
		var sex = document.getElementById("Sex").value;
		if(sex.localeCompare("M") == 0)
		{
			document.getElementById("pregnant").disabled = true;
			
		}
		else
		{
			if(sex.localeCompare("F") == 0)
			{
				document.getElementById("pregnant").disabled = false;
			}
			
		}
		
	}
	
	function getValue(inputid)
	{
		if(document.getElementById(inputid).disabled)
		{
			return "?";
		}
		else
		{
			return document.getElementById(inputid).value;
		}
		
	}
	
	function getPregnant()
	{
		if(document.getElementById("pregnant").disabled)
		{
			return "-1";
		}
		else
		{
			return document.getElementById("pregnant").value;
		}
		
	}
	
	
	function generateData()
	{
		
		var exportdata = "Sex=" + document.getElementById("Sex").value;
		exportdata += "&Age=" + document.getElementById("Age").value;
		exportdata += "&on_thyroxine=" + document.getElementById("on_thyroxine").value;
		exportdata += "&query_on_thyroxine=" + document.getElementById("query_on_thyroxine").value;
		exportdata += "&on_antithyroid_medication=" + document.getElementById("on_antithyroid_medication").value;
		exportdata += "&thyroid_surgery=" + document.getElementById("thyroid_surgery").value;
		exportdata += "&query_hypothyroid=" + document.getElementById("query_hypothyroid").value;
		exportdata += "&query_hyperthyroid=" + document.getElementById("query_hyperthyroid").value;
		exportdata += "&lithium=" + document.getElementById("lithium").value;
		exportdata += "&sick=" + document.getElementById("sick").value;
		exportdata += "&pregnant=" + getPregnant();
		exportdata += "&tumor=" + document.getElementById("tumor").value;
		exportdata += "&goitre=" + document.getElementById("goitre").value;
		exportdata += "&TSH_measured=" + document.getElementById("TSH_sender").value;
		exportdata += "&TSH=" + getValue("TSH");
		exportdata += "&T3_measured=" + document.getElementById("T3_sender").value;
		exportdata += "&T3=" + getValue("T3");
		exportdata += "&TT4_measured=" + document.getElementById("TT4_sender").value;
		exportdata += "&TT4=" + getValue("TT4");
		exportdata += "&FTI_measured=" + document.getElementById("FTI_sender").value;
		exportdata += "&FTI=" + getValue("FTI");
		exportdata += "&TBG_measured=" + document.getElementById("TBG_sender").value;
		exportdata += "&TBG=" + getValue("TBG");
		
		return exportdata;
	}
	
	
	function generateDatahyper()
	{
		
		var exportdata = "Sex=" + document.getElementById("Sex").value;
		exportdata += "&Age=" + document.getElementById("Age").value;
		exportdata += "&on_thyroxine=" + document.getElementById("on_thyroxine").value;
		exportdata += "&query_on_thyroxine=" + document.getElementById("query_on_thyroxine").value;
		exportdata += "&on_antithyroid_medication=" + document.getElementById("on_antithyroid_medication").value;
		exportdata += "&thyroid_surgery=" + document.getElementById("thyroid_surgery").value;
		exportdata += "&query_hypothyroid=" + document.getElementById("query_hypothyroid").value;
		exportdata += "&query_hyperthyroid=" + document.getElementById("query_hyperthyroid").value;
		exportdata += "&lithium=" + document.getElementById("lithium").value;
		exportdata += "&sick=" + document.getElementById("sick").value;
		exportdata += "&pregnant=" + getPregnant();
		exportdata += "&tumor=" + document.getElementById("tumor").value;
		exportdata += "&goitre=" + document.getElementById("goitre").value;
		exportdata += "&TSH_measured=" + document.getElementById("TSH_sender").value;
		exportdata += "&TSH=" + getValue("TSH");
		exportdata += "&T3_measured=" + document.getElementById("T3_sender").value;
		exportdata += "&T3=" + getValue("T3");
		exportdata += "&TT4_measured=" + document.getElementById("TT4_sender").value;
		exportdata += "&TT4=" + getValue("TT4");
		exportdata += "&FTI_measured=" + document.getElementById("FTI_sender").value;
		exportdata += "&FTI=" + getValue("FTI");
		exportdata += "&TBG_measured=" + document.getElementById("TBG_sender").value;
		exportdata += "&TBG=" + getValue("TBG");
		exportdata += "&psych=" + document.getElementById("psych").value;
		exportdata += "&hypopituitary=" + document.getElementById("hypopituitary").value;
		exportdata += "&Il3l_treatment=" + document.getElementById("Il3l_treatment").value;
		
		return exportdata;
	}
	
	
	
	
	function sendandChangeData()
	{
		document.getElementById("loading").style.display = "block";
		document.getElementById("datasendbutton").disabled = true;
		
		var xhttp = new XMLHttpRequest();
		xhttp.onreadystatechange = function(){
			
			if(this.readyState == 4 && this.status == 200)
			{
				document.getElementById("datasendbutton").disabled = false;
				document.getElementById("loading").style.display = "none";
				document.getElementById("response").innerHTML = this.responseText;
			}
		}
		xhttp.open("POST","ajax/personaldata.php");
		xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
		xhttp.send(generateData());
		
		
		
	}
	
	
	function sendandChangeDataHyper()
	{
		document.getElementById("loading").style.display = "block";
		document.getElementById("datasendbutton").disabled = true;
		
		var xhttp = new XMLHttpRequest();
		xhttp.onreadystatechange = function(){
			
			if(this.readyState == 4 && this.status == 200)
			{
				document.getElementById("datasendbutton").disabled = false;
				document.getElementById("loading").style.display = "none";
				document.getElementById("response").innerHTML = this.responseText;
			}
		}
		xhttp.open("POST","ajax/hyperdata.php");
		xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
		xhttp.send(generateDatahyper());
		
	}
	
	
	
  
  </script>
  
</head>

<body>



<nav class="navbar navbar-dark bg-dark">
  <a class="navbar-brand" href="index.php">MedicalAI</a>
  <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
    <span class="navbar-toggler-icon"></span>
  </button>

  <div class="collapse navbar-collapse" id="navbarSupportedContent">
    <ul class="navbar-nav mr-auto">
      <li class="nav-item <?php if($base_name === "index.php") echo 'active'; ?>">
        <a class="nav-link" href="index.php">Hypotiroidism <span class="sr-only">(current)</span></a>
      </li>
      <li class="nav-item <?php if($base_name === "photoverifier.php") echo 'active'; ?>">
        <a class="nav-link" href="photoverifier.php">Verifica poza</a>
      </li>
	  
	  <li class="nav-item <?php if($base_name === "hyper.php") echo 'active'; ?>">
        <a class="nav-link" href="hyper.php">Hipertiroidism</a>
      </li>
      
      
    </ul>

  </div>
</nav>
