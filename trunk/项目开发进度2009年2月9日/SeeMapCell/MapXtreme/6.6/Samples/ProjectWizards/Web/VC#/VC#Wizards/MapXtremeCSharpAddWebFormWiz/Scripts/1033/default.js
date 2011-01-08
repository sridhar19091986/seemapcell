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

function OnFinish(selProj, selObj)
{
    var oldSuppressUIValue = true;
	try
	{
        oldSuppressUIValue = dte.SuppressUI;
		var strProjectName		= wizard.FindSymbol("PROJECT_NAME");
		var strSafeProjectName = CreateSafeName(strProjectName);
		wizard.AddSymbol("SAFE_PROJECT_NAME", strSafeProjectName);
		SetTargetFullPath(selObj);
		var strProjectPath		= wizard.FindSymbol("TARGET_FULLPATH");
		var strTemplatePath		= wizard.FindSymbol("TEMPLATES_PATH");

		// Open registry and get sample data path
		var wshShell = new ActiveXObject("WScript.Shell");
		var dataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\SampleDataSearchPath");
		wizard.AddSymbol("WS_PATH", dataFolder +"\\world.mws");

		var strTpl = "";
		var strName = "";
		var InfFile = CreateInfFile();
		
		// add the default project props for the aspx file before we
		// render it
		AddDefaultServerScriptToWizard(selProj);
		AddDefaultClientScriptToWizard(selProj);
		AddDefaultTargetSchemaToWizard(selProj);
		AddDefaultDefaultHTMLPageLayoutToWizard(selProj);

		// Add MapInfo assemblies as reference
		var VSProject = selProj.Object;
		var refmanager = VSProject.References;
		var wshShell = new ActiveXObject("WScript.Shell");
		var AppDataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\ApplicationDir");
		refmanager.Add(AppDataFolder + "\\MapInfo.CoreTypes");
		refmanager.Add(AppDataFolder + "\\MapInfo.CoreEngine");
		refmanager.Add(AppDataFolder + "\\MapInfo.WebControls");
		
		// render our file
		AddFilesToCSharpProject(selObj, strProjectName, strProjectPath, InfFile, true);
		AddReferencesForWebForm(selProj);
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

function SetFileProperties(oFileItem, strFileName)
{
    if(strFileName == "WebForm1.aspx")
    {
        oFileItem.Properties("SubType").Value = "Form";
    }
}
