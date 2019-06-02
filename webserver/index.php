<?php 


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
	
	
  
  </script>
  
</head>

<body>




<div class="container">
  <h2>MedicalAI Center</h2>
  <p>Va rugam completati toate campurile pentru un rezultat cat mai corect!</p>
  
  
  <div class="columns">
  
  <form>
    <div class="form-group">
      <label for="sel1">Sex:</label>
      <select name="Sex" id="Sex" onchange="changePregnantProp()" class="form-control" style="width:150px;">
        <option value="M">Masculin</option>
        <option value="F">Feminin</option>
      </select>
      
    </div>
	
	<div class="form-group">
      <label for="sel1">Varsta:</label>
	  <br>
      <input name="Age" id="Age" type="number" value="0" min="0" max="1000" />
      <br>
    </div>
	
	
	
	<div class="form-group">
      <label for="sel1">Pacientul ia tratament cu thyroxina?</label>
      <select id="on_thyroxine" name="on_thyroxine" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Investigatiile s-au facut in timpul tratarii cu thyroxina?</label>
      <select id="query_on_thyroxine" name="query_on_thyroxine" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      <br>
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Pacientul are tumoare?</label>
      <select id="tumor" name="tumor" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      <br>
    </div>
	
	
	
	<div class="form-group">
      <label for="sel1">Pacientului i s-a verificat FTI-ul?</label>
      <select name="FTI_measured" id="FTI_sender" onchange="changeDisableProp('FTI_sender','FTI')" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
		<option value="f">Necunoscut</option>
      </select>
      
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Valaore FTI:</label>
	  <br>
      <input name="FTI" id="FTI" type="number" step="0.01" value="0.01" min="0" max="1000" />
      <br>
    </div>
	

	<div class="form-group">
      <label for="sel1">Pacientul a fost operat la tiroida?</label>
	  <br>
      <select id="thyroid_surgery" name="thyroid_surgery"  class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      <br>
	  
    </div>
	
	<div class="form-group">
      <label for="sel1">Pacientul ia tratament cu antitiroidiene?</label>
	  <br>
      <select id= "on_antithyroid_medication" name="on_antithyroid_medication" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      <br>
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Pacientul este suspect de hypothyroia?</label>
	  <br>
      <select id= "query_hypothyroid" name="query_hypothyroid" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      <br>
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Pacientul este suspect de hypertiroida?</label>
	  <br>
      <select id ="query_hyperthyroid" name="query_hyperthyroid" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      <br>
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Pacienta este insarcinata?</label>
	  <br>
      <select id="pregnant" name="pregnant" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      <br>
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Pacientul este racit?</label>
	  <br>
      <select id="sick" name="sick" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      <br>
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Pacientul a fost tratat cu litiu?</label>
	  <br>
      <select id="lithium" name="lithium" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      <br>
    </div>
	
	<br>
	<div class="form-group">
      <label for="sel1">Pacientul i s-a verificat TBG-ul?</label>
	  <br>
      <select name="TBG_measured" id="TBG_sender" onchange="changeDisableProp('TBG_sender','TBG')" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
		<option value="f">Necunoscut</option>
      </select>
      <br>
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Valaore TBG:</label>
	  <br>
      <input name="TBG" id="TBG" type="number" step="0.01" value="0.01" min="0" max="1000" />
      <br>
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Pacientul are gusa?</label>
	  <br>
      <select id="goitre" name="goitre" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
      </select>
      <br>
    </div>
	
	
	
	
	<div class="form-group">
      <label for="sel1">Pacientului i s-a verificat TSH-ul?</label>
	  <br>
      <select name="TSH_measured" id="TSH_sender" onchange="changeDisableProp('TSH_sender','TSH')" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
		<option value="f">Necunoscut</option>
      </select>
      
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Valaore TSH:</label>
	  <br>
      <input name="TSH" id="TSH" type="number" step="0.01" value="0.01" min="0" max="1000" />
      <br>
    </div>
	
	
	
	
	
	
	
		<div class="form-group">
      <label for="sel1">Pacientului i s-a verificat T3-ul?</label>
	  <br>
      <select name="T3_measured" id="T3_sender" onchange="changeDisableProp('T3_sender','T3')" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
		<option value="f">Necunoscut</option>
      </select>
      
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Valaore T3:</label>
	  <br>
      <input name="T3" id="T3" type="number" step="0.01" value="0.01" min="0" max="1000" />
      
    </div>
	
	
	
	
	
	<div class="form-group">
      <label for="sel1">Pacientului i s-a verificat TT4-ul?</label>
	  <br>
      <select name="TT4_measured" id="TT4_sender" onchange="changeDisableProp('TT4_sender','TT4')" class="form-control" style="width:150px;">
        <option value="t">Da</option>
        <option value="f">Nu</option>
		<option value="f">Necunoscut</option>
      </select>
      
    </div>
	
	
	<div class="form-group">
      <label for="sel1">Valaore TT4:</label>
	  <br>
      <input name="TT4" id="TT4" type="number" step="0.01" value="0.01" min="0" max="1000" />
      <br>
    </div>
	
  </form>
  
  </div>
  
  <button id="datasendbutton" type="button" onclick="sendandChangeData()" style="margin-top:5%;" class="btn btn-primary btn-lg">Trimite datele</button>
  <br>
  <br>
  
  <div id="loading" style="display:none;" class="spinner-border text-primary" role="status">
  <span class="sr-only">Loading...</span>
  </div>
  <br>
  
	<div class="form-group">
      <label id="response" for="sel1"></label>
    </div>
	
</div>


<div class="footer">





</div>



<script>

changePregnantProp();
changeDisableProp('TT4_sender','TT4');
changeDisableProp('T3_sender','T3');
changeDisableProp('TSH_sender','TSH');
changeDisableProp('TBG_sender','TBG');
changeDisableProp('FTI_sender','FTI')

</script>


</body>




<html>