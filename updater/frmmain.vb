Imports System.IO
Imports System.Reflection
Imports System.Threading
Public Class frmmain
  Dim locationdir =Directory.GetCurrentDirectory()
   Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles Me.Shown
         lblver.Text=FileVersionInfo.GetVersionInfo(Application.ExecutablePath).FileVersion

        ProgressBar1.Value=ProgressBar1.Value+10
        ProgressBar1.Refresh()
        Me.Refresh()
      'Application.DoEvents 
       ' Thread.Sleep(2000)
      
      func. copyto()

        try
         
        func. delfile (locationdir & "\tmpupdate" )
           File.Delete( locationdir & "\source.zip" )
            Thread.Sleep(500)
              ProgressBar1.Value=ProgressBar1.Value+10
              ProgressBar1.Refresh()
      '  msgbox ("Update completed.press ok to start relax...")
        Process.Start(locationdir & "\relax.exe","/afterupdate")
         Thread.Sleep(1000)
              ProgressBar1.Value=ProgressBar1.Value+10
              ProgressBar1.Refresh()
       end
               
        Catch ex As Exception
              func.makelog (ex)

        End Try
           ProgressBar1.Value=ProgressBar1.Value+10
          ProgressBar1.Refresh()

    End Sub

    
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
          ProgressBar1.Value=200
        end
    End Sub

    Private Sub frmmain_Load(sender As Object, e As EventArgs) Handles Me.Load
          ProgressBar1.Value=ProgressBar1.Value+10
        ProgressBar1.Refresh()
        func.getargs()
    End Sub
End Class
