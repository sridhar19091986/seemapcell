<%@ WebHandler Language="C#" Class="GenericHandler" %>

using System;
using System.Web;
using Google.KML;

/// <summary>
/// This class will handle returning all of the dynamic KML/KMZ requests coming from Google Earth
/// Usually from a NetworkLink.
/// </summary>
public class GenericHandler : IHttpHandler {

    private const string REQUEST_TYPE_QS = "reqtype";
    private const string FILE_TYPE_QS = "filetype";


    private string requestType = "ex1init";
    private string fileType = "kml";
    private geKML kml;
    
    
    //We're gonna use these just incase were running under the VS web server or under a different port.
    private string host;
    private string port;
    private string app;
    private string appRoot;
    private string baseRequest;
    
    
    /// <summary>
    /// This is your main, or page_load.  It's where we're gonna start.
    /// </summary>
    /// <param name="context"></param>
    public void ProcessRequest (HttpContext context) {

        Setup(context);  //Set up our environment based on current session or query string info.


        switch (requestType.ToLower())
        {
            case "ex1init":
                {
                    //This initial kml will provide the necessary NetworkLink(s) to get other data
                    kml = WebExample1.GetInit( baseRequest + "&" + REQUEST_TYPE_QS + "=ex1data" );
                    break;
                }
            case "ex1data":
                {
                    kml = WebExample1.GetWebExample1Data();
                    break;
                }
            case "ex2init":
                {
                    break;
                }
            case "ex2data":
                {
                    break;   
                }
            
        }
            
        
        
        context.Response.ExpiresAbsolute = DateTime.Now.AddDays(-1);  //make sure the browser (or GE) doesn't cache the response
        
        //This is for local testing only.  It will prompt you to save the file instead of just opening it, incase you want to test.
       // if (context.Request.IsLocal)
        //    context.Response.AppendHeader("content-disposition", "attachment; filename=ge-kml-test-" + DateTime.Now.Ticks.ToString() + "." + FILE_TYPE);
        
        if (fileType.ToLower() == "kmz")
        {
            context.Response.ContentType = "application/vnd.google-earth.kmz";
            context.Response.BinaryWrite(kml.ToKMZ());
        }
        else
        {
            context.Response.ContentType = "application/vnd.google-earth.kml+xml";
            context.Response.BinaryWrite(kml.ToKML());
        }

        //Send everything now and close the server response.        
        context.Response.Flush();
        //context.Response.End();
        
    }
 
    /// <summary>
    /// Needed to implement IHttpHandler, we probably won't use it.
    /// </summary>
    public bool IsReusable {
        get {
            return false;
        }
    }

    /// <summary>
    /// Set up our environment based on current session or query string info.
    /// </summary>
    /// <param name="context"></param>
    private void Setup(HttpContext context)
    {
            
        //We're gonna use these just incase were running under the VS web server or under a different port.
        host = context.Request.ServerVariables["SERVER_NAME"];
        port = context.Request.ServerVariables["SERVER_PORT"];
        app = context.Request.ApplicationPath + "/";
        appRoot = "http://" + host + ":" + port + "/" + app + "/";
        baseRequest = appRoot + "GenericHandler.ashx?" + FILE_TYPE_QS + "=" + fileType;
               
        
        if (context.Request.QueryString[REQUEST_TYPE_QS] != null)
        {
            requestType = context.Request.QueryString[REQUEST_TYPE_QS];
        }

        if ((context.Request.QueryString[FILE_TYPE_QS] != null) && 
            ((context.Request.QueryString[FILE_TYPE_QS].ToLower() == "kml") 
                || (context.Request.QueryString[FILE_TYPE_QS].ToLower() == "kmz")))
        {
            fileType = context.Request.QueryString[FILE_TYPE_QS].ToLower();
        }
        
        
    }

}