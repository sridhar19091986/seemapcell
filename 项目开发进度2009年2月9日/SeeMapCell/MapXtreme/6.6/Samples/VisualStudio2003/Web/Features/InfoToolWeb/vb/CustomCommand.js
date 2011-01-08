//client info command to control client behavior for info tool.
function InfoCommand(name, interaction)
{
	if (arguments.length > 0) {
		this.Init(name, interaction);
	}
}
InfoCommand.prototype = new MapCommand();
InfoCommand.prototype.constructor = InfoCommand;
InfoCommand.superclass = MapCommand.prototype;
InfoCommand.prototype.Execute = function()
{
	this.CreateUrl();
	this.AddParamToUrl("PixelTolerance", this.pixelTolerance);
	//create an XMLHttp obj to send request to server
	var xmlHttp = CreateXMLHttp();
	xmlHttp.open("GET", this.url, false);
	xmlHttp.send(null);
	//get response back
	this.result = xmlHttp.responseText;
	
	var div = FindElement("Info");
	if(div.style.visibility != "visible")
		div.style.visibility = "visible";		
	//display the response at client html
	div.innerHTML = "<font size=2 face=Arial><b>Selected Feature Info:</b></font><p>" + this.result;

};
//function to update zoom label
function getZoomValue()
{
	//create url to send to server, server command name is "ZoomValue"
	var url = "MapController.ashx?Command=ZoomValue&Ran=" + Math.random();
	var mapImage = document.getElementById("MapControl1_Image");                        
	if (mapImage.mapAlias) 
		url +=  "&MapAlias=" + mapImage.mapAlias;
		
	var xmlHttp = CreateXMLHttp();
	xmlHttp.open("GET", url, false);
	xmlHttp.send(null);
	var result = xmlHttp.responseText;		
	var div = FindElement("ZoomValue");
	div.innerHTML = "<font size=2 face=Arial><b>Zoom: <font color=Indigo>" + result + "</font></b></font>";
};					





