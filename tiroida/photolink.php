<?php 



?>


<link id="bootstrap-styleshhet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css" rel="stylesheet" type="text/css"/>
<link href="https://maxcdn.bootstrapcdn.com/font-awesome/4.5.0/css/font-awesome.min.css" rel="stylesheet" type="text/css"/>
<link href="css/w3.css">
<link href="css/responsive.min.css" rel="stylesheet">
<link href="css/style.min.css" rel="stylesheet">
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
 
 
 <div class="row" style="">
        <div class="col-md-12">
            <div class="col-md-12 text-center">
                <form role="form" action="upload.php" method="post" enctype="multipart/form-data">
				
				
					<div class="row">
					
					
	<div class="form-group">
      <label for="sel1" style="margin-right:20%;font-size:15px;">Selectati boala:</label>
      <select name="disease" id="disease" class="form-control" style="width:160px;height:35px;font-size:15px;">
        <option style="font-size:15px;" value="1">Pneumonie</option>
        <option style="font-size:15px;" value="2">Tumberculoza</option>
		<option style="font-size:15px;" value="3">Hemoragie la creier</option>
		<option style="font-size:15px;" value="4">Cancer la san</option>
		<option style="font-size:15px;" value="5">Leucemie</option>
		<option style="font-size:15px;" value="6">Malarie</option>
		<option style="font-size:15px;" value="7">Cancer la piele</option>
		<option style="font-size:15px;" value="8">Parkinson</option>
      </select>
      
    </div>
	
	

					
					</div>
				
					
					
					
                    <div class="row">
                        <div class="col-xs-12 col-md-12">
                            <div class="col-md-12 col-lg-12 col-xs-12" id="columns">
                                <h2 class="form-label" style="font-size:25px;">Selecteaza imaginea</h3>
                                <div class="desc"><p class="text-center" style="font-size:14px;">poti folosi functia drag & drop</p></div>
                                <div id="uploads"><!-- Upload Content --></div>
                            </div>
                            <div class="clearfix"></div>
                            <button class="btn btn-danger btn-lg pull-left" id="reset" type="button" ><i class="fa fa-history"></i> Clear</button>
                            <button class="btn btn-primary btn-lg pull-right" type="submit" ><i class="fa fa-upload"></i> Upload </button>
                        </div>
                    </div>
					
					
					
                </form>
            </div>
        </div>
    </div>
	
	
	 <br>
	<div class="form-group">
      <label style="font-size:15px;" id="response" for="sel1"></label>
    </div>
	
	
<script src="js/jquery.min.js"></script>
<script src="js/modernizr.min.js"></script>
<script src="js/uploadHBR.min.js"></script>
<script>
        $(document).ready(function () {
            uploadHBR.init({
                "target": "#uploads",
                "max": 1,
                "textNew": "ADD",
                "textTitle": "Click here or drag to upload an imagem",
                "textTitleRemove": "Click here remove the imagem"
            });
            $('#reset').click(function () {
                uploadHBR.reset('#uploads');
            });
        });
</script>
