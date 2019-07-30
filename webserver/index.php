<?php 

include 'header.php';

?>

<div class="container">
  
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


<script>

changePregnantProp();
changeDisableProp('TT4_sender','TT4');
changeDisableProp('T3_sender','T3');
changeDisableProp('TSH_sender','TSH');
changeDisableProp('TBG_sender','TBG');
changeDisableProp('FTI_sender','FTI')

</script>


<?php 

include 'footer.php';

?>
