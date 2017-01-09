Imports System.ComponentModel
Imports System.Runtime.InteropServices

'Animates a form when it is shown, hidden or closed.
'MDI child forms do not support the Blend method and only support other methods while being displayed for the first time and when closing.
Public NotInheritable Class FormAnimator

    Public Enum AnimationMethod
        'Rolls out from edge when showing and into edge when hiding.  Requires a Direction.
        Roll = &H0
        'Expands out from centre when showing and collapses into centre when hiding.
        Centre = &H10
        'Slides out from edge when showing and slides into edge when hiding.  Requires a Direction.
        Slide = &H40000
        'Fades from transaprent to opaque when showing and from opaque to transparent when hiding.
        Blend = &H80000
    End Enum

    <Flags()> Public Enum AnimationDirection

        'The directions in which the Roll and Slide animations can be shown.  Horizontal and vertical directions can be combined to create diagonal animations.
        Right = &H1
        Left = &H2
        Down = &H4
        Up = &H8
    End Enum

    'Hide the form.
    Private Const AW_HIDE As Integer = &H10000

    'Activate the form.
    Private Const AW_ACTIVATE As Integer = &H20000

    'The number of milliseconds over which the animation occurs if no value is specified.
    Private Const DEFAULT_DURATION As Integer = 250

    'The form to be animated.
    Private WithEvents _form As Form

    'The animation method used to show and hide the form.
    Private _method As AnimationMethod

    'The direction in which to Roll or Slide the form.
    Private _direction As AnimationDirection

    'The number of milliseconds over which the animation is played.
    Private _duration As Integer

    Public Property Method() As AnimationMethod
        'Gets or sets the animation method used to show and hide the form.  Roll is used by default if no method is specified.
        Get
            Return Me._method
        End Get
        Set(ByVal Value As AnimationMethod)
            Me._method = Value
        End Set
    End Property

    Public Property Direction() As AnimationDirection
        'Gets or sets the direction in which the animation is performed.  Roll and Slide only.
        Get
            Return Me._direction
        End Get
        Set(ByVal Value As AnimationDirection)
            Me._direction = Value
        End Set
    End Property

    Public Property Duration() As Integer
        'Gets or sets the number of milliseconds over which the animation is played.
        Get
            Return Me._duration
        End Get
        Set(ByVal Value As Integer)
            Me._duration = Value
        End Set
    End Property

    Public ReadOnly Property Form() As Form
        'Gets the form to be animated.
        Get
            Return Me._form
        End Get
    End Property

    'Windows API function to animate a window.
    <DllImport("user32")> _
    Private Shared Function AnimateWindow(ByVal hWnd As IntPtr, ByVal dwTime As Integer, ByVal dwFlags As Integer) As Boolean
    End Function

    'Creates a new FormAnimator object for the specified form.
    'No animation will be used unless the Method and/or Direction properties are set independently. The Duration is set to quarter of a second by default.
    'Used when you specify FORM and DURATION.
    Public Sub New(ByVal form As Form)
        Me._form = form
        Me._duration = DEFAULT_DURATION
    End Sub

    'Creates a new FormAnimator object for the specified form using the specified method over the specified duration.
    'No animation will be used for the Roll or Slide methods unless the Direction property is set independently.
    'Used when you specify FORM, METHOD and DURATION.
    Public Sub New(ByVal form As Form, ByVal method As AnimationMethod, ByVal duration As Integer)
        Me.New(form)
         Application.DoEvents
                 
        Me._method = method
        Me._duration = duration
    End Sub


    'Creates a new FormAnimator object for the specified form using the specified method in the specified direction over the specified duration.
    'The Direction argument will have no effect if the Centre or Blend method is specified.
    'Used when you specify FORM, METHOD, DIRECTION and DURATION.
    Public Sub New(ByVal form As Form, ByVal method As AnimationMethod, ByVal direction As AnimationDirection, ByVal duration As Integer)
        Me.New(form, method, duration)
        Me._direction = direction
    End Sub

    Private Sub Form_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles _form.Load
        'MDI child forms do not support transparency so do not try to use the Blend method.
        If Me._form.MdiParent Is Nothing OrElse Me._method <> AnimationMethod.Blend Then
            'Activate the form.
            FormAnimator.AnimateWindow(Me._form.Handle, Me._duration, AW_ACTIVATE Or Me._method Or Me._direction)
        End If
    End Sub

    Private Sub Form_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _form.VisibleChanged
        'Do not attempt to animate MDI child forms while showing or hiding as they do not behave as expected.
        If Me._form.MdiParent Is Nothing Then
            Dim flags As Integer = Me._method Or Me._direction

            If Me._form.Visible Then
                'Activate the form.
                flags = flags Or AW_ACTIVATE
            Else
                'Hide the form.
                flags = flags Or AW_HIDE
            End If

            FormAnimator.AnimateWindow(Me._form.Handle, Me._duration, flags)
        End If
    End Sub

    Private Sub Form_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles _form.Closing
        If Not e.Cancel Then
            'MDI child forms do not support transparency so do not try to use the Blend method.
            If Me._form.MdiParent Is Nothing OrElse Me._method <> AnimationMethod.Blend Then
                'Hide the form.
                FormAnimator.AnimateWindow(Me._form.Handle, Me._duration, AW_HIDE Or Me._method Or Me._direction)
            End If
        End If
    End Sub

End Class
