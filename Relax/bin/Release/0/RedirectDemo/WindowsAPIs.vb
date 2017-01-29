Imports System.Runtime.InteropServices
Imports Microsoft.Win32.SafeHandles

Public Class WindowsAPIs


#Region "Constants"

    Public Const STARTF_USESTDHANDLES As UInteger = &H100
    Public Const CREATE_NEW_CONSOLE As UInteger = &H10
    Public Const STARTF_USESHOWWINDOW As UInteger = 1
    Public Const NORMAL_PRIORITY_PROCESS As UInteger = &H20
    Public Const WAIT_TIMEOUT As UInteger = &H102
    Public Const DUPLICATE_SAME_ACCESS As UInteger = 2
    Public Const WAIT_FAILED As UInteger = 4294967295

#End Region


#Region "Class/Structure Definitions"

    <StructLayout(LayoutKind.Sequential)> _
    Public Class SECURITY_ATTRIBUTES
        Public nLength As Integer
        Public lpSecurityDescriptor As New SafeFileHandle(IntPtr.Zero, False)
        Public bInheritHandle As Boolean
    End Class

    <StructLayout(LayoutKind.Sequential)> _
    Public Class STARTUPINFO
        Public cb As Integer
        Public lpReserved As IntPtr = IntPtr.Zero
        Public lpDesktop As IntPtr = IntPtr.Zero
        Public lpTitle As IntPtr = IntPtr.Zero
        Public dwX As Integer
        Public dwY As Integer
        Public dwXSize As Integer
        Public dwYSize As Integer
        Public dwXCountChars As Integer
        Public dwYCountChars As Integer
        Public dwFillAttribute As Integer
        Public dwFlags As Integer
        Public wShowWindow As Short
        Public cbReserved2 As Short
        Public lpReserved2 As IntPtr = IntPtr.Zero
        Public hStdInput As SafeFileHandle = New SafeFileHandle(IntPtr.Zero, False)
        Public hStdOutput As SafeFileHandle = New SafeFileHandle(IntPtr.Zero, False)
        Public hStdError As SafeFileHandle = New SafeFileHandle(IntPtr.Zero, False)
    End Class

    <StructLayoutAttribute(LayoutKind.Sequential)> _
    Public Structure PROCESS_INFORMATION
        Public hProcess As System.IntPtr
        Public hThread As System.IntPtr
        Public dwProcessId As UInteger
        Public dwThreadId As UInteger
    End Structure

#End Region


#Region "Methods"


    <DllImport("kernel32.dll", EntryPoint:="CreatePipe", SetLastError:=True)> _
    Public Shared Function CreatePipe(<Out()> ByRef hReadPipe As SafeFileHandle, <Out()> ByRef hWritePipe As SafeFileHandle, _
                                      ByVal lpPipeAttributes As SECURITY_ATTRIBUTES, ByVal nSize As Integer) As Boolean
    End Function

    <DllImport("kernel32.dll", EntryPoint:="CreateProcess", SetLastError:=True)> _
    Public Shared Function CreateProcess(<MarshalAs(UnmanagedType.LPTStr)> ByVal lpApplicationName As String, ByVal lpCommandLine As String, _
                                         ByVal lpProcessAttributes As SECURITY_ATTRIBUTES, ByVal lpThreadAttributes As SECURITY_ATTRIBUTES, ByVal bInheritHandles As Boolean, _
                                         ByVal dwCreationFlags As Integer, ByVal lpEnvironment As IntPtr, <MarshalAs(UnmanagedType.LPTStr)> ByVal lpCurrentDirectory As String, _
                                         ByVal lpStartupInfo As STARTUPINFO, ByRef lpProcessInformation As PROCESS_INFORMATION) As Boolean
    End Function

    <DllImportAttribute("kernel32.dll", EntryPoint:="PeekNamedPipe", SetLastError:=True)> _
    Public Shared Function PeekNamedPipe(<InAttribute()> ByVal hNamedPipe As SafeFileHandle, ByRef lpBuffer As Byte(), ByVal nBufferSize As UInteger, _
                                         <Out()> ByRef lpBytesRead As UInteger, <Out()> ByRef lpTotalBytesAvail As UInteger, _
                                         <Out()> ByRef lpBytesLeftThisMessage As UInteger) As <MarshalAsAttribute(UnmanagedType.Bool)> Boolean
    End Function

    <DllImportAttribute("kernel32.dll", EntryPoint:="ReadFile", SetLastError:=True)> _
    Public Shared Function ReadFile(<InAttribute()> ByVal hFile As SafeFileHandle, ByVal lpBuffer As Byte(), ByVal nNumberOfBytesToRead As UInteger, _
                                   <Out()> ByRef lpNumberOfBytesRead As UInteger, ByVal lpOverlapped As UInteger) As <MarshalAsAttribute(UnmanagedType.Bool)> Boolean
    End Function

    <DllImportAttribute("kernel32.dll", EntryPoint:="WriteFile", SetLastError:=True)> _
    Public Shared Function WriteFile(<InAttribute()> ByVal hFile As SafeFileHandle, <InAttribute()> ByVal lpBuffer As Byte(), ByVal nNumberOfBytesToWrite As UInteger, _
                                     <Out()> ByRef lpNumberOfBytesWritten As UInteger, ByVal lpOverlapped As UInteger) As <MarshalAsAttribute(UnmanagedType.Bool)> Boolean
    End Function

    <DllImportAttribute("kernel32.dll", EntryPoint:="CloseHandle", SetLastError:=True)> _
    Public Shared Function CloseHandle(<InAttribute()> ByVal hObject As IntPtr) As <MarshalAsAttribute(UnmanagedType.Bool)> Boolean
    End Function

    <DllImportAttribute("kernel32.dll", EntryPoint:="CloseHandle")> _
    Public Shared Function CloseHandle(<InAttribute()> ByVal hObject As SafeFileHandle) As <MarshalAsAttribute(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("kernel32.dll", EntryPoint:="DuplicateHandle", CharSet:=CharSet.Auto, SetLastError:=True)> _
    Public Shared Function DuplicateHandle(ByVal hSourceProcessHandle As HandleRef, ByVal hSourceHandle As SafeHandle, ByVal hTargetProcess As HandleRef, _
                                           <Out()> ByRef targetHandle As SafeFileHandle, ByVal dwDesiredAccess As Integer, _
                                           ByVal bInheritHandle As Boolean, ByVal dwOptions As Integer) As Boolean
    End Function

    <DllImport("kernel32.dll", EntryPoint:="WaitForMultipleObjects", SetLastError:=True)> _
    Public Shared Function WaitForMultipleObjects(ByVal nCount As UInteger, ByVal WaitForHandles As IntPtr(), _
                                                  ByVal bWaitAll As Boolean, ByVal dwMilliseconds As UInteger) As UInteger
    End Function

#End Region


End Class
