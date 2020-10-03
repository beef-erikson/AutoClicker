using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoClicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Timer min and max values
        const float MINTIME = 1;
        const float MAXTIME = 360000;

        // Timer
        static System.Timers.Timer theTimer;

        // Mouse click fields
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        //Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        // Mouse coordinate fields
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool GetCursorPos(ref Win32Point pt);

        [StructLayout(LayoutKind.Sequential)]
        
        // Mouse points
        internal struct Win32Point
        {
            public Int32 X;
            public Int32 Y;
        };


        /// <summary>
        /// Main method
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }


        /// <summary>
        /// Gets current mouse position in X, Y
        /// </summary>
        /// <returns>Mouse point</returns>
        public static Point GetMousePosition()
        {
            var w32Mouse = new Win32Point();
            GetCursorPos(ref w32Mouse);

            return new Point(w32Mouse.X, w32Mouse.Y);
        }


        /// <summary>
        /// Forces mouse click
        /// </summary>
        private static void DoMouseClick()
        {
            // Call the imported function with the cursor's current position
            Point mousePoint = GetMousePosition();
            uint X = (uint)mousePoint.X;
            uint Y = (uint)mousePoint.Y;
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }


        /// <summary>
        /// Handles timer checking and button labels.
        /// Passes to StartTimer.
        /// </summary>
        private void ProcessTimer()
        {
            // "Start" has been clicked
            if ((string)TimerButton.Content == "Start")
            {
                // Change button label
                TimerButton.Content = "Stop";

                // Checks to make sure value is float and within parameters before passing to StartTimer
                float f;

                if (float.TryParse(TimerTextBox.Text, out f) && f >= MINTIME && f <= MAXTIME)
                    StartTimer(f);
                else
                    MessageBox.Show($"Value must be a valid number between {MINTIME:n0} and {MAXTIME:n0}.", "Error");
            }
            // "Stop" has been clicked
            else
            {
                TimerButton.Content = "Start";
                StopTimer();
            }
        }


        /// <summary>
        /// Starts the timer / mouse clicking.
        /// </summary>
        /// <param name="setTime">Amount of time in between clicks</param>
        private void StartTimer(float setTime)
        {
            // Sets and starts timer
            theTimer = new System.Timers.Timer(setTime);
            theTimer.Elapsed += ClickMouse;
            theTimer.Enabled = true;
        }


        /// <summary>
        /// Stops timer / mouse clicking.
        /// </summary>
        private void StopTimer()
        {
            theTimer.Stop();
            theTimer.Dispose();
        }


        /// <summary>
        /// Processes the mouse clicks.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e">Elapsed time</param>
        private static void ClickMouse(Object source, ElapsedEventArgs e)
        {
            Debug.WriteLine("{0:HH:mm:ss.fff}", e.SignalTime);
            DoMouseClick();
        }


        /// <summary>
        /// Button has been clicked; start timer and change text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProcessTimer();
        }
    }
}
