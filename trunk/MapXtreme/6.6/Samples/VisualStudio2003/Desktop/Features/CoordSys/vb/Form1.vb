Imports MapInfo.Engine

Public Class Form1
    Inherits System.Windows.Forms.Form
    Private miCommand As MapInfo.Data.MICommand

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        Dim prjfile As String

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.components = New System.ComponentModel.Container
        Session.Current.CoordSysFactory.LoadDefaultProjectionFile()

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents TestCoordSys As System.Windows.Forms.Button
    Friend WithEvents outputTextBox As System.Windows.Forms.RichTextBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.ExitButton = New System.Windows.Forms.Button
        Me.TestCoordSys = New System.Windows.Forms.Button
        Me.outputTextBox = New System.Windows.Forms.RichTextBox
        Me.SuspendLayout()
        '
        'ExitButton
        '
        Me.ExitButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExitButton.Location = New System.Drawing.Point(648, 408)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(88, 32)
        Me.ExitButton.TabIndex = 0
        Me.ExitButton.Text = "Exit"
        '
        'TestCoordSys
        '
        Me.TestCoordSys.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TestCoordSys.Location = New System.Drawing.Point(520, 408)
        Me.TestCoordSys.Name = "TestCoordSys"
        Me.TestCoordSys.Size = New System.Drawing.Size(104, 32)
        Me.TestCoordSys.TabIndex = 1
        Me.TestCoordSys.Text = "Test CoordSys"
        '
        'outputTextBox
        '
        Me.outputTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.outputTextBox.Font = New System.Drawing.Font("Courier New", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.outputTextBox.Location = New System.Drawing.Point(8, 8)
        Me.outputTextBox.Name = "outputTextBox"
        Me.outputTextBox.Size = New System.Drawing.Size(728, 384)
        Me.outputTextBox.TabIndex = 2
        Me.outputTextBox.Text = ""
        Me.outputTextBox.WordWrap = False
        '
        'Form1
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(752, 454)
        Me.Controls.Add(Me.outputTextBox)
        Me.Controls.Add(Me.TestCoordSys)
        Me.Controls.Add(Me.ExitButton)
        Me.Name = "Form1"
        Me.Text = "Form1"
        Me.ResumeLayout(False)

    End Sub

#End Region


    Private Sub EnumCoordSysFactory()
        Dim coordSysEnum As MapInfo.Geometry.CoordSysInfoEnumerator

        coordSysEnum = Session.Current.CoordSysFactory.GetCoordSysInfoEnumerator()

        coordSysEnum.Reset()
        outputTextBox.AppendText("Enumerate CoordSysFactory:" + Chr(10))
        While (coordSysEnum.MoveNext())
            outputTextBox.AppendText(Chr(9) + coordSysEnum.Current.CoordSys.MapBasicString + Chr(10))
        End While
    End Sub


    Private Sub ExitButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitButton.Click
        Application.Exit()
    End Sub
    Private Sub CreateCoordSys()
        Dim csys As MapInfo.Geometry.CoordSys

        csys = Session.Current.CoordSysFactory.CreateFromPrjString("1, 56")
        Me.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + Chr(10))
        csys = Session.Current.CoordSysFactory.CreateFromPrjString(Chr(34) + "test\\p45678" + Chr(34) + ", 1, 56" + Chr(34))
        Me.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + Chr(10))
        csys = Session.Current.CoordSysFactory.CreateLongLat(MapInfo.Geometry.DatumID.AstroBeaconE)
        Me.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + Chr(10))
        csys = Session.Current.CoordSysFactory.CreateFromPrjString("30, 47, 7, 103.853, 1.287639, 30000, 30000")
        Me.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + Chr(10))
        csys = Session.Current.CoordSysFactory.CreateFromPrjString("30, 1000, 7, 13.62720367, 52.41864828, 40000, 10000")
        Me.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + Chr(10))
        csys = Session.Current.CoordSysFactory.CreateFromMapBasicString("CoordSys Earth Projection 4, 62, ""m"", 0, 90, 90")
        Me.outputTextBox.AppendText("Created CoordSys: " + csys.MapBasicString + Chr(10))
    End Sub

    ' We use the Math.Round method since none of the transforms are exact.
    Private Sub ComparePoints(ByVal msg As String, ByVal p1 As MapInfo.Geometry.DPoint, ByVal p2 As MapInfo.Geometry.DPoint)
        Dim prec As Integer

        prec = 5
        If Math.Round(p1.x, prec) = Math.Round(p2.x, prec) And Math.Round(p1.y, prec) = Math.Round(p2.y, prec) Then
            outputTextBox.AppendText(msg + " points are equal: (" + Math.Round(p1.x, prec).ToString() + ", " + Math.Round(p1.y, prec).ToString() + ") == (" + Math.Round(p2.x, prec).ToString() + ", " + Math.Round(p2.y, prec).ToString() + ")" + Chr(10))
        Else
            outputTextBox.AppendText(msg + " points are NOT equal: (" + Math.Round(p1.x, prec).ToString() + ", " + Math.Round(p1.y, prec).ToString() + ") != (" + Math.Round(p2.x, prec).ToString() + ", " + Math.Round(p2.y, prec).ToString() + ")" + Chr(10))
        End If
    End Sub


    ' We are going to use the CoordSys transform to transform a point from one coordsys to another then back
    ' again to see if the round trip produces the correct results. Then test the array version of the transform.
    Private Sub UseCoordinateTransform()
        ' create LongLat projection
        Dim csys As MapInfo.Geometry.CoordSys
        Dim csys1 As MapInfo.Geometry.CoordSys
        Dim pntSrc, pntDest, pntSrc1, pntDest1, pntBackToSrc, pnt(2) As MapInfo.Geometry.DPoint
        Dim coordTransform As MapInfo.Geometry.CoordinateTransform

        csys = Session.Current.CoordSysFactory.CreateFromPrjString("1, 56")
        ' Create Robinson projection
        csys1 = Session.Current.CoordSysFactory.CreateFromPrjString("12, 62, 7, 0")
        If csys.Equals(csys1) Then
            outputTextBox.AppendText("Oops, coordsys's are equal, this is bad\n")
        End If
        coordTransform = Session.Current.CoordSysFactory.CreateCoordinateTransform(csys, csys1)
        pntSrc = New MapInfo.Geometry.DPoint(0, 0)
        pntDest = coordTransform.CoordSys1ToCoordSys2(pntSrc)
        pntSrc1 = New MapInfo.Geometry.DPoint(1, 1)
        pntDest1 = coordTransform.CoordSys1ToCoordSys2(pntSrc1)
        pntBackToSrc = coordTransform.CoordSys2ToCoordSys1(pntDest1)
        ComparePoints("Round trip", pntSrc1, pntBackToSrc)
        ' compare the result with arrays
        pnt(0).x = 0
        pnt(0).y = 0
        pnt(1).x = 1
        pnt(1).y = 1
        coordTransform.CoordSys1ToCoordSys2(pnt, pnt)
        ComparePoints("Array converted", pntDest, pnt(0))
        ComparePoints("Array converted", pntDest1, pnt(1))
    End Sub


    Private Sub TestCoordSys_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TestCoordSys.Click
        CreateCoordSys()
        UseCoordinateTransform()
        EnumCoordSysFactory()
    End Sub
End Class
