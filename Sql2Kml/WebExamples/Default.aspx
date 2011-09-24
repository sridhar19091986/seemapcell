<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/GenericHandler.ashx?reqType=ex1init&fileType=kmz">Run WebExample1 (Dynamic auto refreshing data using NetworkLink)</asp:HyperLink>&nbsp;<br />
        <br />
        (.../WebExamples/GenericHandler.ashx?reqType=<strong>ex1init</strong>&amp;fileType=kmz)<br />
        This link creates a KML which contains one NetworkLink.&nbsp; Remember, a NetworkLink
        is just a container in a KML document that tells Google Earth to go and fetch additional
        KML data from another URL.&nbsp; The NetworkLink is set to refresh every 10 seconds,
        and has an href of
        <br />
        (.../WebExamples/GenericHandler.ashx?reqType=<strong>ex1data</strong>&amp;fileType=kmz)<br />
        This first request could have been static, but I chose to generate it on the fly.<br />
        <br />
        The ex1data request type generates a KML that has our dynamic data.&nbsp; In this
        case I'm just generating a point that assigns the server's DateTime to the Name
        property so that you can see it is dynamic.&nbsp; Since it refreshes every 10 seconds
        based on the parent NetworkLink, you should see the time increment by about 10 seconds
        every refresh.<br />
        <br />
        <br />
        <br />
    
    </div>
    </form>
</body>
</html>
