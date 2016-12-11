'INI Call for VB.NET
'Version 1.1

Imports System.IO
Imports System.Text

Public Class goini

    Private Enum KeyData
        KeyName = 0
        KeyValue = 1
    End Enum

    Private Const SplitChar As String = "€"
    Private Const AssignChar As String = "="
    Private Const SelectionStart As String = "["
    Private Const SelectionEnd As String = "]"

    Private mFilename As String
    Private IniData As List(Of String)

    Public Sub ReadSection(ByVal Selection As String, ByVal Strings As List(Of String))
        If (Not IniData Is Nothing) Then
            'Return a list of key names from a selection.
            For Each sLine In IniData
                'Check if selections match.
                If IsMatch(GetSelection(sLine), Selection) Then
                    'Get key name
                    Dim sPos As Integer = sLine.IndexOf(SplitChar)
                    sLine = sLine.Substring(sPos + 1)
                    'Check for selection.
                    If Not IsSelection(sLine) Then
                        'Add keyname to list
                        Call Strings.Add(GetKeyData(sLine, KeyData.KeyName))
                    End If
                End If
            Next
        End If
    End Sub

    Public Sub ReadSections(ByVal Strings As List(Of String))
        Dim selName As String
        If (Not IniData Is Nothing) Then
            For Each sLine In IniData
                selName = GetSelection(sLine)
                'Check if selection is already found.
                If Not Strings.Contains(selName) Then
                    'Selection not found in list add it.
                    Call Strings.Add(selName)
                End If
            Next
        End If
    End Sub

    Property Filename As String
        'Get filename.
        Get
            Filename = mFilename
        End Get
        'Set filename.
        Set(ByVal value As String)
            mFilename = value
        End Set
    End Property

    Public Function ReadBool(ByVal Selection As String, ByVal Key As String, Optional ByVal iDefault As Boolean = False) As Boolean
        Dim src As String = ReadString(Selection, Key, iDefault).ToLower()

        If (src = "yes") Or (src = "on") Then
            ReadBool = True
        Else
            ReadBool = CBool(src)
        End If

    End Function

    Public Function ReadString(ByVal Selection As String, ByVal Key As String, Optional ByVal iDefault As String = "") As String
        'Returns a keys data.
        Dim Ret As String = vbNullChar

        If (Not IniData Is Nothing) Then
            'Loop though the lines in the list collection.
            For Each sLine In IniData
                Dim sPos As Integer = sLine.IndexOf(SplitChar)
                'Check for splitchar chr 128 position.
                If (sPos > 0) Then
                    'Check for selection match.
                    If IsMatch(sLine.Substring(0, sPos), Selection) Then
                        'Get line.
                        Dim Temp As String = sLine.Substring(sPos + 1, sLine.Length - sPos - 1)
                        'Check that key name is found.
                        If IsMatch(GetKeyData(Temp, KeyData.KeyName), Key) Then
                            'Store return value.
                            Ret = GetKeyData(sLine, KeyData.KeyValue)
                        End If
                    End If
                End If
            Next
        End If

        If (Ret = vbNullChar) Then
            'Return default value.
            ReadString = iDefault
        Else
            'Return returned value.
            ReadString = Ret
        End If
    End Function

    Function SectionExists(ByVal Selection As String) As Boolean
        'Returns true or false if a selection exists.
        Dim bFound As Boolean = False

        If (Not IniData Is Nothing) Then
            For Each sLine In IniData
                Dim selName As String = GetSelection(sLine)
                'Check if selection is found.
                If IsMatch(selName, Selection) Then
                    bFound = True
                    Exit For
                End If
            Next
        End If
        'Return result.
        SectionExists = bFound

    End Function

    Public Function LoadFromFile() As Boolean
        Dim sLine As String
        Dim mSelection As String = ""
        Dim sr As StreamReader

        'Check that file is found
        If Not File.Exists(Filename) Then
            LoadFromFile = False
            Exit Function
        End If

        IniData = New List(Of String)
        'Create new string builder object

        sr = New StreamReader(Filename)
        'Read each line of the ini filename.
        While Not sr.EndOfStream
            'Get current line
            sLine = sr.ReadLine().Trim()

            'Check line length and if we have a selection.
            If (sLine.Length) And IsSelection(sLine) Then
                mSelection = RemoveBrackets(sLine)
            End If

            'Get value names.
            If sLine.Length Then
                'Add to list.
                Call IniData.Add(mSelection.ToUpper() + SplitChar + sLine)
            End If

        End While
        'Close open file stream.
        Call sr.Close()
        'Looks ok to me lets return good result.
        LoadFromFile = True
    End Function

    'INI TOOLS
    Private Function GetKeyData(ByVal TempLine As String, ByVal RetType As KeyData) As String
        Dim Ret As String = vbNullChar
        'Check for assign position.
        Dim aPos As Integer = TempLine.IndexOf(AssignChar)

        'Return key name.
        If (RetType = KeyData.KeyName) Then
            If (aPos <> -1) Then
                'Remove assign char and return key name.
                Ret = TempLine.Substring(0, aPos).TrimEnd()
            End If
        End If

        'Return key value.
        If (RetType = KeyData.KeyValue) Then
            'Check for assignment psoition.
            If (aPos <> -1) Then
                Ret = TempLine.Substring(aPos + 1)
            End If
        End If
        GetKeyData = Ret

    End Function

    Private Function GetSelection(ByVal TmpLine As String) As String
        Dim sPos As Integer = TmpLine.IndexOf(SplitChar)

        If (sPos = 0) Then
            GetSelection = ""
        Else
            GetSelection = TmpLine.Substring(0, sPos)
        End If
    End Function

    Private Function IsMatch(ByVal Source As String, ByVal FindStr As String) As Boolean
        'Return true if source is the same as findstr.
        IsMatch = StrComp(Source, FindStr, CompareMethod.Text) = 0
    End Function

    Private Function IsSelection(ByVal TempLine As String) As Boolean
        'Checks that string is selection by checking start and end chars.
        IsSelection = (TempLine.StartsWith(SelectionStart)) And (TempLine.EndsWith(SelectionEnd))
    End Function

    Private Function RemoveBrackets(ByVal TempLine As String) As String
        'Remove braces and return selection name.
        RemoveBrackets = TempLine.Substring(1, TempLine.Length - 2)
    End Function


End Class