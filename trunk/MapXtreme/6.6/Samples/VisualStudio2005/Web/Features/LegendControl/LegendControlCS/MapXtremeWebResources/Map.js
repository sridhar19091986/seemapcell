function MapInfo_Web_UI_WebControls_Map_SetActiveTool(tool)
{
	if(this.activeTool != null)
	{
		this.activeTool.Deactivate(this);
	}
	this.activeTool = tool;
	if(this.activeTool != null)
	{
		this.activeTool.Activate(this);
	}
}

function MapInfoWebAddElement(element, id)
{
	var obj = document.createElement(element);
	obj.id = id;
	return(obj);
}

function MapInfoWebCreateHiddenField(theform, elemID, elemName, elemValue)
{
	var ua = navigator.userAgent.toLowerCase();
	var isIE = ( ua.indexOf("msie") != -1 );

	var elem = document.getElementById(elemID);
	if(elem == null)
	{
		if (isIE)
		{
			elem = document.createElement("<input />");
		} else {
			elem = MapInfoWebAddElement("INPUT", elemID);
		}
		elem.type = "hidden";
		elem.id = elemID;
		elem.name = elemName;
		theform.appendChild(elem);
	}
	elem.value = elemValue;
	return elem;
}

function MapInfoWebSelectLayers(){

}

function MapInfoWebSelectLayer(){
	this.name="";
	this.selectable="true";
}

function MapInfoWebCreateSelectableLayerField(id)
{
	var theform = document.forms[0];
	var sellyrsstring="";
	var atleastone = false;
	var lyrsArray = eval("_selectlayers_"+id);
	for ( i=0;i<lyrsArray.Layers.length;i++ )
	{
		var layer = lyrsArray.Layers[i];
		if (layer.selectable == "True")
		{
			if (atleastone)
			{
				sellyrsstring += ',';
			}
			sellyrsstring += layer.name;
			atleastone = true;
		}
	}
	MapInfoWebCreateHiddenField(theform, id+"_SelLayers", id+"_SelLayers", sellyrsstring);
}
