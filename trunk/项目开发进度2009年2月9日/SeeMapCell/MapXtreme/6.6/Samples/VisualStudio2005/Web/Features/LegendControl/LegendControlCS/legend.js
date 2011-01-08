//server-side command name
CREATE_THEME_SRVCMD = "CreateTheme";
REMOVE_THEME_SRVCMD = "RemoveTheme";
DISPLAY_LEGEND_SRVCMD = "GetLegend";	

LEGEND_EXPORT_FORMAT = "bmp";		

function createTheme()
{		
	var btn = document.getElementById("CreateTheme");
	btn.disabled = true;

	themeCmd.CreateTheme(CREATE_THEME_SRVCMD);
	
	btn = document.getElementById("RemoveTheme");
	btn.disabled = false;
}

function removeTheme()
{
	var btn = document.getElementById("RemoveTheme");
	btn.disabled = true;
	
	themeCmd.RemoveTheme(REMOVE_THEME_SRVCMD);
	
	btn = document.getElementById("CreateTheme");
	btn.disabled = false;
		
}

//CustomCommand definition, base class for ThemeCommand and LegendCommand
function CustomCommand(mapImageID)
{
	
}
CustomCommand.prototype.CreateUrl = function()
{
	this.url = "MapController.ashx?Ran=" + Math.random();
					
	if (this.mapImage.mapAlias) this.AddParamToUrl("MapAlias", this.mapImage.mapAlias);

}
CustomCommand.prototype.AddParamToUrl = function(param, value)
{
	this.url += "&" + param + "=" + value;
	
}

//Width, Height, ExportFormat, Command are predefined names at server side to get request values
//So a couple of functions defined to hard-code these key names
CustomCommand.prototype.AddMapSizeToUrl = function(width, height)
{
	this.url += "&Width=" + width;
	this.url += "&Height=" + height;	
}

CustomCommand.prototype.AddMapExportFormatToUrl = function(format)
{
	this.url +=  "&ExportFormat=" + format;	
}

CustomCommand.prototype.AddSrvCommandNameToUrl = function(cmdName)
{
	
	this.url += "&Command=" + cmdName;	
}

CustomCommand.prototype.Init = function(mapImageID)
{
	this.mapImageID = mapImageID;
	this.mapImage = document.getElementById(mapImageID);
}

//ThemeCommand inherits CustomCommand
function ThemeCommand(mapImageID)
{
	if (arguments.length > 0) {
		this.Init(mapImageID);
	}
}
ThemeCommand.prototype = new CustomCommand();
ThemeCommand.prototype.constructor = ThemeCommand;
ThemeCommand.superclass = CustomCommand.prototype;
ThemeCommand.prototype.Init = function(mapImageID)
{
	ThemeCommand.superclass.Init.call(this, mapImageID);
}

ThemeCommand.prototype.CreateTheme = function(srvCmdName)
{
	this.CreateUrl();
	this.AddMapSizeToUrl(this.mapImage.width, this.mapImage.height);
	this.AddMapExportFormatToUrl(this.mapImage.exportFormat);
	this.AddSrvCommandNameToUrl(srvCmdName);	
    //attach onload event to map image, so when the map image is loaded, legend will display     
	this.mapImage.attachEvent('onload', this.DisplayLegend);
	
	try 
	{
		this.mapImage.src = this.url;
	} 
	catch(e) 
	{ 
		alert("ll"); 
	}					
}

ThemeCommand.prototype.RemoveTheme = function(srvCmdName)
{
	this.CreateUrl();
	this.AddMapSizeToUrl(this.mapImage.width, this.mapImage.height);
	this.AddMapExportFormatToUrl(this.mapImage.exportFormat);
	this.AddSrvCommandNameToUrl(srvCmdName);	
	
	this.mapImage.attachEvent('onload', this.HideLegend);
	
	try 
	{
		this.mapImage.src = this.url;
	} 
	catch(e) 
	{ 
		alert("ll"); 
	}					
	
}

ThemeCommand.prototype.DisplayLegend = function()
{	
	legendCmd.DisplayLegend(DISPLAY_LEGEND_SRVCMD, LEGEND_EXPORT_FORMAT);
}

ThemeCommand.prototype.HideLegend = function()
{	
	legendCmd.HideLegend();
}

//LegendCommand inherits CustomCommand
function LegendCommand(mapImageID, legendSpanID)
{
	if (arguments.length > 0) {
		this.Init(mapImageID, legendSpanID);
	}
}
LegendCommand.prototype = new CustomCommand();
LegendCommand.prototype.constructor = LegendCommand;
LegendCommand.superclass = CustomCommand.prototype;
LegendCommand.prototype.Init = function(mapImageID, legendSpanID)
{
	LegendCommand.superclass.Init.call(this, mapImageID);	
	this.legendSpanID = legendSpanID;
	this.legendImageID = legendSpanID + "_Image";
}
LegendCommand.prototype.DisplayLegend = function(srvCommandName, legendExportFormat)
{
	this.CreateUrl();
	this.AddSrvCommandNameToUrl(srvCommandName);
	this.AddParamToUrl("LegendExportFormat", legendExportFormat);
	
	var legendControl = document.getElementById(this.legendSpanID);	
	var legendImage = document.getElementById(this.legendImageID);
	
	legendControl.innerText = "";	
	//if there is alreay legend image drawn, remove it.
	if(legendImage != null)
		legendControl.innerHTML = null;
	
	var inner = "<img id='" + this.legendImageID +  "' src='" + this.url + "'/>";
	legendControl.innerHTML = inner;
	
	legendControl.style.visibility = "visible";
	//detach the map image onload event, because the legend is already created.
	this.mapImage.detachEvent('onload', themeCmd.DisplayLegend);
}

LegendCommand.prototype.HideLegend = function()
{
	var legendControl = document.getElementById(this.legendSpanID);	
	legendControl.style.visibility = "hidden";
	//detach the map image onload event, because the legend is already hided.
	this.mapImage.detachEvent('onload', themeCmd.HideLegend);		
}

//define themeCmd and legendCmd, which can be used throughout the application.
var themeCmd = new ThemeCommand("MapControl1_Image");
var legendCmd = new LegendCommand("MapControl1_Image", "Legend1");