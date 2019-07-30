<script>

function sendData(imagePath, disease)
{
	var data = "imagePath=" + imagePath + "&disease=" + disease;
	
	
	var xhttp = new XMLHttpRequest();
	xhttp.onreadystatechange = function(){
			
		if(this.readyState == 4 && this.status == 200)
		{
			document.getElementById("loading").style.display = "none";
			document.getElementById("response").innerHTML = this.responseText;
		}
	}
	xhttp.open("POST","ajax/photoupload.php");
	xhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
	xhttp.send(data);
	
}


</script>