Imports System.Xml
Imports System.IO
Imports System.Threading

Public   Class getxml
    public Shared Function wrtxml
        
         'first let's check if there is a file MyXML.xml into our application folder
		'if there wasn't a file something like that, then let's create a new one.
        'declare our xmlwritersettings object
			Dim settings As New XmlWriterSettings()

			'lets tell to our xmlwritersettings that it must use indention for our xml
			settings.Indent = True

			'lets create the MyXML.xml document, the first parameter was the Path/filename of xml file
			' the second parameter was our xml settings
			Dim XmlWrt As XmlWriter = XmlWriter.Create( My.Application.Info.DirectoryPath & "\vars.xml", settings)

			With XmlWrt

				' Write the Xml declaration.
				.WriteStartDocument()

				' Write a comment.
				.WriteComment("XML Database.")

				' Write the root element.
				.WriteStartElement("Configs")

				' Start our first person.
				.WriteStartElement("Time")

				' The person nodes.

				.WriteStartElement("startin1")
				.WriteString(Form1.txtstartin1.Text )
				.WriteEndElement()

				.WriteStartElement("startin2")
				.WriteString(Form1.txtstartin2.Text)
				.WriteEndElement()

                .WriteStartElement("startin3")
				.WriteString(Form1.txtstartin3.Text)
				.WriteEndElement()


				' The end of this person.
				.WriteEndElement()
                    
                	        ' Start our first person.
				        .WriteStartElement("path")

				        ' The person nodes.

				        .WriteStartElement("FilePathUbix")
				        .WriteString(Form1.txtfilepathubix .Text)
				        .WriteEndElement()

				        .WriteStartElement("FilePathClient")
				        .WriteString(Form1.txtfilepathclient .Text)
				        .WriteEndElement()

                        .WriteStartElement("FilePathClientOut")
				        .WriteString(Form1.txtfilepathclientout .Text)
				        .WriteEndElement()


				        ' The end of this person.
				        .WriteEndElement()


                              ' Start our first person.
				                .WriteStartElement("remove")

				                ' The person nodes.

				                .WriteStartElement("path1")
				                .WriteString(Form1.txtremovepath1.Text)
				                .WriteEndElement()

				                .WriteStartElement("path2")
				                .WriteString(Form1.txtremovepath2.Text)
				                .WriteEndElement()

                                .WriteStartElement("path3")
				                .WriteString(Form1.txtremovepath3.Text)
				                .WriteEndElement()

                                 .WriteStartElement("path4")
				                .WriteString(Form1.txtremovepath4.Text)
				                .WriteEndElement()

                                 .WriteStartElement("path5")
				                .WriteString(Form1.txtremovepath5.Text)
				                .WriteEndElement()

            
				                .WriteStartElement("ext1")
				                .WriteString(Form1.txtremoveExt1.Text)
				                .WriteEndElement()

				                .WriteStartElement("ext2")
				                .WriteString(Form1.txtremoveExt2.Text)
				                .WriteEndElement()

                                .WriteStartElement("ext3")
				                .WriteString(Form1.txtremoveExt3.Text)
				                .WriteEndElement()
            

                                .WriteStartElement("RemoveTS")
				                .WriteString(Form1.chkRemoveTS.CheckState  )
				                .WriteEndElement()

				                ' The end of this person.
				                .WriteEndElement()

                     ' Start our first person.
				        .WriteStartElement("timers")

				        ' The person nodes.

				        .WriteStartElement("chktimer")
				        .WriteString(Form1.txtchktimer .Text)
				        .WriteEndElement()
            
				        ' The end of this person.
				        .WriteEndElement()


				' Close the XmlTextWriter.
				.WriteEndDocument()
				.Close()

			End With

		'	stuff.statusmsg ("Configuration saved successfully.",3000)

   


		 

    End Function



    public Shared Function readxml()

    'check if file myxml.xml is existing
		If (IO.File.Exists( My.Application.Info.DirectoryPath & "\vars.xml")) Then

			'create a new xmltextreader object
			'this is the object that we will loop and will be used to read the xml file
			Dim document As XmlReader = New XmlTextReader( My.Application.Info.DirectoryPath & "\vars.xml")

			'loop through the xml file
			While (document.Read())

				Dim type = document.NodeType
  ' ----------------------- start time section --------------------
				'if node type was element
				If (type = XmlNodeType.Element) Then

					'if the loop found a <FirstName> tag
					If (document.Name = "startin1") Then

                    
                          Form1.txtstartin1 .Text = document.ReadInnerXml.ToString()

					End If

					'if the loop found a <LastName tag
					If (document.Name = "startin2") Then

					    Form1.txtstartin2 .Text  = document.ReadInnerXml.ToString()
                        
					End If

                    'if the loop found a <LastName tag
					If (document.Name = "startin3") Then

					    Form1.txtstartin3 .Text  = document.ReadInnerXml.ToString()
                        
					End If 

  ' -----------------------main path section --------------------

                    'if the loop found a <LastName tag
					If (document.Name = "FilePathUbix") Then

					Form1.txtfilepathubix 	.Text = document.ReadInnerXml.ToString()

					End If

                     'if the loop found a <LastName tag
					If (document.Name = "FilePathClient") Then

						Form1.txtfilepathclient   .Text = document.ReadInnerXml.ToString()

					End If

                     'if the loop found a <LastName tag
					If (document.Name = "FilePathClientOut") Then

					 Form1.txtfilepathclientout.Text  = document.ReadInnerXml.ToString()

					End If
                    
   ' ----------------------- remove path section --------------------
                     'if the loop found a <LastName tag
					If (document.Name = "path1") Then

					 Form1.txtremovepath1 .Text  = document.ReadInnerXml.ToString()

					End If
                   'if the loop found a <LastName tag
					If (document.Name = "path2") Then

					 Form1.txtremovepath2.Text  = document.ReadInnerXml.ToString()

					End If
                  'if the loop found a <LastName tag
					If (document.Name = "path3") Then

					 Form1.txtremovepath3.Text  = document.ReadInnerXml.ToString()

					End If
                'if the loop found a <LastName tag
					If (document.Name = "path4") Then

					 Form1.txtremovepath4.Text  = document.ReadInnerXml.ToString()

					End If
                'if the loop found a <LastName tag
					If (document.Name = "path5") Then

					 Form1.txtremovepath5.Text  = document.ReadInnerXml.ToString()

					End If


  ' ----------------------- extention remove section --------------------

                     'if the loop found a <LastName tag
					If (document.Name = "ext1") Then

					 Form1.txtremoveExt1.Text  = document.ReadInnerXml.ToString()

					End If
                     'if the loop found a <LastName tag
					If (document.Name = "ext2") Then

					 Form1.txtremoveExt2.Text  = document.ReadInnerXml.ToString()

					End If
                     'if the loop found a <LastName tag
					If (document.Name = "ext3") Then

					 Form1.txtremoveExt3.Text  = document.ReadInnerXml.ToString()

					End If

                    If (document.Name = "RemoveTS") Then

					 Form1.chkRemoveTS.CheckState   = document.ReadInnerXml.ToString()

					End If

     ' ----------------------- timer section --------------------

                     'if the loop found a <LastName tag
					If (document.Name = "chktimer") Then

					 Form1.txtchktimer .Text  = document.ReadInnerXml.ToString()

					End If

				End If

			End While
            document.Close ()

		Else

			MessageBox.Show("The filename you selected was not found.")
		End If
        end Function

End Class
