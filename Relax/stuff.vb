Imports System.IO
Imports System.Threading
  Imports Microsoft.Win32






Public Class stuff

  

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

        
           Form1.   notifyIcon1.BalloonTipIcon = ToolTipIcon.Error
           Form1.  notifyIcon1.BalloonTipTitle = title
           Form1. notifyIcon1.BalloonTipText = txt
           Form1. notifyIcon1.ShowBalloonTip(delay)
     

    End sub
    public Shared  sub ttip2  (txt As String )

        Form1.    NotifyIcon1.BalloonTipText = txt
      Form1.   NotifyIcon1.ShowBalloonTip(8000)
      
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
             if  (CommandLineArgs(i)) ="/s" Then 
                 'MsgBox ("end")
              '  stuff.notify   (1000,"salaam","sss",Color.Red )
                'When Show menu clicks, it will show the form:
         

        form1.Show()
      

             End If 
             if  (CommandLineArgs(i)) = "/f"  Then
                 'MsgBox ("null")
             End If 

        Next
    End Function

 public shared Function removefilebyext(Folder As String)

             If Directory.Exists(Folder) Then
      
             For Each _file As String In Directory.GetFiles(Folder, "*." & Form1.txtremoveExt1.Text )
            File.Delete(_file)
        Next
             For Each _file As String In Directory.GetFiles(Folder, "*." & Form1.txtremoveExt2.Text )
            File.Delete(_file)
        Next
             For Each _file As String In Directory.GetFiles(Folder, "*." & Form1.txtremoveExt3.Text )
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
        
        Form1.lblstartuppath.Text= regVersion.GetValue ("RelaxEngine")
    regVersion.Close()


      End Sub


    public Shared sub statusmsg(txt As String,tmr As String )
        	Form1.lblstatus.Visible  = True  
        	Form1.lblstatus.Text = txt
     Application.DoEvents()
           Thread.Sleep (tmr)
        	Form1.lblstatus.Visible  = False 
  end Sub
End Class