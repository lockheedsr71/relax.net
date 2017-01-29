Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles

Public Class Form1


#Region "Main Methods and Objects"

    'This ManualResetEvent is used to control when the background thread waits and when it checks for more
    'output, to avoid it just doing a constant loop unnecessarily
    Private ThreadSignal As New Threading.ManualResetEvent(False)

    Private StopMonitoring As Boolean = False
    Private CurrentOutputHandle As SafeFileHandle = Nothing
    Private CurrentInputHandle As SafeFileHandle = Nothing
    Private CurrentProcessHandle As IntPtr = Nothing


    Private Sub StartProcessAndMonitor()
        StopMonitoring = False
        'Declare objects that will be used later in this method
        Dim TmpReadOutputHandle As SafeFileHandle = Nothing
        Dim TmpWriteInputHandle As SafeFileHandle = Nothing
        Dim StartInfo As New WindowsAPIs.STARTUPINFO

        'Setup the object that is used to provide parameters for the process we will be creating
        StartInfo.cb = Marshal.SizeOf(StartInfo)
        If HideChk.Checked Then
            'The user wants to hide the process window so we specify that we want the ShowWindow API to be used
            'to show the window and because SW_HIDE = 0 then we dont need to bother setting the wShowWindow member
            StartInfo.dwFlags = WindowsAPIs.STARTF_USESTDHANDLES Or WindowsAPIs.STARTF_USESHOWWINDOW
        Else
            'The user does not want to hide the process window so just specify that we want to supply the
            'StdIn, StdOut and StdError handles and do not use ShowWindow
            StartInfo.dwFlags = WindowsAPIs.STARTF_USESTDHANDLES
        End If

        'Setup the structure that provides information for the pipe we will be creating
        Dim SecurityAttributes As New WindowsAPIs.SECURITY_ATTRIBUTES
        SecurityAttributes.nLength = Marshal.SizeOf(SecurityAttributes)
        SecurityAttributes.bInheritHandle = True

        'Create pipe for console output and store the "read end" handle in TmpReadOutputHandle and
        'the "write end" handle is given to our startup parameters object so that the process we create
        'can write to it instead of writing to its normal output (the screen)
        If Not WindowsAPIs.CreatePipe(TmpReadOutputHandle, StartInfo.hStdOutput, SecurityAttributes, 0) Then
            Throw New System.ComponentModel.Win32Exception
        End If

        'Create pipe for console input and store the "write end" handle in TmpWriteInputHandle and
        'the "read end" handle is given to our startup parameters object so that the process we create
        'can read from it instead of reading from its normal input (the keyboard buffer)
        If Not WindowsAPIs.CreatePipe(StartInfo.hStdInput, TmpWriteInputHandle, SecurityAttributes, 0) Then
            Throw New System.ComponentModel.Win32Exception
        End If

        'Duplicate the "write end" handle that we are using for the standard output so that it can be used 
        'for the Error output as well, then we can use the same pipe to read both.
        Dim WriteErrorHandle As SafeFileHandle = Nothing
        If Not WindowsAPIs.DuplicateHandle(New HandleRef(Me, Process.GetCurrentProcess.Handle), StartInfo.hStdOutput, _
                               New HandleRef(Me, Process.GetCurrentProcess.Handle), StartInfo.hStdError, 0, True, WindowsAPIs.DUPLICATE_SAME_ACCESS) Then
            Throw New System.ComponentModel.Win32Exception
        End If

        'Duplicate the "write end" handle for the input pipe and make the duplicated handle non inheritable
        If Not WindowsAPIs.DuplicateHandle(New HandleRef(Me, Process.GetCurrentProcess.Handle), TmpWriteInputHandle, _
                               New HandleRef(Me, Process.GetCurrentProcess.Handle), CurrentInputHandle, 0, False, WindowsAPIs.DUPLICATE_SAME_ACCESS) Then
            Throw New System.ComponentModel.Win32Exception
        End If

        'Duplicate the "read end" handle for the output pipe and make the duplicated handle non inheritable
        If Not WindowsAPIs.DuplicateHandle(New HandleRef(Me, Process.GetCurrentProcess.Handle), TmpReadOutputHandle, _
                               New HandleRef(Me, Process.GetCurrentProcess.Handle), CurrentOutputHandle, 0, False, WindowsAPIs.DUPLICATE_SAME_ACCESS) Then
            Throw New System.ComponentModel.Win32Exception
        End If

        'Close the original handles we got from CreatePipe before we start the process so that it does 
        'not inherit these handles.
        TmpReadOutputHandle.Close()
        TmpWriteInputHandle.Close()
        TmpReadOutputHandle.Dispose()
        TmpWriteInputHandle.Dispose()

        'This will hold information about the process (such as the process handle) once it has been started
        Dim ProcessInfo As New WindowsAPIs.PROCESS_INFORMATION

        'Launch the specified process
        If Not WindowsAPIs.CreateProcess(Nothing, PathBox.Text, SecurityAttributes, SecurityAttributes, True, WindowsAPIs.NORMAL_PRIORITY_PROCESS, IntPtr.Zero, Nothing, StartInfo, ProcessInfo) Then
            'If the CreateProcess API returned false then tell the user that the process has not been started
            MessageBox.Show("Failed to create process. Last error reported was: " & New System.ComponentModel.Win32Exception().Message, "Error Launching Process", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End If
        'Close the handle that CreateProcess gave us for the created process's primary thread
        WindowsAPIs.CloseHandle(ProcessInfo.hThread)

        'Check to make sure we got a valid process ID and process handle back from CreateProcess
        If Not ProcessInfo.dwProcessId = 0 AndAlso Not ProcessInfo.hProcess = IntPtr.Zero Then
            'Store the process's handle in our class level object so that we can access it from other methods when
            'we need to close it or wait on it
            CurrentProcessHandle = ProcessInfo.hProcess
            WriteToOutputBox("Process started successfully" & vbNewLine)
            StartBtn.Enabled = False
            'Create a new thread that will run the BG_GetOutput method
            Dim OutputWatcherThread As New Threading.Thread(AddressOf BG_GetOutput)
            OutputWatcherThread.IsBackground = True
            'Start the background thread
            OutputWatcherThread.Start()
        Else
            'If no process ID or handle was found then alert the user
            MessageBox.Show("Failed to find process ID and handle. Last error reported was: " & New System.ComponentModel.Win32Exception().Message, "Error Launching Process", MessageBoxButtons.OK, MessageBoxIcon.Error)
            DestroyHandles()
            ProcessEnded()
        End If
    End Sub

    ''' <summary>
    ''' Sends a string to the child process
    ''' </summary>
    ''' <param name="message">The string to write to the client process's StdIn pipe</param>
    Private Sub SendInput(ByVal message As String)
        'Make sure we currently have a handle to the "write end" of an input pipe
        If Not CurrentInputHandle Is Nothing AndAlso Not CurrentInputHandle.IsClosed Then
            'Convert the message to a byte array, using cp850 encoding
            Dim MessageBytes() As Byte = System.Text.Encoding.GetEncoding(850).GetBytes(message & vbCrLf & vbCr)
            Dim BytesWritten As UInteger = 0
            'Write the message to the pipe using the WriteFile API
            If Not WindowsAPIs.WriteFile(CurrentInputHandle, MessageBytes, CUInt(MessageBytes.Length - 1), BytesWritten, 0) Then
                MessageBox.Show("There was a problem sending input to the process. The last error reported was: " & New System.ComponentModel.Win32Exception().Message)
            End If
            'Tell the background thread (which is running the BG_GetOutput method) that it should check for output now
            ThreadSignal.Set()
        Else
            WriteToOutputBox("No process to send input to")
        End If
    End Sub

    ''' <summary>
    ''' Reads output from the process and writes it to the output box. Run on a background thread
    ''' </summary>
    Private Sub BG_GetOutput()
        'Loop that stops the thread from ending after it has read all currently available output
        Do
            'Loop that reads from the output pipe until there is no more data to be read
            Do
                'Unfortunately this Thread.Sleep is necessary as otherwise we try to read before any data has been
                'written by the child process. Could probably get away with making the sleep period a bit shorter 
                'but not sure how well it would work on slower PCs then - 600ms should be fine.
                Threading.Thread.Sleep(600)
                Dim Buffer(0) As Byte
                Dim BytesRead As UInteger = 0
                Dim BytesAvailable As UInteger = 0
                Debug.WriteLine("Checking to see if any output is available to be read")
                'Check to see if any data is available to be read in the Output pipe
                Dim PeekResult As Boolean = WindowsAPIs.PeekNamedPipe(CurrentOutputHandle, Nothing, 0, 0, BytesAvailable, 0)
                If PeekResult Then
                    If BytesAvailable > 0 Then
                        'There are some bytes in the Output pipe so lets resize our array so it fits all of them in
                        ReDim Buffer(CInt(BytesAvailable))
                        Debug.WriteLine("Attempting read from pipe")
                        'Read from the pipe into our byte array by using the ReadFile API
                        If WindowsAPIs.ReadFile(CurrentOutputHandle, Buffer, CUInt(Buffer.Length), BytesRead, 0) Then
                            'ReadFile was successful so convert the bytes to a string (using cp850 encoding) and 
                            'write it to the Output box
                            WriteToOutputBox(System.Text.Encoding.GetEncoding(850).GetString(Buffer) & vbNewLine)
                        Else
                            'Read failed so write warning message to the output box and exit read loop
                            WriteToOutputBox("There was a problem reading the process output. The last error reported was: " & _
                                             New System.ComponentModel.Win32Exception().Message)
                            Exit Do 'exit the read loop
                        End If
                    Else
                        'PeekNamedPipe was successful but there are no bytes waiting to be read, so we must have
                        'read all of the output so exit this read loop
                        Debug.WriteLine("No bytes available")
                        Exit Do
                    End If
                Else
                    'The PeekNamedPipe API returned false so lets exit this read loop
                    Debug.WriteLine("Peek failed, last error = " & New System.ComponentModel.Win32Exception().Message)
                    Exit Do
                End If
            Loop 'Back to the start to check if any more data is available to be read

            'If "Exit Do" was called from anywhere above then we will end up here,
            'so we should be here either because all current output has been read or an error was encountered

            'Set the ManualResetEvent back to "non signalled" so that we can wait on it with WaitForMultipleObjects below 
            ThreadSignal.Reset()
            'Create an array of pointers that point to handles we want to wait on, then add the handle for the process
            'we launched so that we know when it has terminated, and the handle for our ManualResetEvent so that we 
            'know when another part of this app has called ThreadSignal.Set, which means we need to check for more output
            Dim WaitOnHandles(1) As IntPtr
            WaitOnHandles(0) = CurrentProcessHandle
            WaitOnHandles(1) = ThreadSignal.SafeWaitHandle.DangerousGetHandle
            Debug.WriteLine("Waiting for signal or process termination")
            'Use the WaitForMultipleObjects API to wait for either the process to exit or our ManualResetEvent
            'to be signalled. The execution of code will halt here now until one of those things happens, but 
            'we use a timeout as well so that we go and check for more output after 10 seconds regardless
            Dim ContinueReason As UInteger = WindowsAPIs.WaitForMultipleObjects(2, WaitOnHandles, False, 10000)
            'If we get here then one of the handles we were waiting on has been signalled or the 10 second
            'timeout has been hit. The return value from WaitForMultipleObjects indicates which it was.
            If ContinueReason = 0 Then
                Debug.WriteLine("Continue reason = process terminated")
                'The process we started has terminated, so clean up the handles that we had open to it 
                'and enable the Start button etc again, then terminate this thread
                DestroyHandles()
                ProcessEnded()
                Exit Do 'terminates this thread as there is no more work to do once we exit this loop
            ElseIf ContinueReason = 1 Then
                'We got here because ThreadSignal.Set has been called from another part of our app (this 
                'happens when the user sends input to the process for example)
                Debug.WriteLine("Continue reason = signal received")
                'The StopMonitoring boolean gets set to true when our app is closing so if that is true
                'then we dont need to enable the Start button again or write anything to the output box 
                If StopMonitoring Then
                    Debug.WriteLine("Output monitoring thread terminating")
                    DestroyHandles()
                    Exit Do 'terminates this thread as there is no more work to do once we exit this loop
                Else
                    'If StopMonitoring is not true then we go back to the start of the loop (which then starts 
                    'the read loop again as well) to check for more output
                    Continue Do
                End If
            ElseIf ContinueReason = WindowsAPIs.WAIT_TIMEOUT Then
                Debug.WriteLine("Continue reason = timed out")
            ElseIf ContinueReason = WindowsAPIs.WAIT_FAILED Then
                Debug.WriteLine("WaitForMultipleObjects failed. Last error = " & New System.ComponentModel.Win32Exception().Message)
            End If
        Loop
        'If we end up here then the thread is about to end as there is no more work for it to do
        Debug.WriteLine("Background thread terminating")
    End Sub

    ''' <summary>
    ''' Closes open handles to processes and pipes
    ''' </summary>
    Private Sub DestroyHandles()
        If Not CurrentProcessHandle = IntPtr.Zero Then
            WindowsAPIs.CloseHandle(CurrentProcessHandle)
        End If
        If Not CurrentInputHandle Is Nothing AndAlso Not CurrentInputHandle.IsClosed Then
            CurrentInputHandle.Close()
        End If
        If Not CurrentOutputHandle Is Nothing AndAlso Not CurrentOutputHandle.IsClosed Then
            CurrentOutputHandle.Close()
        End If
    End Sub

    ''' <summary>
    ''' Enables the start button again and write an appropiate message to the output textbox. Thread safe
    ''' </summary>
    Private Sub ProcessEnded()
        If Me.StartBtn.InvokeRequired Then
            Invoke(New MethodInvoker(AddressOf ProcessEnded))
        Else
            StartBtn.Enabled = True
            WriteToOutputBox(vbNewLine & "- - - Process has terminated - - -" & vbNewLine)
        End If
    End Sub

    ''' <summary>
    ''' Writes a string to the output textbox. Thread safe
    ''' </summary>
    ''' <param name="message">The string to write to the output box</param>
    Private Sub WriteToOutputBox(ByVal message As String)
        If Me.OutputBox.InvokeRequired Then
            Invoke(New Action(Of String)(AddressOf WriteToOutputBox), message)
        Else
            OutputBox.AppendText(vbNewLine & message & vbNewLine)
            OutputBox.ScrollToCaret()
        End If
    End Sub

#End Region


#Region "Event Handlers"

Private Sub StartBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StartBtn.Click
    If PathBox.TextLength < 1 Then
        MessageBox.Show("Please enter the path to the program you wish to launch", "No Program Path Specified", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    Else
        StartProcessAndMonitor()
    End If
End Sub

Private Sub InputBtn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputBtn.Click
    If InputBox.TextLength < 1 Then
        MessageBox.Show("You must enter at least 1 character to be sent to the process", "No Text Entered", MessageBoxButtons.OK, MessageBoxIcon.Warning)
    Else
        SendInput(InputBox.Text)
        InputBox.Clear()
    End If
End Sub

Private Sub Form1_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
    DestroyHandles()
    StopMonitoring = True
    ThreadSignal.Set()
End Sub

#End Region


End Class
