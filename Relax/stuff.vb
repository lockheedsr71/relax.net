Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Threading
Imports Microsoft.Win32
  
Public Class stuff

   public  Shared   getcmdargs as Boolean 
   Public Shared   _listAllDirectories As New List(Of String)


    Public Function CompareFiles(ByVal file1FullPath As String, ByVal file2FullPath As String) As Boolean

        If Not File.Exists(file1FullPath) Or Not File.Exists(file2FullPath) Then
            'One or both of the files does not exist.
            MsgBox ("file nist")
            Return False
        End If

        If file1FullPath = file2FullPath Then
            ' fileFullPath1 and fileFullPath2 points to the same file...
            MsgBox ("file MASIR YEKIE ")
            Return True
        End If

        Try
            Dim file1Hash As String = hashFile(file1FullPath)
            Dim file2Hash As String = hashFile(file2FullPath)

            If file1Hash = file2Hash Then
                  MsgBox ("file hash yekist ")
                Return True
            Else
                  MsgBox ("file motavaet ast ")
                Return False
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

    Public  Function hashFile(ByVal filepath As String) As String
        Using reader As New System.IO.FileStream(filepath, IO.FileMode.Open, IO.FileAccess.Read)
            Using md5 As New System.Security.Cryptography.MD5CryptoServiceProvider
                Dim hash() As Byte = md5.ComputeHash(reader)
                Return System.Text.Encoding.Unicode.GetString(hash)
            End Using
        End Using
    End Function

    
 Public Shared Sub DumpLog(r As StreamReader)
        Dim line As String
        line = r.ReadLine()
        While Not (line Is Nothing)
            Console.WriteLine(line)
            line = r.ReadLine()
        End While
    End Sub

     


  Public Shared sub mylog (logtxt As String)
    
      dim logdir =  My.Application.Info.DirectoryPath
          Directory.CreateDirectory (logdir & "\logs")
          Thread.Sleep(1000)
      Dim strFile As String = logdir &  "\logs\ErrorLog_" & DateTime.Today.ToString("dd-MMM-yyyy") & ".log"
      Dim sw As StreamWriter


            Try
               If (Not File.Exists(strFile)) Then
                  sw = File.CreateText(strFile)
                  sw.WriteLine("Start Error Log for today")
               Else
                  sw = File.AppendText(strFile)
               End If
               sw.WriteLine( DateTime.Now & " Err# [" & Err.Number & "].Des --> "  & logtxt & vbCrLf & "------------------------------------------------------------------------------------" )
               sw.Close()
            Catch ex As IOException
               MsgBox("Error writing to log file.")
            End Try


    End sub

    
       
   public Shared  sub ttip (title As String,txt As String  ,delay as integer    )

        
           frmMain.   notifyIcon1.BalloonTipIcon = ToolTipIcon.Error
           frmMain.  notifyIcon1.BalloonTipTitle = title
           frmMain. notifyIcon1.BalloonTipText = txt
           frmMain. notifyIcon1.ShowBalloonTip(delay)
     

    End sub
    public Shared  sub ttip2  (txt As String )

        frmMain.    NotifyIcon1.BalloonTipText = txt
      frmMain.   NotifyIcon1.ShowBalloonTip(8000)
      
        End sub

   
    public Shared sub notify ( tmr As Integer , txt1  As String , txt2 As String,clrtxt As color ,clrback As color)
 
        Dim Notification As New frmNotification(tmr,txt1,txt2)
       Notification.BackColor=clrback
        Notification.ForeColor=clrtxt

      Notification.Show()
        Notification.Refresh()
        End sub
 
   public Shared   Function   getargs()
              Dim CommandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Application.CommandLineArgs
            
         For i As Integer = 0 To CommandLineArgs.Count - 1
          'MessageBox.Show(CommandLineArgs(i))
             if  (CommandLineArgs(i)) ="/afterupdate" Then
                Thread.Sleep (1000)
                      If File.Exists(Application.StartupPath & "\updater.tmp") then
                      
                        File.Delete (Application.StartupPath & "\updater.exe")
                        MsgBox ("Update completed.All new files replaced with new versions.",vbInformation,"Update completed")
                        File.Copy(Application.StartupPath & "\updater.tmp",Application.StartupPath & "\updater.exe", True)
                        Thread.Sleep (1500)
                        File.Delete (Application.StartupPath & "\updater.tmp")
                    Else 
                     MsgBox ("Update not completed correctly.Updater.exe can't replaced with new version.Please try again or If you interest on update this module please download the last version from website.",vbCritical   ,"Update error")
                    End If


                frmMain.BringToFront
              '  stuff.notify   (1000,"salaam","sss",Color.Red )
                'When Show menu clicks, it will show the form:
         getcmdargs=True  
                

        frmMain.Show()
      

             End If 
             if  (CommandLineArgs(i)) = "/f"  Then
               ' MsgBox ("null")
             End If 

        Next
    End Function

 public shared Function removefilebyext(Folder As String)

             If Directory.Exists(Folder) Then
      
             For Each _file As String In Directory.GetFiles(Folder, "*." & frmMain.txtremoveExt1.Text )
            File.Delete(_file)
        Next
             For Each _file As String In Directory.GetFiles(Folder, "*." & frmMain.txtremoveExt2.Text )
            File.Delete(_file)
        Next
             For Each _file As String In Directory.GetFiles(Folder, "*." & frmMain.txtremoveExt3.Text )
            File.Delete(_file)
        Next
             
            
        For Each _folder As String In Directory.GetDirectories(Folder)
            removefilebyext(_folder)
        Next
        End If
       
 End Function

    public Shared Sub addreg()
 

Dim regVersion As RegistryKey
regVersion = 
Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)

If regVersion Is Nothing Then
    ' Key doesn't exist; create it.
    regVersion = 
Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run")
End If
        
    regVersion.SetValue("RelaxEngine", application.ExecutablePath , RegistryValueKind.String)
    regVersion.Close()

       end Sub


     public Shared Sub removereg()
 

Dim regVersion As RegistryKey

regVersion = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
        Try 
             regVersion.DeleteValue ("RelaxEngine")
              regVersion.Close()

        Catch ex As Exception

        End Try
   
  

       end Sub

  
      public Shared Sub readreg()
        Dim regVersion As RegistryKey
regVersion = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", True)
        
        frmMain.lblstartuppath.Text= regVersion.GetValue ("RelaxEngine")
         regVersion.Close()

       

      End Sub

   



    public Shared sub startupdate

           Dim locationdir =Directory.GetCurrentDirectory()
       
          msgbox ("Click OK to restart program and perform update ... ",vbInformation,"Updating ...")
          Process.Start(locationdir & "\updater.exe ", "/update ")
       

    End sub


    Public Shared Sub DeleteEmptyFolder(ByVal sourceFolderPath As String)
        Try
            CheckFolder(sourceFolderPath)
                For ctr As Integer = _listAllDirectories.Count - 1 To 0 Step -1
                    Dim dir As New IO.DirectoryInfo(_listAllDirectories(ctr))
                    If dir.GetFiles.Length = 0 And dir.GetDirectories.Length = 0 Then
                        dir.Delete()
                    End If
                Next
        Catch ex As Exception

                stuff.mylog (ex.ToString())
        End Try

       
    End Sub

    'Here's the code:
          Shared   Sub CheckFolder(ByVal sourceFolderPath As String)
        Dim di As New IO.DirectoryInfo(sourceFolderPath)
        For Each directory As IO.DirectoryInfo In di.GetDirectories
            _listAllDirectories.Add(directory.FullName)
            CheckFolder(directory.FullName)
        Next
    End Sub



End Class




 
Public class hashing

     Public Shared Function hashed(ByVal plainText As String, ByVal salt As String) As String
 
'Encode by UTF8
Dim pt As New UTF8Encoding
Dim ptBytes() As Byte = pt.GetBytes(plainText)
Dim s As New UTF8Encoding
Dim sBytes() As Byte = s.GetBytes(salt)
'Mix plaintext + salt
'Reserver array to mix plain and salt
Dim ptSBytes() As Byte = New Byte(ptBytes.Length + sBytes.Length - 1) {}
'Copy plaintext bytes to array.
Dim num As Integer
 
For num = 0 To ptBytes.Length - 1
ptSBytes(num) = ptBytes(num)
Next num
 
'Copy salt bytes to array.
For num = 0 To sBytes.Length - 1
ptSBytes(ptBytes.Length + num) = sBytes(num)
Next num
 
Dim hash As HashAlgorithm
'hashing algorithm class.
'hash = New SHA1Managed()
'hash = New SHA384Managed()
'hash = New SHA512Managed()
hash = New SHA256Managed()
' Compute hash
 
Dim hashBytes As Byte()
hashBytes = hash.ComputeHash(ptSBytes)
' Convert into a base64-encoded string.
 
Dim hashValue As String
hashValue = Convert.ToBase64String(hashBytes)
hashed = hashValue
End Function


     Public Shared Function rndhash() As String
 
'Encode by UTF8
Dim pt As New UTF8Encoding
Dim ptBytes() As Byte = pt.GetBytes(rnd)
Dim s As New UTF8Encoding
Dim sBytes() As Byte = s.GetBytes(rnd)
'Mix plaintext + salt
'Reserver array to mix plain and salt
Dim ptSBytes() As Byte = New Byte(ptBytes.Length + sBytes.Length - 1) {}
'Copy plaintext bytes to array.
Dim num As Integer
 
For num = 0 To ptBytes.Length - 1
ptSBytes(num) = ptBytes(num)
Next num
 
'Copy salt bytes to array.
For num = 0 To sBytes.Length - 1
ptSBytes(ptBytes.Length + num) = sBytes(num)
Next num
 
Dim hash As HashAlgorithm
'hashing algorithm class.
'hash = New SHA1Managed()
'hash = New SHA384Managed()
'hash = New SHA512Managed()
hash = New SHA256Managed()
' Compute hash
 
Dim hashBytes As Byte()
hashBytes = hash.ComputeHash(ptSBytes)
' Convert into a base64-encoded string.
 
Dim hashValue As String
hashValue = Convert.ToBase64String(hashBytes)
rndhash = hashValue
End Function








End Class