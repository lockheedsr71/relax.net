Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Threading
Imports Microsoft.Win32              ' for sleep command



Public Class frmMain
    Private myIni As goini
    Public startin1, startin2, startin3, FilePathUbix, FilePathClient, FilePathClientOut, removepath1, removepath2, _
        removepath3, removepath4, removepath5, Removeemptydir, chktimer, ExtractOnFly ,updserver As String
    Public mydate As String
    Public mytime As String
    Public projdir As String
    Public mytimeend As String
    Public FileSizeClient As System.IO.FileInfo
    Public FileSizeUbix As System.IO.FileInfo
    Public nofileclient As Boolean = False
    Public co As Integer = chktimer
    Public variable As Integer = 1

    '---------- updater dimintions -----------
    Dim uriSource As Uri
    Dim downloading As New Net.WebClient
    Dim timerID As IntPtr = 0
    Dim downloadSpeed As Integer = 0
    Dim currBytes As Long
    Dim prevBytes As Long
    Dim startTime As Long
    Dim elapsedTime As TimeSpan
    Dim downloadStarted As Boolean = False
     Private cbindex As Integer
   '  Dim location = Assembly.GetExecutingAssembly().Location
     Dim locationdir =Directory.GetCurrentDirectory()

    

    Private Sub Main()

start:
        if stuff.getcmdargs = false then stuff.getargs()

     '  On Error GoTo errpart

        ' check needed files tu run relax 
        filechk("vars.xml")
        filechk("extract.exe")
        filechk("LisaCore.dll")
        filechk("LisaExtractor.dll")
        filechk("LisaExtractorApp.dll")
        filechk("LisaCoreWin.dll")
        filechk("updater.exe")


        projdir = My.Application.Info.DirectoryPath & "\"
        mydate = DateTime.Now.ToString("yyyy-MM-dd")
        mytime = DateTime.Now.ToString("HH:mm")
        lbltime.Text = DateTime.Now.ToString("HH:mm:ss")
        Application.DoEvents()




        '    if mytime = mytimeend then goto ex
        If mytime = startin1 Or mytime = startin2 Or mytime = startin3 Then

            '''  CreateObject("WScript.Shell").Popup("This program will copy and convert TOOSHEH TV DATA and Media to shared folder on network share path.RELAX Running at this location : " & projdir, 3, "Welcome to RELAX", 64)

            stuff.notify(4000, "Starting time detected", "This program will copy and convert TOOSHEH TV DATA and Media to shared folder on network share path.RELAX Running at this location : " & projdir, Color.Yellow, Color.DarkBlue)

            Directory.CreateDirectory(FilePathClient)         ' create ts folder in project dir 
            Thread.Sleep(1000)



            If Directory.Exists(FilePathUbix) Then


                If chktsclient("ts") = False Then                  ' if TS file alrady not copied to destination then ... 

                    If ExtractOnFly = 1 Then

                        FilePathClient = Chr(34) & FilePathUbix & Chr(34)

                        doextract()
                        GoTo nocopy
                    End If
                    My.Computer.FileSystem.CopyDirectory(FilePathUbix, txtfilepathclient.Text, showUI:=FileIO.UIOption.AllDialogs)


                Else
                End If


                Dim pathsubix() As String = IO.Directory.GetFiles(FilePathUbix, "*.ts")
                FileSizeUbix = My.Computer.FileSystem.GetFileInfo(pathsubix(0))            ' ' get TS file size on SERVER  

                CreateObject("WScript.Shell").Popup("file size on server = " & FileSizeUbix.Length, 2, "File already exist  ", 64)

                If chktssrv("ts") = False Then

                    stuff.notify(4000, "Copy status", "No .TS file found in server to copy. ", Color.Yellow, Color.DarkBlue)
                Else

                    stuff.notify(4000, "Copy status", "All file(s) copied to client successfully.wait to extracting ...", Color.Yellow, Color.DarkBlue)
                End If

            Else

                stuff.notify(4000, "TS Source not found ", "Source file not detected on source path.RELAX waiting for it in next proper time ...", Color.Yellow, Color.DarkBlue)


                Thread.Sleep(chktimer)
                GoTo start

            End If


            doextract()
          
          
         
         
nocopy:

            '   Remove folders  ==============================================================================================================

            delfile(FilePathClientOut & mydate & removepath1)
            delfile(FilePathClientOut & mydate & removepath2)
            delfile(FilePathClientOut & mydate & removepath3)
            delfile(FilePathClientOut & mydate & removepath4)
            delfile(FilePathClientOut & mydate & removepath5)
           

            if Removeemptydir=1 Then
                 Thread.Sleep (1000)
                stuff.DeleteEmptyFolder(FilePathClientOut & mydate )
            End If    
            
            

            '  Remove extentions  ==============================================================================================================

            stuff.removefilebyext(FilePathClientOut & mydate)               ' remove all extentions define in main form


            ''      If RemoveTS = 1 then 
            If ExtractOnFly = 1 Then
                GoTo nodelete

                delfile(FilePathClient)
            End If


            stuff.notify(4000, ".TS file deleting ... ", "Operational successfully completed.", Color.Yellow, Color.DarkBlue)
nodelete:

            mytimeend = DateTime.Now.ToString("HH:mm")

            Thread.Sleep(chktimer)
            Application.DoEvents


        End If



errpart:

        
        Dim answ As String
        '   s = "----------------------------------------" & vbCrLf
        Dim s As String = s & Err.Number & " - "                                 '& Err.Source & vbCrLf
        s = s & Err.Description & vbCrLf
        s = s & "You can select cancel to terminate program or click Ok to continue. "
        '	s = s & "----------------------------------------"



        If Err.Number = 76 Then
            stuff.mylog(ErrorToString)



            MsgBox(ErrorToString & " RELAX need correct path to access the TS file.Please set correct path in vars.xml or in setting tab.Please run relax again 1 min latter after starting time. RELAX now terminate.", vbCritical, "TS path not found")
            stuff.mylog(ErrorToString)
            End

        End If

        If Err.Number <> 0 Then
            '   MsgBox(Err.Number & " - " &  ErrorToString)
            stuff.mylog(ErrorToString)
            answ = MsgBox(s, MsgBoxStyle.ApplicationModal + MsgBoxStyle.Critical + MsgBoxStyle.OkCancel, Application.ProductName)

            If answ = MsgBoxResult.Cancel Then End

        End If

    End Sub
     

    Function doextract()

        Dim cmdProcess As New Process
        Dim cmdcommand


        cmdcommand = FilePathClientOut & mydate & " /ts " & FilePathClient



        With cmdProcess
            Dim procID As Integer
            ' Run calculator.


            .StartInfo = New ProcessStartInfo("cmd.exe", " /c extract.exe " & cmdcommand)
            With .StartInfo
                .CreateNoWindow = False
                .UseShellExecute = True
                ' .RedirectStandardOutput = true 

            End With

            .Start()

            .WaitForExit()
        End With

        If ExtractOnFly = 1 Then FilePathClient = txtfilepathclient.Text

        try
           delfile (  txtfilepathclient.Text)

        Catch ex As Exception

        End Try


        '   Dim cmdout As String = cmdProcess.StandardOutput.ReadToEnd
        '  cmdProcess.StandardOutput .ReadToEnd 
        ' stuff.mylog (cmdout)
    End Function


    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Try
            '
            'Setup the needed event handlers for getting download status, ect..
            setupEventHandlers()

        Catch exc As Exception

            MessageBox.Show(exc.Message, "  Info!", MessageBoxButtons.OK, _
                 MessageBoxIcon.Information)

        End Try

        chkxml
     '   ContextMenuStrip1.Enabled = True
      '  Me.Show()
        '---------------------------------

      Me.Show()
        NotifyIcon1.BalloonTipText = "Application Minimized.Double click to bring on top."
         NotifyIcon1.BalloonTipTitle = "RELAX Engine"




        '--------------------------------
        Label2.Text = Application.ProductVersion
        While 1
            Application.DoEvents()
       
            Thread.Sleep(100)
            Main()
        End While

      '  stuff.zip()


    End Sub


    Private Sub Button3_Click_1(sender As Object, e As EventArgs) Handles Button3.Click
        getxml.wrtxml
        stuff.notify(3000, "Saving ...", "Configuration saved successfully.", Color.Yellow, Color.DarkBlue)
        Form1_Load(e, e)


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
        Me.Show()
        Me.TabControl1.SelectedTab = TabPage2
    End Sub

    Private Sub butTry_Click(sender As Object, e As EventArgs)
        Me.Hide()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click

        '  richtxtlog.LoadFile(projdir &"\logs\"&"ErrorLog_08-Dec-2016.log")
        Dim openFileDialog1 As New OpenFileDialog()
        openFileDialog1.InitialDirectory = Application.StartupPath & "\logs"
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
        If chkstartup.Checked = True Then stuff.addreg

        If chkstartup.Checked = False Then stuff.removereg

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



    Private Sub Button9_Click(sender As Object, e As EventArgs)
        getxml.wrtxml
    End Sub

  Private Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        chkxml
        txtstartin3.Text = DateTime.Now.ToString("HH:mm")
        startin3 = DateTime.Now.ToString("HH:mm")
        stuff.notify(3000, "Starting manually...", "Program starting manually now ... ", Color.Cyan, Color.DarkBlue)
        Thread.Sleep(1000)
        Call Main()
        startin3 = 0
        txtstartin3.Text = "N/A"
    End Sub

    Private Sub btnDownload_Click(sender As Object, e As EventArgs) Handles btnDownload.Click

        Dim updlink As String = updsrvlist.SelectedItem  & "/version.xml"& "?random=" & hashing.rndhash
        dim tmpdir As String = Path.GetTempPath() 
         My.Computer.Network.DownloadFile(updlink, tmpdir &  "/version.xml", "", "", True, 3000, True)
        clsver.rxml(tmpdir & "version.xml")
        Dim location = Assembly.GetExecutingAssembly().Location
        clsver.comparever(clsver.pver, clsver.getver(location))
        lblupdcurrentver.Text = clsver.getver(location)
        lblupdid.Text = clsver.pID
        lblupdprogname.Text = clsver.pName
        lblupdveronserver.Text = clsver.pver
        lbllisacorever0.Text=clsver.getver ( "o:\MY GIT\relax.net\Relax\bin\Release\LisaExtractor.dll")
        'lblupddesc.Text=clsver.pdes
        txtupddesc.Text = clsver.pdes
        lblupdlink.Text = updlink
        Select Case clsver.result
            Case 1
                lblupdstatus.Text = "There is a new release on the update server.You can install it by click on Update now button."
                btnOpenDownload.Enabled=True
              ' The following is the only Case clause that evaluates to True.
            Case 0
                lblupdstatus.Text = "There is no new update available. the installed version is the last version."
                 btnOpenDownload.Enabled=false
            Case -1
                lblupdstatus.Text = "It seems your installed version is newer than on this update server.You can check update steps by changing your update server and try again.or you have the last version"
                    btnOpenDownload.Enabled=false
            Case Else
                 btnOpenDownload.Enabled=false
                lblupdstatus.Text = "There is a problem to detect updete version.Send an email to developer."
        End Select

    End Sub

    Private Sub btnOpenDownload_Click(sender As Object, e As EventArgs) Handles btnOpenDownload.Click
        Try
            '
            'Setup a new Uniform Resource Identifier. I originally had this in the DoDownload method
            'but decided to put it hereso it wouldn't process anymore code then it has to if the url 
            'is invalid.
        
            uriSource = New Uri(updserver & "/source.zip?random=" & hashing.rndhash)
            '
            'Starts a Windows timer to tick every one second and gets the id which will be used when 
            'you want to kill the timer.
            timerID = SystemEvents.CreateTimer((1000))
            '
            'This will call the sub to get the downloaded started.
            doDownload() 'txtDownloadAddress.Text, txtSaveAddress.Text)
            '
            'This will hold the initial value for display the download elapsed time. Tickcount uses less
            'resources than the stopwatch class although it has a much lower resolution of abour 15-16ms.
            startTime = Now.Ticks
            '
            'Clear all values.
            currBytes = 0
            prevBytes = 0
            downloadSpeed = 0
            pbDownloadProgress.Value = 0
            lblStatus.Text = "Status: Started"

        Catch exc As Exception

            MessageBox.Show(exc.Message, "  Download Button!", MessageBoxButtons.OK, _
                 MessageBoxIcon.Information)

        End Try

       

     
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        '
        'Try to stop the download gracefully.
        '
        Try
            '
            'First make sure the timerID has a address to a timer before calling KillTimer.
            If timerID.ToInt32 > 0 Then
                '
                'Now kill the timer passing the ID of the timer.
                SystemEvents.KillTimer(timerID)
                timerID = Nothing

            End If
            '
            'Tell the Download operation to stop the download.
            downloading.CancelAsync()

        Catch exc As Exception

            MessageBox.Show(exc.Message, "  Info!", MessageBoxButtons.OK, _
                 MessageBoxIcon.Information)

        End Try
    End Sub

    

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Me.WindowState = FormWindowState.Normal
        'Show in the task bar:
        Me.ShowInTaskbar = True

        Me.Show()
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

    Private Sub chkxml()

        getxml.readxml

        'read start times
        startin1 = txtstartin1.Text
        startin2 = txtstartin2.Text
        startin3 = txtstartin3.Text

        'read paths
        FilePathUbix = txtfilepathubix.Text
        FilePathClient = txtfilepathclient.Text
        FilePathClientOut = txtfilepathclientout.Text & "\"

        'read removing files

        removepath1 = txtremovepath1.Text
        removepath2 = txtremovepath2.Text
        removepath3 = txtremovepath3.Text
        removepath4 = txtremovepath4.Text
        removepath5 = txtremovepath5.Text
        Removeemptydir = chkRemoveemptydir .CheckState 
        ExtractOnFly = chkextractonfly.CheckState


        If removepath1 = "" Then removepath1 = "0"
        If removepath2 = "" Then removepath2 = "0"
        If removepath3 = "" Then removepath3 = "0"
        If removepath4 = "" Then removepath4 = "0"
        If removepath5 = "" Then removepath5 = "0"

        'read timers
        chktimer = txtchktimer.Text
        updserver = updsrvlist.SelectedItem  ()
      

    End Sub

    Private Function filechk(filename As String)
        projdir = My.Application.Info.DirectoryPath & "\" & filename
        If File.Exists(projdir) = False Then

            stuff.mylog("RELAX need some files to runing correctly but  " & projdir & " not found in current dir.Please copy this file to current project dir and run it again.RELAX terminated by now.File not found")
            MsgBox("RELAX need some files to runing correctly but  " & projdir & " not found in current dir.Please copy this file to current application  dir and run it again.RELAX terminated by now.", vbCritical, "Some file corrupted or not found")

            End

        End If
        projdir = ""
    End Function

   

    Private Function chktsclient(ext As String)

        Dim tspath = FilePathClient
        Dim paths() As String = IO.Directory.GetFiles(tspath, "*." & ext)
        If paths.Length > 0 Then
            chktsclient = True

        Else
            chktsclient = False
        End If
    End Function

   
    Private Sub chkextractonfly_MouseClick(sender As Object, e As MouseEventArgs) Handles chkextractonfly.MouseClick
          getxml.wrtxml()
    End Sub

  
    Private Function chktssrv(ext As String)

        Dim tspath = FilePathUbix
        Dim paths() As String = IO.Directory.GetFiles(tspath, "*." & ext)
        If paths.Length > 0 Then
            chktssrv = True            '      MsgBox ("file   fond")

        Else
            chktssrv = False      '    MsgBox ("file nis")
        End If
    End Function



    Private Function delfile(fname As String)

        Try
            Directory.Delete(fname, True)

            Dim directoryExists = Directory.Exists(fname)
        Catch e As Exception

        End Try

    End Function

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
         ShowInTaskbar = True
    NotifyIcon1.Visible = False
    WindowState = FormWindowState.Normal
    End Sub

    Private Sub chkRemoveemptydir_MouseClick(sender As Object, e As MouseEventArgs) Handles chkRemoveemptydir.MouseClick
           getxml.wrtxml()
    End Sub


    Private Sub NotifyIcon1_MouseDown(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDown

        If e.Button = MouseButtons.Left Then
            'When Show menu clicks, it will show the form:
            if me.Visible= False Then 
            Me.Visible = True
            Me.WindowState = FormWindowState.Normal
                me .TopMost = True

                End If
            else 
            Me.Visible = False
              Me.WindowState = FormWindowState.normal

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
        Dim full As String

      '  getxml.wrtxml()
        getxml.readxml()


        stuff.readreg()
        If lblstartuppath.Text = "" Then
            chkstartup.Checked = False
        Else
            chkstartup.Checked = True
        End If

          Dim appPath As String = Application.StartupPath()

           try
                    lblextractorexever.Text = clsver.getver (appPath & "\extract.exe")
                   lblrelaxver.Text = clsver.getver (appPath & "\relax.exe")
                   lbllisacorever.Text = clsver.getver (appPath & "\lisacore.dll")
                   lbllisacorewinver.Text = clsver.getver (appPath & "\lisacorewin.dll")
                   lbllisaextractorver.Text = clsver.getver (appPath & "\lisaextractor.dll")
                    lblupdaterver.Text=  clsver.getver (appPath & "\updater.exe")

           Catch ex As Exception
            stuff.mylog (ex.ToString())
         MsgBox ( "Some files can't be found to export it's version.For more informations see the log.",vbCritical , "Version error")

        End Try
            



    End Sub

    Private Sub txtchktimer_Leave(sender As Object, e As EventArgs) Handles txtchktimer.Leave
        Dim vtxtchk As Integer
        vtxtchk = Val(txtchktimer.Text)
        If vtxtchk < 10000 Then txtchktimer.Text = "20000"
    End Sub

    Private Sub txtfilepathubix_LostFocus(sender As Object, e As EventArgs) Handles txtfilepathubix.LostFocus

        Dim lastchar As String
        lastchar = Microsoft.VisualBasic.Right(txtfilepathubix.Text, 1)

        If txtfilepathubix.Text.Length <> 0 Then
            If lastchar = "\" Then
                txtfilepathubix.Text = txtfilepathubix.Text.Substring(0, txtfilepathubix.Text.Length - 1)
            End If

        End If


    End Sub

    '
    'This Sub contains the main download code. Not really any advantage to this now that I moved the 
    'uri and other code that was originially in this sub.
    Sub doDownload() '(ByVal sourceAddress As String, ByVal destLocation As String)

        Try
            '
            '
            'By using the Async version of DownloadFile your applications thread won't be focused on the 
            'download and thus you can still interact with your application. The only way to get that 
            'feature with the regular DownloadFile method is by running it in a seperate thread which is 
            'really not really worth doing since an async version is availble.
            Dim appPath As String = Application.StartupPath() & "\source.zip"
            downloading.DownloadFileAsync(uriSource, appPath)
            '
            downloadStarted = True

            btnCancel.Enabled = True
            btnDownload.Enabled = False

        Catch exc As Exception

            downloadStarted = False

            MessageBox.Show(exc.Message, "  Info!", MessageBoxButtons.OK, _
                 MessageBoxIcon.Information)

        End Try


    End Sub

    'This sub will setup the event handlers for the program.
    Sub setupEventHandlers()

        Try
            '
            'The getDownloadProgress Sub will fire whenever the DownloadAsync method updates the file 
            'download status.
            AddHandler downloading.DownloadProgressChanged, AddressOf getDownloadProgress
            AddHandler downloading.DownloadFileCompleted, AddressOf downloadHasEnded
            '
            'This will setup the handler for the api timer.
            AddHandler SystemEvents.TimerElapsed, AddressOf downloadUpdating

        Catch exc As Exception

            MessageBox.Show(exc.Message, "  Info!", MessageBoxButtons.OK, _
                 MessageBoxIcon.Information)

        End Try

    End Sub

    Sub getDownloadProgress(ByVal sender As Object, ByVal e As System.Net.DownloadProgressChangedEventArgs)
        '
        Try
            '
            'Update the progress bar and percentage label.
            pbDownloadProgress.Value = e.ProgressPercentage
            lblProgress.Text = "Progress: " & e.ProgressPercentage.ToString & "%"
            '
            'I went back and forth on whether to define a kilo-byte as 1000 bytes or 1024 bytes. I 
            'did some research and came up with download based bytes says 1KB = 1000bytes. But when 
            'it comes to files or storage then 1KB = 1024bytes. Anyways, I just went ahead and set 
            '1 KB to equal 1000 bytes. Even though I personally feel all KB's whether packet based 
            'or file based should be 1024.
            lblDownloadBytes.Text = "Downloaded: " & FormatNumber(e.BytesReceived / 1024, 2).ToString & " KB"
            lblDownloadSize.Text = "Download Size: " & FormatNumber(e.TotalBytesToReceive / 1024, 2).ToString & " KB"
            '
            'Keep this updated with the latest value. Used for calculating the download speed.
            currBytes = e.BytesReceived
            '
        Catch exc As Exception

            MessageBox.Show(exc.Message, "  Info!", MessageBoxButtons.OK, _
                 MessageBoxIcon.Information)

        End Try

    End Sub

    '
    'Will will fire when the download has ended.
    Sub downloadHasEnded(ByVal sender As Object, ByVal e As System.ComponentModel.AsyncCompletedEventArgs)

        Try
            '
            'Display the download has finished or was canceled.
            '
            If e.Cancelled Then
                '
                'Show that the download had been canceled.
                lblStatus.Text = "Status: Canceled"
                btnOpenDownload.Enabled = False

            Else

                'Show that the download finished gracefully.
                lblStatus.Text = "Status: Finished!"
                btnOpenDownload.Enabled = True
              
          

                
            End If
            '
            btnCancel.Enabled = False
            btnDownload.Enabled = True
          stuff.startupdate()


            'First make sure the timerID has a address to a timer before calling KillTimer.
            If timerID.ToInt32 > 0 Then
                '
                'Now kill the timer passing the ID of the timer.
                SystemEvents.KillTimer(timerID)
                timerID = Nothing

            End If

        Catch exc As Exception

            MessageBox.Show(e.Error.Message, "  Info!", MessageBoxButtons.OK, _
                 MessageBoxIcon.Information)

        End Try


         


    End Sub

    '
    'This method will be called when the TimerElapsed event for the API timer is executed.
    Sub downloadUpdating(ByVal sender As Object, ByVal e As Microsoft.Win32.TimerElapsedEventArgs)
        '
        'The code below will calculate the approximate speed the download is running at. Unfortunately
        'the Webclient class doesn't have a feature for this. Thats why I came up with me own way. :)
        'My testing does show its not to far off from the real download speed.
        '
        Try
            '
            'This will display the time that has elapsed since the download started.
            elapsedTime = TimeSpan.FromTicks((Now.Ticks - startTime))
            '
            'I decided to use this custom code to format the timespan to not include 6 or more 
            'numbers past the decimal. Big oversite by Microsoft not having custom format options 
            'available for the TimeSpan class. Unless I overlooked it somewhere? I got this code from
            'a post in a question forum. Not sure the exact place though.
            lblElapsedTime.Text = "Elapsed Time: " & String.Format("{0:00}:{1:00}:{2:00}", _
                elapsedTime.TotalHours, elapsedTime.Minutes, elapsedTime.Seconds)
            '
            'This contains the total bytes that have been processed since the last time the value was 
            'checked about 1 second ago. Thus this will give you the approximate download speed in 
            'Kilo-Bytes per second. :)
            downloadSpeed = (currBytes - prevBytes)
            '
            'This will display the Kilo-BYTEs speed. I divide by 1024 to convert to the Kilo-Bytes value.
            'I've been back and forth between using 1000 bytes and 1024 bytes as a Kilo-Byte. I've 
            'looked at various info and I still don't know what I will eventually leave it at yet.
            lblSpeedKBytes.Text = "Speed: " & FormatNumber(downloadSpeed / 1000, 2).ToString & " KB/s"
            '
            'This will display the Kilo-BITS speed which some people like to see. Since their is 8 Bits to
            'every byte you will want to multiply the bytes value by 8. ISPs love using Bit to advertise
            'their internet speeds since it is a much higher number.
            lblSpeedKBits.Text = "Speed: " & FormatNumber((downloadSpeed / 1000) * 8, 2).ToString & " Kb/s"
            '
            'Keep the previous value to recalulate the speed when the timer reaches 1 second again.
            prevBytes = currBytes

        Catch exc As Exception

            MessageBox.Show(exc.Message, "  Info!", MessageBoxButtons.OK, _
                 MessageBoxIcon.Information)

        End Try

    End Sub

 

    Private Sub updsrvlist_LostFocus(sender As Object, e As EventArgs) Handles updsrvlist.LostFocus
          updsrvlist.Items(cbindex) = updsrvlist.Text
        getxml.wrtxml()
    End Sub
    
    Private Sub updsrvlist_SelectedIndexChanged(sender As Object, e As EventArgs) Handles updsrvlist.SelectedIndexChanged
          cbindex = updsrvlist .SelectedIndex
            btnOpenDownload.Enabled=false
    End Sub

    Private Sub frmMain_RegionChanged(sender As Object, e As EventArgs) Handles Me.RegionChanged

    End Sub

    Private Sub frmMain_Resize(sender As Object, e As EventArgs) Handles Me.Resize
         If WindowState = FormWindowState.Minimized Then
        ShowInTaskbar = False
        NotifyIcon1.Visible = True
        NotifyIcon1.ShowBalloonTip(1000)
    End If
    End Sub
End Class
