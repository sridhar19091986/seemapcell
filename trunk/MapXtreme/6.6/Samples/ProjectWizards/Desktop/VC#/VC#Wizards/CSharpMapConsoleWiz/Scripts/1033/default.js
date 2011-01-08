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

		var bEmptyProject = 0; //wizard.FindSymbol("EMPTY_PROJECT");

		var proj = CreateCSharpProject(strProjectName, strProjectPath, "default.csproj");

		var InfFile = CreateInfFile();
		if (!bEmptyProject && proj)
		{
			AddReferencesForClass(proj);
			AddReferencesMapXtremeReferences(proj);
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
			strTarget = "Class1.cs";
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
		case "Class1.cs":
			bOpen = true;
			break;
	}
	return bOpen; 
}

function SetFileProperties(oFileItem, strFileName)
{
    if(strFileName == "File1.cs" || strFileName == "assemblyinfo.cs")
    {
        oFileItem.Properties("SubType").Value = "Code";
    }
}

function AddReferencesMapXtremeReferences(oProj) {
	var refmanager = GetCSharpReferenceManager(oProj);
	var wshShell = new ActiveXObject("WScript.Shell");
	var AppDataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\ApplicationDir");
	refmanager.Add(AppDataFolder + "\\MapInfo.CoreTypes");
	refmanager.Add(AppDataFolder + "\\MapInfo.CoreEngine");
}
