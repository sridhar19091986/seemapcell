function OnFinish(selProj, selObj)
{
    var oldSuppressUIValue = true;
    try
    {
        oldSuppressUIValue = dte.SuppressUI;
        var bSilent = wizard.FindSymbol("SILENT_WIZARD");
        dte.SuppressUI = bSilent;

        var strProjectName = wizard.FindSymbol("PROJECT_NAME");
        var strProjectPath = wizard.FindSymbol("PROJECT_PATH");
        var strTemplatePath = wizard.FindSymbol("TEMPLATES_PATH");
        var strTemplateFile = strTemplatePath + "\\WindowsApplication.vbproj"; 

        var project = CreateVSProject(strProjectName, ".vbproj", strProjectPath, strTemplateFile);
        if( project )
        {
            strProjectName = project.Name;  //In case it got changed

						// Add MapXtreme references
						AddMapXtremeReferences(project);

            var item;
            var editor;

            var strRawGuid = wizard.CreateGuid();
            wizard.AddSymbol("GUID_ASSEMBLY", wizard.FormatGuid(strRawGuid, 0));

            strTemplateFile = strTemplatePath + "\\AssemblyInfo.vb"; 
            item = AddFileToVSProject("AssemblyInfo.vb", project, project.ProjectItems, strTemplateFile, false);
            if( item )
            {
                item.Properties("SubType").Value = "Code";
            }

            strTemplateFile = strTemplatePath + "\\Form.vb"; 
            item = AddFileToVSProject("Form1.vb", project, project.ProjectItems, strTemplateFile, false);
            if( item )
            {
                item.Properties("SubType").Value = "Form";
                project.Properties("StartupObject").Value = project.Properties("RootNamespace").Value + ".Form1";

                editor = item.Open(vsViewKindPrimary);
                editor.Visible = true;
            }
            project.Save();
        }
        
        return 0;
    }
    catch(e)
    {   
        switch(e.number)
        {
        case -2147024816 /* FILE_ALREADY_EXISTS */ :
            return -2147213313;

        default:
            SetErrorInfo(e);
            return e.number;
        }
    }
    finally
    {
        dte.SuppressUI = oldSuppressUIValue;
    }
}

function AddMapXtremeReferences(oProj) {
	var VSProject = oProj.Object;
	var refmanager = VSProject.References;
	var wshShell = new ActiveXObject("WScript.Shell");
	var AppDataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\ApplicationDir");
	
	refmanager.Add(AppDataFolder + "\\MapInfo.CoreTypes");
	refmanager.Add(AppDataFolder + "\\MapInfo.CoreEngine");
	refmanager.Add(AppDataFolder + "\\MapInfo.Windows.Framework");
	refmanager.Add(AppDataFolder + "\\MapInfo.Windows");
	refmanager.Add(AppDataFolder + "\\MapInfo.Windows.Dialogs");
}
