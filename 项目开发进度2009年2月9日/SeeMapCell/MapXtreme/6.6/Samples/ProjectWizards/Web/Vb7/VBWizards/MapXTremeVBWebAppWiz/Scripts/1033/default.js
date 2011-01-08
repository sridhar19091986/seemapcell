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
        var strTemplateFile = strTemplatePath + "\\WebApplication.vbproj"; 

        var project = CreateVSProject(strProjectName, ".vbproj", strProjectPath, strTemplateFile);
        if( project )
        {
            strProjectName = project.Name;  //In case it got changed

            var item;
            var editor;

			// Open registry and get sample data path
			var wshShell = new ActiveXObject("WScript.Shell");
			var dataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\SampleDataSearchPath");
			wizard.AddSymbol("WS_PATH", dataFolder +"\\world.mws");
			
            strTemplateFile = strTemplatePath + "\\Global.asax"; 
            item = AddFileToVSProject("Global.asax", project, project.ProjectItems, strTemplateFile, false);

			SafeDeleteFile(fso, project.FileName.substring(0, project.FileName.lastIndexOf("\\")) + "\\Global.asax.vb");

			wizard.AddSymbol("SAFE_CLASS_NAME", "Global");
            strTemplateFile = strTemplatePath + "\\Global.asax.vb"; 
            item = AddFileToVSProject("Global.asax.vb", project, project.ProjectItems, strTemplateFile, false);

		    item = DoesFileExistInProj(project,"Web.config")
            if (item == null) 
            {
               strTemplateFile = strTemplatePath + "\\Web.config"; 
               item = AddFileToVSProject("Web.config", project, project.ProjectItems, strTemplateFile, false);
            }

            strTemplateFile = strTemplatePath + "\\Styles.css"; 
            item = AddFileToVSProject("Styles.css", project, project.ProjectItems, strTemplateFile, false);


            var strRawGuid = wizard.CreateGuid();
            wizard.AddSymbol("GUID_ASSEMBLY", wizard.FormatGuid(strRawGuid, 0));

            strTemplateFile = strTemplatePath + "\\AssemblyInfo.vb"; 
            item = AddFileToVSProject("AssemblyInfo.vb", project, project.ProjectItems, strTemplateFile, false);
            if( item )
            {
                item.Properties("SubType").Value = "Code";
            }

            AddDefaultWebFormsPropertiesToWizard(dte, wizard, project);
            
            strTemplateFile = strTemplatePath + "\\WebForm.aspx"; 
            item = AddFileToVSProject("WebForm1.aspx", project, project.ProjectItems, strTemplateFile, false);


            var configs = new Enumerator(project.ConfigurationManager);
            for(;!configs.atEnd();configs.moveNext())
            {
                configs.item().Properties("StartPage").Value = "WebForm1.aspx";
            }
            editor = item.Open(vsViewKindPrimary);
            editor.Visible = true;
            
			// The trick to replacing default webform1.aspx.vb file with custom one is to install a temporary .vb file first and then
			// rename it to webform1.aspx.vb file, and then remove that temporary .vb file from project items.
			
			wizard.AddSymbol("SAFE_CLASS_NAME", "WebForm1");
            strTemplateFile = strTemplatePath + "\\WebForm_temp.aspx.vb"; 
            item = AddFileToVSProject("WebForm1_temp.aspx.vb", project, project.ProjectItems, strTemplateFile, false);
			
			var src = fso.GetParentFolderName(project.FullName)+"\\WebForm1_temp.aspx.vb";
			var dest = fso.GetParentFolderName(project.FullName)+"\\WebForm1.aspx.vb";
			
			// First delete the webform1.aspx.cs file, so that we don't get error
			SafeDeleteFile(fso, dest);
			
			// Since there is no rename, use move
			fso.MoveFile(src, dest);
			
			// Now delete the temp aspx.vb item from project
			project.ProjectItems("WebForm1_temp.aspx.vb").Delete();

			// Add MapInfo assemblies as reference
			var VSProject = project.Object;
			var refmanager = VSProject.References;
			var wshShell = new ActiveXObject("WScript.Shell");
			var AppDataFolder = wshShell.regread("HKLM\\SOFTWARE\\MapInfo\\MapXtreme\\6.6\\ApplicationDir");
			refmanager.Add(AppDataFolder + "\\MapInfo.CoreTypes");
			refmanager.Add(AppDataFolder + "\\MapInfo.CoreEngine");
			refmanager.Add(AppDataFolder + "\\MapInfo.WebControls");
		
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
