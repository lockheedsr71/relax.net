Imports System.Diagnostics.Eventing.Reader
Imports System.IO
Imports System.Threading
Imports System.IO.Compression

Public Class func
     public shared Function delfile(fname As String)

        Try
            Directory.Delete(fname, True)

            Dim directoryExists = Directory.Exists(fname)
        Catch e As Exception

        End Try

    End Function
    
    public Shared  sub copyto()
        Dim locationdir =Directory.GetCurrentDirectory()
        Dim fromDirectory As String = locationdir & "\tmpupdate"
        Dim destinationDirectory As String = locationdir

        frmmain.ProgressBar1.Value=frmmain.ProgressBar1.Value+10
        frmmain.    ProgressBar1.Refresh()
      
        killrelax()
              Thread.Sleep (2000)

          func.zip(locationdir & "\source.zip",  locationdir & "\tmpupdate" )
          frmmain.ProgressBar1.Value=frmmain.ProgressBar1.Value+10
        frmmain.    ProgressBar1.Refresh()
    Try
            My.Computer.FileSystem.CopyDirectory(fromDirectory, destinationDirectory,true )
         Catch e As Exception
              makelog(e)
            frmmain.lblerror.Text = "Some errors has been occurred.Please see the log for more informations."
                End Try
          Thread.Sleep (2000)
        try
            FileCopy (fromDirectory & "\updater.exe",destinationDirectory & "\updater.tmp")
        Catch ex As Exception
            makelog(ex)
            frmmain.lblerror.Text = "Some errors has been occurred.Please see the log for more informations."
        End Try
        

    End sub
    

    public Shared sub getargs()
          frmmain.ProgressBar1.Value=frmmain.ProgressBar1.Value+10
        frmmain.    ProgressBar1.Refresh()
     If Environment.GetCommandLineArgs.Count <> 1 Then
                  If Environment.GetCommandLineArgs(1) = "/update" Then
                 
                   Exit Sub


                  End If
             If Environment.GetCommandLineArgs(1) <> "" Then
                  MsgBox ("Arguments not declared.",vbCritical,"Error")
                  end
              End If 
     Else
            ' if progrum run directly with no args
            MsgBox ("This file need to run via Relax engine.You can't run it directly.",vbInformation,"Warning")
            end
    End If
        
       
   End Sub
    public Shared Function makelog ( e As Exception )
          frmmain.ProgressBar1.Value=frmmain.ProgressBar1.Value+10
        frmmain.    ProgressBar1.Refresh()
         dim logdir =  My.Application.Info.DirectoryPath
              Directory.CreateDirectory (logdir & "\logs")
          Thread.Sleep(1000)
      Dim strFile As String = logdir &  "\logs\updator_ErrorLog_" & DateTime.Today.ToString("yyyy-MM-dd") & ".log"

         File.AppendAllText(strFile, String.Format("{0}{1}"& vbNewLine &  "----------------------------------------------------------" & DateTime.Now  , Environment.NewLine, e.ToString()))

    End Function
            


     
      public Shared  Sub  zip (zipPath As String , unzippath As String )
          frmmain.ProgressBar1.Value=frmmain.ProgressBar1.Value+10
         frmmain.    ProgressBar1.Refresh()
        try
            ZipFile.ExtractToDirectory(zipPath, unzippath)
        Catch ex As Exception

          func.makelog(ex)
           
        End Try
       
    End Sub 



   public Shared  Function killrelax
          frmmain.ProgressBar1.Value=frmmain.ProgressBar1.Value+10
        frmmain.    ProgressBar1.Refresh()
        dim arrProcess() As Process = System.Diagnostics.Process.GetProcessesByName("relax")
        For Each p As Process In arrProcess
            p.Kill()
        Next

   End Function
End Class
