var fso = new ActiveXObject("Scripting.FileSystemObject");

function AddDefaultServerScriptToWizard(selProj)
{
	wizard.AddSymbol("DEFAULT_SERVER_SCRIPT", "JavaScript");
}

function AddDefaultClientScriptToWizard(selProj)
{
    var prjScriptLang = selProj.Properties("DefaultClientScript").Value;
    // 0 = JScript
    // 1 = VBScript
    if(prjScriptLang == 0)
    {
        wizard.AddSymbol("DEFAULT_CLIENT_SCRIPT", "JavaScript");
    }
    else
    {
        wizard.AddSymbol("DEFAULT_CLIENT_SCRIPT", "VBScript");
    }
}

function AddDefaultDefaultHTMLPageLayoutToWizard(selProj)
{
    var prjPageLayout = selProj.Properties("DefaultHTMLPageLayout").Value;
    // 0 = FlowLayout
    // 1 = GridLayout
    if(prjPageLayout == 0)
    {
        wizard.AddSymbol("DEFAULT_HTML_LAYOUT", "FlowLayout");
    }
    else
    {
        wizard.AddSymbol("DEFAULT_HTML_LAYOUT", "GridLayout");
    }
}

function AddFileToCSharpProject(oProj, strProjectName, strProjectPath, strName)
{
	dte.SuppressUI = false;
	var projItems = oProj.ProjectItems;
	var strTemplatePath = wizard.FindSymbol("TEMPLATES_PATH");

	// if( Not a web project )
	if(strProjectPath.charAt(strProjectPath.length - 1) != "\\")
		strProjectPath += "\\";	

	var strTarget = "";
	var strFile = "";
	strTarget = GetCSharpTargetName(strName, strProjectName);

	var fso;
	fso = new ActiveXObject("Scripting.FileSystemObject");
	var TemporaryFolder = 2;
	var tfolder = fso.GetSpecialFolder(TemporaryFolder);
	var strTempFolder = fso.GetAbsolutePathName(tfolder.Path);

	var strFile = strTempFolder + "\\" + fso.GetTempName();

	var strClassName = strTarget.split(".");
	wizard.AddSymbol("SAFE_CLASS_NAME", strClassName[0]);
	    wizard.AddSymbol("SAFE_ITEM_NAME", strClassName[0]);

	var strTemplate = strTemplatePath + "\\" + strName;
	var bCopyOnly = false;
	var strExt = strName.substr(strName.lastIndexOf("."));
	if(strExt==".bmp" || strExt==".ico" || strExt==".gif" || strExt==".rtf" || strExt==".css")
		bCopyOnly = true;
	wizard.RenderTemplate(strTemplate, strFile, bCopyOnly, true);

	var projfile = projItems.AddFromTemplate(strFile, strTarget);
	SafeDeleteFile(fso, strFile);
			
	if(projfile)
		SetFileProperties(projfile, strName);

	var bOpen = false;
	if (DoOpenFile(strTarget))
		bOpen = true;

	if(bOpen)
	{
		var window = projfile.Open(vsViewKindPrimary);
		window.visible = true;
	}
}

function OnFinish(selProj, selObj)
{
    var oldSuppressUIValue = true;
	try
	{
        oldSuppressUIValue = dte.SuppressUI;

		var strProjectPath = wizard.FindSymbol("PROJECT_PATH");
		var strProjectName = wizard.FindSymbol("PROJECT_NAME");

		var bEmptyProject = 0; //wizard.FindSymbol("EMPTY_PROJECT");

		var proj = CreateCSharpProject(strProjectName, strProjectPath, "defaultwebproject.csproj");

		if( !ProjectIsARootWeb( strProjectPath ) )
		{
			wizard.AddSymbol("NOT_ROOT_WEB_APP", true);
		}
		var InfFile = CreateInfFile();
		if (!bEmptyProject && proj)
		{
			// Open registry and get sample data path
			var wshShell = new ActiveXObject("WScript.Shell");
			var dataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\SampleDataSearchPath");
			wizard.AddSymbol("WS_PATH", dataFolder +"\\world.mws");
			
			AddReferencesForWebForm(proj);
			var refmanager = GetCSharpReferenceManager(proj);
			var wshShell = new ActiveXObject("WScript.Shell");
			var AppDataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\ApplicationDir");
			refmanager.Add(AppDataFolder + "\\MapInfo.CoreTypes");
			refmanager.Add(AppDataFolder + "\\MapInfo.CoreEngine");
			refmanager.Add(AppDataFolder + "\\MapInfo.WebControls");
			AddDesignerFileToCSharpWebProject(proj, strProjectName, strProjectPath, "Global.asax", false);
		
			SafeDeleteFile(fso, proj.FileName.substring(0, proj.FileName.lastIndexOf("\\")) + "\\Global.asax.cs");

	        // add the code behind file back:
			AddFileToCSharpProject(proj, strProjectPath, "Global.asax.cs", "Global.asax.cs");

			// add the default project props for the aspx file before we
			// render it
			AddDefaultServerScriptToWizard(proj);
			AddDefaultClientScriptToWizard(proj);
			AddDefaultTargetSchemaToWizard(proj);
			AddDefaultDefaultHTMLPageLayoutToWizard(proj);

			// render our files
			AddFilesToCSharpProject(proj, strProjectName, strProjectPath, InfFile, false);
			
			// The trick to replacing default webform1.aspx.cs file with custom one is to install a temporary .cs file first and then
			// rename it to webform1.aspx.cs file, and then remove that temporary .cs file from project items.
			var src = fso.GetParentFolderName(proj.FullName)+"\\WebForm1_temp.aspx.cs";
			var dest = fso.GetParentFolderName(proj.FullName)+"\\WebForm1.aspx.cs";
			
			// First delete the webform1.aspx.cs file, so that we don't get error
			SafeDeleteFile(fso, dest);
			
			// Since there is no rename, use move
			fso.MoveFile(src, dest);
			
			// Now delete the temp aspx.cs item from project
			proj.ProjectItems("WebForm1_temp.aspx.cs").Delete();
			
			SetStartupPage(proj, "WebForm1.aspx");
            CollapseReferencesNode(proj);
		}
		proj.Save();
	}
	catch(e)
	{
		if( e.description.length > 0 )
			SetErrorInfo(e);
		return e.number;
	}
    finally
    {
   		dte.SuppressUI = oldSuppressUIValue;
   		if( InfFile )
			InfFile.Delete();
    }
}

function GetCSharpTargetName(strName, strProjectName)
{
	var strTarget = strName;

	switch (strName)
	{
		case "readme.txt":
			strTarget = "ReadMe.txt";
			break;
		case "WebForm1.aspx":
			strTarget = "WebForm1.aspx";
			break;
		case "assemblyinfo.cs":
			strTarget = "AssemblyInfo.cs";
			break;
		case "Global.asax":
			strTarget = "Global.asax";
			break;
		case "Global.asax.cs":
			strTarget = "Global.asax.cs";
			break;
		case "Web.config":
			strTarget = "Web.config";
			break;
		case "DynamicDisco.disco":
			strTarget = strProjectName + ".vsdisco";
			break;
	}
	return strTarget; 
}

function DoOpenFile(strName)
{
	var bOpen = false;
    
	switch (strName)
	{
		case "WebForm1.aspx":
			bOpen = true;
			break;
	}
	return bOpen; 
}
function SetFileProperties(oFileItem, strFileName)
{
    if(strFileName == "WebForm1.aspx")
    {
        oFileItem.Properties("SubType").Value = "Form";
    }
    if(strFileName == "Global.asax")
    {
        oFileItem.Properties("SubType").Value = "Component";
    }
}
