Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design
Imports System.IO
Imports System.Web.UI.Design
Imports System.Web.UI
Imports System.Runtime.InteropServices
Imports EnvDTE
Imports System.Xml
Imports System.Reflection
Imports System.Globalization

Friend Class WebDesignerUtility
	<DllImport("ole32.dll", EntryPoint:="GetRunningObjectTable", _
						SetLastError:=True, _
						ExactSpelling:=True, _
						CallingConvention:=CallingConvention.StdCall)> _
	Private Shared Function GetRunningObjectTable(ByVal res As Integer, _
						 ByRef ROT As UCOMIRunningObjectTable) As Boolean
	End Function

	<DllImport("ole32.dll", EntryPoint:="CreateBindCtx", _
						SetLastError:=True, _
						ExactSpelling:=True, _
						CallingConvention:=CallingConvention.StdCall)> _
	Private Shared Function CreateBindCtx(ByVal res As Integer, _
						 ByRef ctx As UCOMIBindCtx) As Boolean
	End Function

    Private Const S_OK As Integer = 0

    Private Shared ReadOnly _applicationDirRegkey As String = "SOFTWARE\\MapInfo\\MapXtreme\\6.6"
    Private Shared ReadOnly _applicationDirSubkey As String = "ApplicationDir"



	<CLSCompliant(False)> _
	Friend Shared Function GetCurrentDTEObject() As DTE
		Dim dte As EnvDTE.DTE = Nothing

		' Get the ROT
		Dim rot As UCOMIRunningObjectTable
		Dim uret As Integer = GetRunningObjectTable(0, rot)
		If uret = S_OK Then
			' Get an enumerator to access the registered objects
			Dim EnumMon As UCOMIEnumMoniker
			rot.EnumRunning(EnumMon)

			Dim fetched As Integer = 0
			Dim aMons() As UCOMIMoniker = New UCOMIMoniker(1) {}

			If Not EnumMon Is Nothing Then
				Dim dteNameStart As String = "VisualStudio.DTE."
				Dim dteNameEnd As String = ":" + System.Diagnostics.Process.GetCurrentProcess().Id.ToString()
				While EnumMon.Next(1, aMons, fetched) = 0
					' Set up a bindind context so that we can access the monikers
					Dim name As String
					Dim ctx As UCOMIBindCtx
					uret = CreateBindCtx(0, ctx)

					' Get the display string
					aMons(0).GetDisplayName(ctx, Nothing, name)

					' if this is the one we are interested in..
					If name.IndexOf(dteNameStart) <> -1 And name.EndsWith(dteNameEnd) Then
						Dim temp As Object
						rot.GetObject(aMons(0), temp)
						dte = CType(temp, EnvDTE.DTE)
						Exit While
					End If
				End While
			End If
		End If
		Return dte
	End Function

	'/			<summary>Gets the current project.</summary>
	'/			<param name="dte"/>
	'/			<param name="documentUrl"></param>
	'/			<returns>None.</returns>
	<CLSCompliantAttribute(False)> _
	Friend Shared Function GetCurrentWebProject(ByVal dte As DTE, ByVal documentUrl As String) As Project
		Dim project As project = Nothing

		' Proceed if we have a solution open
		If Not (dte Is Nothing) And (dte.Solution.Count > 0) Then
			' Proceed if solution has atleast one project
			Dim activeDoc As Document = dte.ActiveDocument

			' If the URL of the project is same as the URL string returned by the control then
			' we have the current project
			Dim tempProject As project = Nothing
			Dim i As Integer
			For i = 1 To dte.Solution.Projects.Count Step i + 1
				tempProject = dte.Solution.Projects.Item(i)
				If Not tempProject.Properties Is Nothing Then
                    Dim projUrl As String
                    Dim is80 As Boolean
                    Dim us As System.Globalization.CultureInfo = New CultureInfo("en-US")
                    If Double.Parse(tempProject.DTE.Version.ToString(), us) >= 8.0 Then
                        is80 = True
                        projUrl = tempProject.Properties.Item("OpenedURL").Value.ToString()
                    Else
                        projUrl = tempProject.Properties.Item("URL").Value.ToString()
                    End If
                    If documentUrl.IndexOf(projUrl) >= 0 Then
                        project = tempProject
                        'Error: Converting Methods, Functions and Constructors 
                        'Error: Converting If-Else-End If Blocks 

                        If (is80) Then
                            ' Copy the web scripts and images local to the project. First create folder
                            Try
                                Dim folder As ProjectItem = project.ProjectItems.Item(Resources.ResourceFolder)
                            Catch
                                ' Now copy files
                                Dim appPath As String = Nothing
                                Dim regKey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(_applicationDirRegkey)
                                If (Not regKey Is Nothing) Then
                                    appPath = CType(regKey.GetValue(_applicationDirSubkey), String)
                                    project.ProjectItems.AddFromFileCopy(appPath + "\" + Resources.ResourceFolder)
                                    regKey.Close()
                                End If
                            End Try
                        End If
                        Exit For
                    End If
                End If
            Next
		End If
		Return project
	End Function

	Private Shared Function RemoveEnTry(ByVal node As XmlNode, ByVal assembly1 As AssemblyName, ByVal attr As String) As Boolean
		Dim bRemoved As Boolean = False
		If Not node Is Nothing Then
			Dim assemblyAttr As String = node.Attributes(attr).Value
			' Remove the node only if the version is different
			If Not (assembly1 Is Nothing) And assemblyAttr.IndexOf(assembly1.Version.ToString()) < 0 Then
				node.ParentNode.RemoveChild(node)
				bRemoved = True
			End If
		Else
			bRemoved = True
		End If
		Return bRemoved
	End Function

    Private Shared Function AddHttpNode(ByVal xmlDoc As XmlDocument, ByVal httpHandlersNode As XmlNode, ByVal path As String, ByVal type As String) As Boolean
        Dim searchStr As String = String.Format("//add[contains(@path, '{0}')]", path)
        Dim node As XmlNode = xmlDoc.SelectSingleNode(searchStr)
        Dim updated As Boolean = False
        Dim _webAssembly As AssemblyName = GetType(MapInfo.WebControls.MapControl).Assembly.GetName()

        If RemoveEnTry(node, _webAssembly, "type") Then
            Dim addNode As XmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "add", "")
            Dim addAttrib1 As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("verb")
            addAttrib1.Value = "*"
            addNode.Attributes.Append(addAttrib1)

            Dim addAttrib2 As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("path")
            addAttrib2.Value = path
            addNode.Attributes.Append(addAttrib2)

            Dim addAttrib3 As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("type")
            addAttrib3.Value = String.Format("{0}, {1}", type, _webAssembly.FullName)
            addNode.Attributes.Append(addAttrib3)

            httpHandlersNode.AppendChild(addNode)
            updated = True
        End If
        Return updated
    End Function

    Public Shared Function AddController(ByVal xmlDoc As XmlDocument, ByVal handlerName As String, ByVal className As String) As Boolean
        Dim updated As Boolean = False
        ' Now create httphandler node
        Dim httpNode As XmlNode = xmlDoc.SelectSingleNode("//httpHandlers")
        If httpNode Is Nothing Then
            httpNode = xmlDoc.CreateNode(XmlNodeType.Element, "httpHandlers", "")
        End If

        updated = AddHttpNode(xmlDoc, httpNode, handlerName, className)

        ' Now insert this information into system.web node in the file
        If (updated) Then
            Dim sysNode As System.Xml.XmlNodeList = xmlDoc.GetElementsByTagName("system.web")
            sysNode.Item(0).AppendChild(httpNode)
        End If
        Return updated
    End Function

    Private Shared Function AddCommentNode(ByVal xmlDoc As XmlDocument, ByVal appSettingsNode As XmlNode, ByVal comment As String) As Boolean
        If comment Is Nothing Then
            Return False
        End If
        ' Leave if already there:
        Dim child As XmlNode
        For Each child In appSettingsNode.ChildNodes
            Dim childAsComment As XmlComment = Nothing
            Try
                childAsComment = DirectCast(child, XmlComment)
            Catch ex As Exception
                childAsComment = Nothing
            End Try

            If Not childAsComment Is Nothing Then
                If childAsComment.Data.ToLower() = comment.ToLower() Then
                    Return False
                End If
            End If
        Next

        ' Add it:
        Dim commentNode As XmlNode = xmlDoc.CreateComment(comment)
        appSettingsNode.AppendChild(commentNode)
        Return True
    End Function

    Friend Shared Function AddAppSettings(ByVal xmlDoc As XmlDocument) As Boolean
        Dim updated As Boolean = False
        Dim appSettingsNode As XmlNode = xmlDoc.SelectSingleNode("//appSettings")
        If appSettingsNode Is Nothing Then
            appSettingsNode = xmlDoc.CreateNode(XmlNodeType.Element, "appSettings", "")
            Dim compNode As System.Xml.XmlNodeList = xmlDoc.GetElementsByTagName("configuration")
            compNode.Item(0).InsertBefore(appSettingsNode, compNode.Item(0).FirstChild)
            updated = True
        End If
        If appSettingsNode.SelectSingleNode("add[@key='MapInfo.Engine.Session.Pooled']") Is Nothing Then
            updated = updated Or AddCommentNode(xmlDoc, appSettingsNode, L10NUtils.Resources.GetString("PooledSetting"))
            Dim str As String = "<add key=" + Chr(34) + "MapInfo.Engine.Session.Pooled" + Chr(34) + " value=" + Chr(34) + "False" + Chr(34) + " />"
            updated = updated Or AddCommentNode(xmlDoc, appSettingsNode, str)
        End If
        If appSettingsNode.SelectSingleNode("add[@key='MapInfo.Engine.Session.State']") Is Nothing Then
            updated = updated Or AddCommentNode(xmlDoc, appSettingsNode, L10NUtils.Resources.GetString("StateSetting"))
            Dim str As String = "<add key=" + Chr(34) + "MapInfo.Engine.Session.State" + Chr(34) + " value=" + Chr(34) + "HttpSessionState" + Chr(34) + " />"
            updated = updated Or AddCommentNode(xmlDoc, appSettingsNode, str)
        End If
        If appSettingsNode.SelectSingleNode("add[@key='MapInfo.Engine.Session.Workspace']") Is Nothing Then
            updated = updated Or AddCommentNode(xmlDoc, appSettingsNode, L10NUtils.Resources.GetString("WorkspaceSetting"))
            Dim str As String = "<add key=" + Chr(34) + "MapInfo.Engine.Session.Workspace" + Chr(34) + " value=" + Chr(34) + "c:\my workspace.mws" + Chr(34) + " />"
            updated = updated Or AddCommentNode(xmlDoc, appSettingsNode, str)
        End If
        If appSettingsNode.SelectSingleNode("add[@key='MapInfo.Engine.Session.UseCallContext']") Is Nothing Then
            updated = updated Or AddCommentNode(xmlDoc, appSettingsNode, L10NUtils.Resources.GetString("UseCallContextSetting"))
            Dim str As String = "<add key=" + Chr(34) + "MapInfo.Engine.Session.UseCallContext" + Chr(34) + " value=" + Chr(34) + "True" + Chr(34) + " />"
            updated = updated Or AddCommentNode(xmlDoc, appSettingsNode, str)
        End If
        If appSettingsNode.SelectSingleNode("add[@key='MapInfo.Engine.Session.RestoreWithinCallContext']") Is Nothing Then
            updated = updated Or AddCommentNode(xmlDoc, appSettingsNode, L10NUtils.Resources.GetString("RestoreWithinCallContextSetting"))
            Dim str As String = "<add key=" + Chr(34) + "MapInfo.Engine.Session.RestoreWithinCallContext" + Chr(34) + " value=" + Chr(34) + "True" + Chr(34) + " />"
            updated = updated Or AddCommentNode(xmlDoc, appSettingsNode, str)
        End If
        Return updated
    End Function

    ' Handle all modules
    Public Shared Function AddHttpModules(ByVal xmlDoc As XmlDocument) As Boolean
        Dim updated As Boolean = False
        ' Now create httphandler node
        Dim httpNode As XmlNode = xmlDoc.SelectSingleNode("//httpModules")
        If httpNode Is Nothing Then
            httpNode = xmlDoc.CreateNode(XmlNodeType.Element, "httpModules", "")
        End If

        Dim searchStr As String = String.Format("//add[contains(@type, '{0}')]", "MapInfo.Engine.WebSessionActivator")
        Dim node As XmlNode = xmlDoc.SelectSingleNode(searchStr)
        Dim coreEngineAssembly As AssemblyName = GetType(MapInfo.Engine.WebSessionActivator).Assembly.GetName()
        If RemoveEnTry(node, coreEngineAssembly, "type") Then
            Dim addNode As XmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "add", "")

            Dim addAttrib1 As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("type")
            addAttrib1.Value = String.Format("{0}, {1}", "MapInfo.Engine.WebSessionActivator", coreEngineAssembly.FullName)
            addNode.Attributes.Append(addAttrib1)

            Dim addAttrib2 As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("name")
            addAttrib2.Value = "WebSessionActivator"
            addNode.Attributes.Append(addAttrib2)

            httpNode.AppendChild(addNode)
            updated = True
        End If

        ' Now insert this information into system.web node in the file
        If (updated) Then
            Dim sysNode As System.Xml.XmlNodeList = xmlDoc.GetElementsByTagName("system.web")
            sysNode.Item(0).AppendChild(httpNode)
        End If
        Return updated
    End Function

    Private Shared Function AddAssemblyNode(ByVal xmlDoc As XmlDocument, ByVal assembliesNode As XmlNode, ByVal asmbly As AssemblyName) As Boolean
        Dim updated As Boolean = False
        Dim searchStr As String = String.Format("//add[contains(@assembly, '{0}')]", asmbly.Name)
        Dim node As XmlNode = xmlDoc.SelectSingleNode(searchStr)
        If RemoveEnTry(node, asmbly, "assembly") Then
            ' Create add node
            Dim addNode As XmlNode = xmlDoc.CreateNode(XmlNodeType.Element, "add", "")

            ' Set the attribute
            Dim addAttrib As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("assembly")
            addAttrib.Value = asmbly.FullName

            ' Add attribute node to add node
            addNode.Attributes.Append(addAttrib)

            ' Add add node to assembly node
            assembliesNode.AppendChild(addNode)

            updated = True
        End If
        Return updated
    End Function

    ' Handle all assemblies
    Friend Shared Function AddAssemblies(ByVal xmlDoc As XmlDocument) As Boolean
        Dim updated As Boolean = False
        Dim assembliesNode As XmlNode = xmlDoc.SelectSingleNode("//compilation/assemblies")
        If assembliesNode Is Nothing Then
            assembliesNode = xmlDoc.CreateNode(XmlNodeType.Element, "assemblies", "")
        End If

        Dim coreEngineAssembly As AssemblyName = (GetType(MapInfo.Mapping.Map)).Assembly.GetName()
        updated = updated Or AddAssemblyNode(xmlDoc, assembliesNode, coreEngineAssembly)

        Dim coreTypesAssembly As AssemblyName = GetType(MapInfo.Geometry.DPoint).Assembly.GetName()
        updated = updated Or AddAssemblyNode(xmlDoc, assembliesNode, coreTypesAssembly)

        ' Now insert this information into compilation node in the file
        If (updated) Then
            Dim compNode As System.Xml.XmlNodeList = xmlDoc.GetElementsByTagName("compilation")
            compNode.Item(0).AppendChild(assembliesNode)
        End If
        Return updated
    End Function

    Friend Shared Function AddSessionState(ByVal xmlDoc As XmlDocument) As Boolean
        Dim updated As Boolean = False
        Dim sessStateNode As XmlNode = xmlDoc.SelectSingleNode("//sessionState")
        If sessStateNode Is Nothing Then
            sessStateNode = xmlDoc.CreateNode(XmlNodeType.Element, "sessionState", "")

            Dim addAttrib1 As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("mode")
            addAttrib1.Value = "StateServer"
            sessStateNode.Attributes.Append(addAttrib1)

            Dim addAttrib2 As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("stateConnectionString")
            addAttrib2.Value = "tcpip=127.0.0.1:42424"
            sessStateNode.Attributes.Append(addAttrib2)

            Dim addAttrib3 As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("sqlConnectionString")
            addAttrib3.Value = "data source=127.0.0.1;user	id=sa;password="
            sessStateNode.Attributes.Append(addAttrib3)

            Dim addAttrib4 As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("cookieless")
            addAttrib4.Value = "false"
            sessStateNode.Attributes.Append(addAttrib4)

            Dim addAttrib5 As System.Xml.XmlAttribute = xmlDoc.CreateAttribute("timeout")
            addAttrib5.Value = "20"
            sessStateNode.Attributes.Append(addAttrib5)

            Dim sysNode As System.Xml.XmlNodeList = xmlDoc.GetElementsByTagName("system.web")
            sysNode.Item(0).AppendChild(sessStateNode)

            updated = True
        End If

        Return updated
    End Function

    Friend Shared Function GetWebConfig(ByVal documentURL As String, ByRef webConfigPath As String) As XmlDocument
        Dim _dte As DTE = GetCurrentDTEObject()
        webConfigPath = ""
        Dim xmlDoc As XmlDocument = New XmlDocument
        If Not _dte Is Nothing Then
            Dim currentProject As Project = GetCurrentWebProject(_dte, documentURL)

            If Not currentProject Is Nothing Then
                ' Get pathname of the Web.Config file
                'webConfigPath = currentProject.FullName
                'webConfigPath = webConfigPath.Substring(0, webConfigPath.LastIndexOf("\")) + "\Web.Config"
                webConfigPath = currentProject.ProjectItems.Item("Web.Config").Properties.Item("FullPath").Value.ToString()

                Dim xmlReader As XmlTextReader = New XmlTextReader(webConfigPath)

                ' Load into XML Dom
                xmlDoc.Load(xmlReader)
                xmlReader.Close()
            End If
        End If
        Return xmlDoc
    End Function
End Class



