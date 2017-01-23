Imports System
Imports System.IO
Imports System.Diagnostics
Imports System.Reflection
Imports System.Security.Cryptography.X509Certificates
Imports System.Xml


Public Class clsver
 ' Import System.Reflection and System.IO at the top of your class file
      

      Public Shared Function getver (location As String )
        '''  Dim location = Assembly.GetExecutingAssembly().Location
        Dim appPath = Path.GetDirectoryName(location)       ' C:\Some\Directory
        Dim appName = Path.GetFileName(location)            ' MyLibrary.DLL

        ' Get the file version for the notepad.
        ' Use either of the following two commands.
        FileVersionInfo.GetVersionInfo(Path.Combine(  appName ))
        Dim myFileVersionInfo As FileVersionInfo = FileVersionInfo.GetVersionInfo(appName )


        ' Print the file name and version number.
      'Form1.Label2.Text = "File: " + myFileVersionInfo.FileDescription + vbLf + "Version number: " + myFileVersionInfo.FileVersion
       Return (myFileVersionInfo.FileVersion)

    end Function

    public Shared result As Integer 
   public shared function comparever(onsite As String ,onpc As String)
        
        Dim sitever As New Version(onsite)
        dim pcver  As New Version(onpc)
       result =    sitever.CompareTo(pcver) 

       
        return result

        End function

     public Shared   pID As String = "", pName As String = "", pver As String = "" ,  pdes As String = ""
      public shared function  rxml(fname As string )
        Dim xmlDoc As New XmlDocument()
         xmlDoc.Load(fname)
        Dim nodes As XmlNodeList = xmlDoc.DocumentElement.SelectNodes("/settings/Product")
       
        For Each node As XmlNode In nodes
            pID = node.SelectSingleNode("ID").InnerText
            pName = node.SelectSingleNode("Name").InnerText
            pver = node.SelectSingleNode("Version").InnerText
            pdes = node.SelectSingleNode("Descriptions").InnerText



            
        Next

    End function
     

     

End Class

 