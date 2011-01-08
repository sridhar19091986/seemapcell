function OnFinish(selProj, selObj)
{
    var oldSuppressUIValue = true;
	try
	{
        oldSuppressUIValue = dte.SuppressUI;
		var strProjectPath = wizard.FindSymbol("PROJECT_PATH");
		var strProjectName = wizard.FindSymbol("PROJECT_NAME");
		var strSafeProjectName = CreateSafeName(strProjectName);
		wizard.AddSymbol("SAFE_PROJECT_NAME", strSafeProjectName);

		var bEmptyProject = 0;

		var proj = CreateCSharpProject(strProjectName, strProjectPath, "DefaultWinExe.csproj");

		var InfFile = CreateInfFile();
		if (!bEmptyProject && proj)
		{
			AddReferencesForWinForm(proj);
			AddMapXtremeReferences(proj);
			AddFilesToCSharpProject(proj, strProjectName, strProjectPath, InfFile, false);
		}
		proj.Properties("ApplicationIcon").Value = "App.ico";
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
		case "File1.cs":
			strTarget = "MapForm1.cs";
			break;
		case "File1.resx":
			strTarget = "MapForm1.resx";
			break;
		case "assemblyinfo.cs":
			strTarget = "AssemblyInfo.cs";
			break;

	}
	return strTarget; 
}

function DoOpenFile(strName)
{
	var bOpen = false;
    
	switch (strName)
	{
		case "MapForm1.cs":
			bOpen = true;
			break;
	}
	return bOpen; 
}

function SetFileProperties(oFileItem, strFileName)
{
    if(strFileName == "File1.cs")
    {
        oFileItem.Properties("SubType").Value = "Form";
    }
    if(strFileName == "File1.resx")
    {
				// Set build action to embedded resource
        oFileItem.Properties("BuildAction").Value = 3;
    }
}

function AddMapXtremeReferences(oProj) {
	var refmanager = GetCSharpReferenceManager(oProj);
	var wshShell = new ActiveXObject("WScript.Shell");
	var AppDataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\ApplicationDir");
	
	refmanager.Add(AppDataFolder + "\\MapInfo.CoreTypes");
	refmanager.Add(AppDataFolder + "\\MapInfo.CoreEngine");
	refmanager.Add(AppDataFolder + "\\MapInfo.Windows.Framework");
	refmanager.Add(AppDataFolder + "\\MapInfo.Windows");
	refmanager.Add(AppDataFolder + "\\MapInfo.Windows.Dialogs");
}
