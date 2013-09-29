using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;

namespace Jarvis
{
    internal class JarvisMouse
    {
        private static DateTime clickInit, timeElapsed;

        private static int counter = 0;

        private static TimeSpan threshold = new TimeSpan(3);

        private Point _point;

        private MainScreen mScreen;

        public JarvisMouse(MainScreen mscreen)
        {
            mScreen = mscreen;
        }

        //MOUSE
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        public static void leftClick()
        {
            Mouse_Event(Mouse_Evnt.Left_button_Down);
            Mouse_Event(Mouse_Evnt.Left_button_Up);
        }

        public static void modify_mouse(Point _point)
        {
            try
            {
                SetCursorPosition(_point.X, _point.Y);
                Thread.Sleep(10);
            }
            catch (Exception e)
            {//nothing
            }
        }

        public static void mousDoubleClick()
        {
            if (counter == 0)
            {
                JarvisMouse.leftClick();
                clickInit = DateTime.Now;
            }

            counter++;
            timeElapsed = DateTime.Now;

            if (timeElapsed - clickInit > threshold)
            {
                counter = 0;
                JarvisMouse.leftClick();
                return;
            }
            mousDoubleClick();
        }

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public static void Mouse_Event(Mouse_Evnt e)
        {
            Point p = GetCursorPosition();
            mouse_event((int)e, p.X, p.Y, 0, 0);
        }

        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        public static void SingleleftClick()
        {
            if (counter == 0)
            {
                Mouse_Event(Mouse_Evnt.Left_button_Down);
                clickInit = DateTime.Now;
            }

            counter++;
            timeElapsed = DateTime.Now;
            if (timeElapsed - clickInit > threshold)
            {
                counter = 0;
                Mouse_Event(Mouse_Evnt.Left_button_Up);
                return;
            }
            mousDoubleClick();
        }

        public void query_mouse()
        {
            _point = GetCursorPosition();

            if (mScreen != null)
                try
                {
                    mScreen.Invoke(new Action(
                        delegate()
                        {
                            mScreen.label2.Text = " X : " + _point.X;
                            mScreen.label3.Text = " Y : " + _point.Y;
                        }));
                }
                catch (Exception e)
                {
                    //MessageBox.Show("Jarvis Mouse_Event failed ToolBar update MainScreen");
                    //Application.Exit();
                }
            Thread.Sleep(500);
            query_mouse();
        }

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }
    }
}

public enum Mouse_Evnt : int
{
    Left_button_Down = 2,
    Left_button_Up = 4,
    Right_button_Down = 8,
    Right_button_Up = 16
}