<Query Kind="Statements">
  <Reference>G:\htmlconvertsql\SqlCompact.v.2011.05.21\Soccer Score Forecast\Soccer Score Forecast\bin\Release\HtmlAgilityPack.dll</Reference>
  <Reference>D:\Program Files\Microsoft SQL Server\100\SDK\Assemblies\Microsoft.SqlServer.Types.dll</Reference>
  <Namespace>HtmlAgilityPack</Namespace>
  <Namespace>Microsoft.SqlServer.Types</Namespace>
  <IncludePredicateBuilder>true</IncludePredicateBuilder>
</Query>

 var point = SqlGeometry.Point(107.04352, 28.870554, 4326);

	Console.WriteLine(point.STX);
	Console.WriteLine(point.STY);
	Console.WriteLine(point.ToString());
	
	
	
	var pointStart = SqlGeometry.Point(107.04352, 28.870554, 4326);
var pointEnd = SqlGeometry.Point(103.84041, 29.170240, 4326);
var result = pointStart.STDistance(pointEnd);
Console.WriteLine("地理距离：" + result + "(米)");

