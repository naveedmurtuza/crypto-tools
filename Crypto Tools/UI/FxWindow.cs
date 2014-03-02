using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CryptoTools.UI
{
    public class FxWindow : Window
    {
        public FxWindow()
        {

           SourceInitialized += InitializeWindowSource;
           PreviewMouseMove += ResetCursor;
        }
        #region ResizeDirection enum

        public enum ResizeDirection
        {
            Left = 1,
            Right = 2,
            Top = 3,
            TopLeft = 4,
            TopRight = 5,
            Bottom = 6,
            BottomLeft = 7,
            BottomRight = 8,
        }

        #endregion

        private const int WmSyscommand = 0x112;

        private readonly string[] _minimizeParts = new[]
                                                      {
                                                          "top", "bottom", "left", "right", "topLeft", "topRight",
                                                          "bottomLeft", "bottomRight"
                                                      };

        private HwndSource _hwndSource;

        /// <summary>
        /// Determines if window should override the chorme.
        /// </summary>
        public virtual bool IsOverridingWindowsChrome
        {
            get { return true; }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (IsOverridingWindowsChrome)
            {
                GetTemplatedChildOrThrow<FrameworkElement>("PART_Caption").MouseLeftButtonDown += DoDragMove;
                GetTemplatedChildOrThrow<Button>("PART_Close").Click += Close;
                
                if(ResizeMode != ResizeMode.NoResize)
                {var minButton = GetTemplatedChild<Button>("PART_Minimize");
                if (minButton != null)
                    minButton.Click += Minimize;
                    if (ResizeMode != ResizeMode.CanMinimize)
                    {
                        foreach (string minimizePart in _minimizeParts)
                        {
                            var element = GetTemplatedChildOrThrow<FrameworkElement>(minimizePart);
                            if (element != null)
                            {
                                element.PreviewMouseDown += Resize;
                                element.MouseMove += DisplayResizeCursor;
                            }
                        }
                    }
                }
                
                
            }
        }

        private TElement GetTemplatedChildOrThrow<TElement>(string elementName) where TElement : DependencyObject
        {
            DependencyObject o = GetTemplateChild(elementName);
            if (o == null)
                throw new ArgumentException("element name not found or the provided type is invalid.", elementName);

            return (TElement) o;
        }

        private TElement GetTemplatedChild<TElement>(string elementName) where TElement : DependencyObject
        {
            DependencyObject o = GetTemplateChild(elementName);
            return (TElement) o;
        }

        protected override void OnInitialized(EventArgs e)
        {
            if (IsOverridingWindowsChrome)
                SetResourceReference(StyleProperty, "WindowsChromeOverride");

            base.OnInitialized(e);
        }

        private void Close(object sender, EventArgs e)
        {
            Close();
        }

        private void DoDragMove(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Minimize(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }


        private void InitializeWindowSource(object sender, EventArgs e)
        {
            _hwndSource = PresentationSource.FromVisual((Visual) sender) as HwndSource;
        }

        //private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        //{
        //    Debug.WriteLine("WndProc messages: " + msg.ToString());
        //    //
        //    // Check incoming window system messages
        //    //
        //    if (msg == WmSyscommand)
        //    {
        //        Debug.WriteLine("WndProc messages: " + msg.ToString());
        //    }

        //    return IntPtr.Zero;
        //}

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void ResizeWindow(ResizeDirection direction)
        {
            SendMessage(_hwndSource.Handle, WmSyscommand, (IntPtr) (61440 + direction), IntPtr.Zero);
        }

        private void ResetCursor(object sender, MouseEventArgs e)
        {
            if (Mouse.LeftButton != MouseButtonState.Pressed)
            {
                Cursor = Cursors.Arrow;
            }
        }

        private void Resize(object sender, MouseButtonEventArgs e)
        {
            var clickedRectangle = sender as Rectangle;

            if (clickedRectangle != null)
                switch (clickedRectangle.Name)
                {
                    case "top":
                        Cursor = Cursors.SizeNS;
                        ResizeWindow(ResizeDirection.Top);
                        break;
                    case "bottom":
                        Cursor = Cursors.SizeNS;
                        ResizeWindow(ResizeDirection.Bottom);
                        break;
                    case "left":
                        Cursor = Cursors.SizeWE;
                        ResizeWindow(ResizeDirection.Left);
                        break;
                    case "right":
                        Cursor = Cursors.SizeWE;
                        ResizeWindow(ResizeDirection.Right);
                        break;
                    case "topLeft":
                        Cursor = Cursors.SizeNWSE;
                        ResizeWindow(ResizeDirection.TopLeft);
                        break;
                    case "topRight":
                        Cursor = Cursors.SizeNESW;
                        ResizeWindow(ResizeDirection.TopRight);
                        break;
                    case "bottomLeft":
                        Cursor = Cursors.SizeNESW;
                        ResizeWindow(ResizeDirection.BottomLeft);
                        break;
                    case "bottomRight":
                        Cursor = Cursors.SizeNWSE;
                        ResizeWindow(ResizeDirection.BottomRight);
                        break;
                }
            e.Handled = true;
        }

        private void DisplayResizeCursor(object sender, MouseEventArgs e)
        {
            var clickedRectangle = sender as Rectangle;

            if (clickedRectangle != null)
                switch (clickedRectangle.Name)
                {
                    case "top":
                        Cursor = Cursors.SizeNS;
                        break;
                    case "bottom":
                        Cursor = Cursors.SizeNS;
                        break;
                    case "left":
                        Cursor = Cursors.SizeWE;
                        break;
                    case "right":
                        Cursor = Cursors.SizeWE;
                        break;
                    case "topLeft":
                        Cursor = Cursors.SizeNWSE;
                        break;
                    case "topRight":
                        Cursor = Cursors.SizeNESW;
                        break;
                    case "bottomLeft":
                        Cursor = Cursors.SizeNESW;
                        break;
                    case "bottomRight":
                        Cursor = Cursors.SizeNWSE;
                        break;
                }
        }
    }
}