function Command(name, interaction)
{
}

Command.prototype.CreateUrl = function()
{
}

Command.prototype.AddParamToUrl = function(param, value)
{
	this.url += "&" + param + "=" + value;
}

Command.prototype.Execute = function()
{
}
Command.prototype.Init = function(name, interaction)
{
	this.name = name;
	if (interaction != null) {
		this.interaction = interaction;
		this.interaction.onComplete = this.eventHandler("Execute");
	}
}

function MapCommand(name, interaction)
{
	if (arguments.length > 0) {
		this.Init(name, interaction);
	}
}
MapCommand.prototype = new Command();
MapCommand.prototype.constructor = MapCommand;
MapCommand.superclass = Command.prototype;
MapCommand.prototype.CreateUrl = function()
{
	var mapImage = this.interaction.element;
	if (!mapImage.mapAlias) mapImage.mapAlias = mapImage.attributes["mapAlias"].value;
	if (!mapImage.exportFormat) mapImage.exportFormat = mapImage.attributes["exportFormat"].value;
	
	this.url = "MapController.ashx?Command="+ this.name + 
					"&Width=" + mapImage.width +
					"&Height=" + mapImage.height +
					"&ExportFormat=" + mapImage.exportFormat +
					"&Ran=" + Math.random();
					
	if (this.interaction.PointsData.NumPoints() > 0) this.AddParamToUrl("Points", this.interaction.PointsData.GetPointsString(mapImage.origin));
	if (mapImage.mapAlias) this.AddParamToUrl("MapAlias", mapImage.mapAlias);
}
MapCommand.prototype.UpdateMap = function()
{
	var mapImage = this.interaction.element;
	
	// Set the source of the image to url to just change the map
	mapImage.style.left = 0;
	mapImage.style.top = 0;
	mapImage.style.clip = 'rect(' + 0 + ' ' +  mapImage.width + ' ' + mapImage.height + ' ' + 0 +')';
	try {
	mapImage.src = this.url;
	} catch(e) { alert("ll"); }
}


MapCommand.prototype.Execute = function()
{
	this.CreateUrl();
	
	this.UpdateMap();
}

function PanCommand(name, interaction)
{
	if (arguments.length > 0) {
		this.Init(name, interaction);
	}
}
PanCommand.prototype = new MapCommand();
PanCommand.prototype.constructor = PanCommand;
PanCommand.superclass = MapCommand.prototype;
PanCommand.prototype.Execute = function()
{
	var mapImage = this.interaction.element;
	
	//This is hack because of the pan redraw problems
	mapImage.style.visibility = "hidden";
	var oldhandler = mapImage.onload;
	mapImage.onload = function (mapImage) {this.style.visibility = ""; this.onload = oldhandler;};
	
	this.CreateUrl();
	// Set the source of the image to url to just change the map
	try {
	mapImage.src = this.url;
	} catch(e) { alert("error"); }
}

// Create XML Http object
function CreateXMLHttp()
{
	var xmlHttp = null;
	if (BrowserType() == IE) {
		xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
	} else if (BrowserType() == NS) {
		xmlHttp = new XMLHttpRequest();
	}
	return xmlHttp;
}

function DistanceCommand(name, interaction)
{
	if (arguments.length > 0) {
		this.Init(name, interaction);
	}
}
DistanceCommand.prototype = new MapCommand();
DistanceCommand.prototype.constructor = DistanceCommand;
DistanceCommand.superclass = MapCommand.prototype;
DistanceCommand.prototype.Execute = function()
{
	this.CreateUrl();
	this.AddParamToUrl("DistanceType", this.distanceType);
	this.AddParamToUrl("DistanceUnit", this.distanceUnit);
	var xmlHttp = CreateXMLHttp();
	xmlHttp.open("GET", this.url, false);
	xmlHttp.send(null);
	this.result = xmlHttp.responseText;
	alert(this.result);
}

function NavigateCommand(name, interaction)
{
	if (arguments.length > 0) {
		this.Init(name, interaction);
	}
	this.Exc = this.eventHandler("Execute");
}
NavigateCommand.prototype = new MapCommand();
NavigateCommand.prototype.constructor = NavigateCommand;
NavigateCommand.superclass = MapCommand.prototype;
NavigateCommand.prototype.Execute = function()
{
	if (this.interaction.element == null) this.interaction.element = FindElement(this.interaction.elementID);
	this.CreateUrl();
	this.AddParamToUrl("Method", this.method);
	this.AddParamToUrl("North", this.north);
	this.AddParamToUrl("East", this.east);
	this.UpdateMap();
}

function ZoomCommand(name, interaction)
{
	if (arguments.length > 0) {
		this.Init(name, interaction);
	}
	this.Exc = this.eventHandler("Execute");
}
ZoomCommand.prototype = new MapCommand();
ZoomCommand.prototype.constructor = ZoomCommand;
ZoomCommand.superclass = MapCommand.prototype;
ZoomCommand.prototype.Execute = function()
{
	if (this.interaction.element == null) this.interaction.element = FindElement(this.interaction.elementID);
	this.CreateUrl();
	this.AddParamToUrl("ZoomLevel", this.zoomLevel);
	this.UpdateMap();
}

function PointSelectionCommand(name, interaction)
{
	if (arguments.length > 0) {
		this.Init(name, interaction);
	}
}
PointSelectionCommand.prototype = new MapCommand();
PointSelectionCommand.prototype.constructor = PointSelectionCommand;
PointSelectionCommand.superclass = MapCommand.prototype;
PointSelectionCommand.prototype.Execute = function()
{
	if (this.interaction.element == null) this.interaction.element = FindElement(this.interaction.elementID);
	this.CreateUrl();
	this.AddParamToUrl("PixelTolerance", this.pixelTolerance);
	this.UpdateMap();
}

