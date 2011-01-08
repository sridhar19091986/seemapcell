function OnFinish(selProj, selObj)
{
    var oldSuppressUIValue = true;
    try
    {
        oldSuppressUIValue = dte.SuppressUI;
        var bSilent = wizard.FindSymbol("SILENT_WIZARD");
        dte.SuppressUI = bSilent;

		// Open registry and get sample data path
		var wshShell = new ActiveXObject("WScript.Shell");
		var dataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\SampleDataSearchPath");
		wizard.AddSymbol("WS_PATH", dataFolder +"\\world.mws");
			
        var strItemName = wizard.FindSymbol("ITEM_NAME");
        var strTemplatePath = wizard.FindSymbol("TEMPLATES_PATH");
        var strTemplateFile = strTemplatePath + "\\WebForm.aspx"; 

        AddDefaultWebFormsPropertiesToWizard(dte, wizard, selProj);

		// Add MapInfo assemblies as reference
		var VSProject = selProj.Object;
		var refmanager = VSProject.References;
		var wshShell = new ActiveXObject("WScript.Shell");
		var AppDataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\ApplicationDir");
		refmanager.Add(AppDataFolder + "\\MapInfo.CoreTypes");
		refmanager.Add(AppDataFolder + "\\MapInfo.CoreEngine");
		refmanager.Add(AppDataFolder + "\\MapInfo.WebControls");
		
        var item = AddFileToVSProject(strItemName, selProj, selObj, strTemplateFile, false);
        if( item )
        {
            var editor = item.Open(vsViewKindPrimary);
            editor.Visible = true;
        }
        
        return 0;
    }
    catch(e)
    {   
        switch(e.number)
        {
        case -2147221492 /* OLE_E_PROMPTSAVECANCELLED */ :
            return -2147221492;

        case -2147024816 /* FILE_ALREADY_EXISTS */ :
        case -2147213313 /* VS_E_WIZARDBACKBUTTONPRESS */ :
            return -2147213313;

        default:
            ReportError(e.description);
            return -2147213313;
        }
    }
    finally
    {
        dte.SuppressUI = oldSuppressUIValue;
    }
}
