Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Imports Microsoft.Win32              ' for sleep command

Public Class Form1
    Private myIni As goini
    Public startin1, startin2, startin3, FilePathUbix, FilePathClient, FilePathClientOut, removepath1, removepath2, removepath3, removepath4, removepath5, chktimer As String
    Public mydate As String
    Public mytime As String
    Public projdir As String
    Public mytimeend As String
    Public FileSizeClient as System.IO.FileInfo
    Public FileSizeUbix as System.IO.FileInfo
    Public nofileclient As Boolean = False
    Public co as Integer = chktimer
    Public variable As Integer = 1
 

    Private Sub Main()
        stuff.getargs  ()
        
      On Error goto errpart
            
   ' check needed files tu run relax 
        filechk("vars.xml")
        filechk("extract.exe")
        filechk("LisaCore.dll")
        filechk("LisaExtractor.dll")
        filechk("LisaExtractorApp.dll")
        filechk("LisaCoreWin.dll")
        
        projdir = My.Application.Info.DirectoryPath & "\"
        mydate = DateTime.Now.ToString("yyyy-MM-dd")
        mytime = DateTime.Now.ToString("HH:mm")
        lbltime.Text =    DateTime.Now.ToString("HH:mm:ss")                               
ex:
        
        Application.DoEvents ()



    '    if mytime = mytimeend then goto ex
        If mytime = startin1 Or mytime = startin2 Or mytime = startin3 then

                CreateObject("WScript.Shell").Popup("This program will copy and convert TOOSHEH TV DATA and Media to shared folder on network share path.RELAX Running at this location : " & projdir, 3, "Welcome to RELAX", 64)
                Directory.CreateDirectory(FilePathClient)         ' create ts folder in project dir 
                Thread.Sleep(1000)


            If chktsclient("ts") = False Then
     
                My.Computer.FileSystem.CopyDirectory(FilePathUbix,  txtfilepathclient.Text , showUI:=FileIO.UIOption.AllDialogs)

            Else

            End If
               Dim pathsubix() As String = IO.Directory.GetFiles(FilePathUbix, "*.ts")
            FileSizeUbix = My.Computer.FileSystem.GetFileInfo(pathsubix(0))            ' ' get TS file size on SERVER  

            CreateObject("WScript.Shell").Popup("file size on server = " & FileSizeUbix.Length, 2, "File already exist  ", 64)

            If chktssrv("ts") = False Then

                     CreateObject("WScript.Shell").Popup("No .TS file found in server to copy. ", 2, "Copy status ... ", 64)

              else

                CreateObject("WScript.Shell").Popup("All file(s) copied to client successfully.", 2, "Copy status ... ", 64)

            end if
            
            doextract ()
            
          


            '   Remove folders  ==============================================================================================================

            delfile( FilePathClientOut &  mydate & removepath1)
            delfile( FilePathClientOut & mydate & removepath2)
            delfile( FilePathClientOut & mydate & removepath3)
            delfile( FilePathClientOut & mydate & removepath4)
            delfile( FilePathClientOut & mydate & removepath5)

            '  Remove folders  ==============================================================================================================
            '      Directory.Delete (projdir  & FilePathClient ,True)

            delfile( FilePathClient)

            CreateObject("WScript.Shell").Popup(".TS file deleting ... , Operational successfully completed. ", 3, "RELAX Inform", 64)

            mytimeend = DateTime.Now.ToString("HH:mm")
             Thread.Sleep(chktimer)

  
        end if
        
errpart:


         Dim s As String
         Dim answ As String 
      '   s = "----------------------------------------" & vbCrLf
		s = s & Err.Number & " - "                                 '& Err.Source & vbCrLf
		s = s & Err.Description & vbCrLf
        s = s & "You can select cancel to terminate program or click Ok to continue. "
	'	s = s & "----------------------------------------"

       

        If Err.Number = 76 then
                stuff.mylog(ErrorToString)
                '   stuff.notify (5000,"test mikonim title asli injast","It looks like the problem is with the instantiation of your class; you've instantiated as Form1, when it should befrmCentsConverter; i.e. Dim frmConvert As New frmCentsConverter, instead of Dim frmConvert As New Form1. It could also be that you've renamed the start-up form of the a",Color.GreenYellow )
                msgbox(ErrorToString & " RELAX need correct path to access the TS file.Please set correct path in vars.xml or in setting tab.Please run relax again 1 min latter after starting time. RELAX now terminate.", vbCritical, "TS path not found")
                stuff.mylog(ErrorToString)
                 end

        end If

    If Err.Number <> 0 then
         '   MsgBox(Err.Number & " - " &  ErrorToString)

             answ =  MsgBox(s, MsgBoxStyle.ApplicationModal + MsgBoxStyle.Critical + MsgBoxStyle.OkCancel , Application.ProductName)
		 
              if answ = MsgBoxResult.Cancel  then End

    End If

        End Sub


    Function doextract ()

         Dim cmdProcess As New Process
         Dim cmdcommand 
        cmdcommand =  FilePathClientOut & mydate & " /ts  "  & FilePathClient
      
       With cmdProcess
             Dim procID As Integer 
' Run calculator.

  
  .StartInfo = New ProcessStartInfo("cmd.exe", " /c extract.exe " & cmdcommand)
    With .StartInfo
        .CreateNoWindow = false
        .UseShellExecute = true
    ' .RedirectStandardOutput = true 
              
    End With
           
    .Start() 
    .WaitForExit()
End With
        
     '   Dim cmdout As String = cmdProcess.StandardOutput.ReadToEnd
     '  cmdProcess.StandardOutput .ReadToEnd 
        ' stuff.mylog (cmdout)
 End Function


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        chkxml
        ContextMenuStrip1.Enabled = True
        me.Show()
        Label2.Text = Application.ProductVersion
        While 1
            Application.DoEvents()
         ' me.Refresh
            Thread.Sleep(100)
            Main()
        end While

    End Sub
    

   Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        getxml.wrtxml
    Form1_Load (e,e)

        End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        getxml.readxml

    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        'Cancel Closing:
        e.Cancel = True
        'Minimize the form:
        Me.WindowState = FormWindowState.Minimized
        'Don't show in the task bar
        Me.ShowInTaskbar = False
        'Enable the Context Menu Strip
        ContextMenuStrip1.Enabled = True
    End Sub

 

    Private Sub ShowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowToolStripMenuItem.Click
        'When Show menu clicks, it will show the form:
        Me.WindowState = FormWindowState.Normal
        'Show in the task bar:
        Me.ShowInTaskbar = True
        'Disable the Context Menu:
        ''  ContextMenuStrip1.Enabled = False
        me.Show()
        Me.TabControl1.SelectedTab = TabPage2
    End Sub

    Private Sub butTry_Click(sender As Object, e As EventArgs)
        me.Hide()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
         
      '  richtxtlog.LoadFile(projdir &"\logs\"&"ErrorLog_08-Dec-2016.log")
         Dim openFileDialog1 As New OpenFileDialog()
        openFileDialog1.InitialDirectory  =  Application.StartupPath & "\logs"
        openFileDialog1.Filter = "log Files|*.log"
        openFileDialog1.Title = "Select a log File"

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            ' Assign the cursor in the Stream to the Form's Cursor property.

            Dim extension = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("."))
            
              richtxtlog.Text = FileIO.FileSystem.ReadAllText(openFileDialog1.FileName)
        

        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
 
         If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
        txtfilepathubix.Text = FolderBrowserDialog1.SelectedPath
    End If

         
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        
         If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
        txtfilepathclient.Text = FolderBrowserDialog1.SelectedPath & "\temp"
    End If
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        
         If FolderBrowserDialog1.ShowDialog() = DialogResult.OK Then
        txtfilepathclientout.Text = FolderBrowserDialog1.SelectedPath
    End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles chkstartup.CheckedChanged
        if chkstartup.Checked=true then stuff.addreg 

        if chkstartup.Checked=false then stuff.removereg 
        
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        End
    End Sub

 

    Private Sub Form1_SizeChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.SizeChanged
        If Me.WindowState = FormWindowState.Minimized Then
            ShowInTaskbar = False
            ContextMenuStrip1.Enabled = True
        End If
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Me.WindowState = FormWindowState.Normal
        'Show in the task bar:
        Me.ShowInTaskbar = True

        me.Show()
        Me.TabControl1.SelectedTab = TabPage3

    End Sub

    Private Sub chkini()

        'Create ini object.
        myIni = New goini
        'File to process.
        myIni.Filename = My.Application.Info.DirectoryPath & "\vars.ini"         ' current ini path in relax app


        Dim oLst As New List(Of String)
        'Check if ini loaded.
        If myIni.LoadFromFile() Then
            'Read a value name.


            'read start times
            startin1 = myIni.ReadString("time", "startin1")
            startin2 = myIni.ReadString("time", "startin2")
            startin3 = myIni.ReadString("time", "startin3")


            'read paths
            FilePathUbix = myIni.ReadString("path", "FilePathUbix")
            FilePathClient = myIni.ReadString("path", "FilePathClient")
            FilePathClientOut = myIni.ReadString("path", "FilePathClientOut")



            'read removing files

            removepath1 = myIni.ReadString("remove", "path1")
            removepath2 = myIni.ReadString("remove", "path2")
            removepath3 = myIni.ReadString("remove", "path3")
            removepath4 = myIni.ReadString("remove", "path4")
            removepath5 = myIni.ReadString("remove", "path5")

            If removepath1 = "" Then removepath1 = "0"
            If removepath2 = "" Then removepath2 = "0"
            If removepath3 = "" Then removepath3 = "0"
            If removepath4 = "" Then removepath4 = "0"
            If removepath5 = "" Then removepath5 = "0"

            'read timers
            chktimer = myIni.ReadString("timers", "chktimer")



        End If

    End Sub

    Private sub chkxml()

        getxml.readxml

        'read start times
        startin1 = txtstartin1.Text
        startin2 = txtstartin2.Text
        startin3 = txtstartin3.Text
        
        'read paths
        FilePathUbix = txtfilepathubix.Text  
        FilePathClient = txtfilepathclient.Text &"\"
        FilePathClientOut = txtfilepathclientout.Text &"\"

        'read removing files

        removepath1 = txtremovepath1.Text
        removepath2 = txtremovepath2.Text
        removepath3 = txtremovepath3.Text
        removepath4 = txtremovepath4.Text
        removepath5 = txtremovepath5.Text

        If removepath1 = "" Then removepath1 = "0"
        If removepath2 = "" Then removepath2 = "0"
        If removepath3 = "" Then removepath3 = "0"
        If removepath4 = "" Then removepath4 = "0"
        If removepath5 = "" Then removepath5 = "0"

        'read timers
        chktimer = txtchktimer.Text

    End sub

    Private Function filechk(filename As String)
        projdir = My.Application.Info.DirectoryPath & "\" & filename
        If File.Exists(projdir) = False Then

            stuff.mylog("RELAX need some files to runing correctly but  " & projdir & " not found in current dir.Please copy this file to current project dir and run it again.RELAX terminated by now.File not found")
            MsgBox("RELAX need some files to runing correctly but  " & projdir & " not found in current dir.Please copy this file to current application  dir and run it again.RELAX terminated by now.", vbCritical, "Some file corrupted or not found")
           
            end
            
        End If
        projdir = ""
    End Function


    Private function chktsclient(ext As string)

        Dim tspath =  FilePathClient
        Dim paths() As String = IO.Directory.GetFiles(tspath, "*." & ext)
        If paths.Length > 0 Then
            chktsclient = True

        Else
            chktsclient = False
        end if
    End function


    Private function chktssrv(ext As string)

        Dim tspath = FilePathUbix
        Dim paths() As String = IO.Directory.GetFiles(tspath, "*." & ext)
        If paths.Length > 0 Then
            chktssrv = True            '      MsgBox ("file   fond")

        Else
            chktssrv = False      '    MsgBox ("file nis")
        end if
    End function



    Private Function delfile(fname As String)

        Try
            Directory.Delete(fname, True)

            Dim directoryExists = Directory.Exists(fname)
        Catch e As Exception

        End Try

        End Function

    Private Sub NotifyIcon1_MouseDown(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDown

      If e.Button = MouseButtons.Left then
            'When Show menu clicks, it will show the form:
               Me.Visible = True
            Me.WindowState = FormWindowState.Normal
            'Show in the task bar:
     
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
       'First minimize the form
         Me.WindowState = FormWindowState.Minimized

        'Now make it invisible (make it look like it went into the system tray)
        Me.Visible = False
    End Sub

    Private Sub TabControl1_MouseClick(sender As Object, e As MouseEventArgs) Handles TabControl1.MouseClick
        dim full As String 

        stuff.readreg()
        if lblstartuppath.Text =   ""  then 
            chkstartup.Checked =False
            else
            chkstartup.Checked =True
        End If

        



    End Sub

    Private Sub txtchktimer_Leave(sender As Object, e As EventArgs) Handles txtchktimer.Leave
         dim vtxtchk as Integer 
        vtxtchk = val ( txtchktimer.Text)
        if vtxtchk < 10000 then  txtchktimer.Text ="20000"
    End Sub
End Class
