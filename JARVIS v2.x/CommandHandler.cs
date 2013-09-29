using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace Jarvis
{
    internal class CommandHandler
    {
        private ExtProgramHandle[] ext_prog = new ExtProgramHandle[3];
        private static int last_word_length;

        public CommandHandler()
        {

        }

        private static string win_name, sPatternMap = "\bmap\b", sPatternWord = "\bword\b";
        private static PXCMGesture.Gesture[] prevGesture = new PXCMGesture.Gesture[2];

        public static void gestureHandler(PXCMGesture.Gesture[] x)
        {
            win_name = ExtProgramHandle.GetActiveWindowTitle();

            for (int i = 0; i < 2; i++)
            {
                //Map Gesture Handles zoom
                if (win_name!=null
                    && System.Text.RegularExpressions.Regex.IsMatch(win_name, sPatternMap, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                {
                    if (x[i].label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_DOWN)
                    {
                        ExtProgramHandle.pass_string("-");
                    }
                    else if (x[i].label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP)
                    {
                        ExtProgramHandle.pass_string("+");
                    }
                }

                //Deafult
                //Regular Mode Scroll
                if (x[i].label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_DOWN)
                {
                    ExtProgramHandle.pass_string("{PGDN}");
                }
                else if (x[i].label == PXCMGesture.Gesture.Label.LABEL_POSE_THUMB_UP)
                {
                    ExtProgramHandle.pass_string("{PGUP}");
                }

                //Minimize Maximize
                if (x[i].label == PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_DOWN)
                {
                    ExtProgramHandle.set_show_state(ShowState.SW_MINIMIZE);
                }
                else if (x[i].label == PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_UP)
                {
                    ExtProgramHandle.restore_prev_win(ShowState.SW_RESTORE);
                }

                //go back
                if (x[i].label == PXCMGesture.Gesture.Label.LABEL_NAV_SWIPE_LEFT)
                    ExtProgramHandle.pass_string("{BACKSPACE}");

                //SingleClick
                if (x[i].label == PXCMGesture.Gesture.Label.LABEL_POSE_BIG5 & prevGesture[i].label != PXCMGesture.Gesture.Label.LABEL_POSE_BIG5)
                {
                    //Point m_point= (Point)GestureRecognition.m_poin_que.Dequeue();
                   // JarvisMouse.modify_mouse(m_point);
                    JarvisMouse.leftClick();
                }

                //DoubleClick
                if (x[i].label == PXCMGesture.Gesture.Label.LABEL_POSE_PEACE & prevGesture[i].label != PXCMGesture.Gesture.Label.LABEL_POSE_PEACE)
                    JarvisMouse.mousDoubleClick();

                /*
                //Drag&Drop Check
                if (x[i].label == PXCMGesture.Gesture.Label.LABEL_POSE_BIG5)
                    counter++;
                else
                    counter = 0;

                //DragfDropExecute
                if (counter > 500)
                {
                    drag = true;
                    JarvisMouse.Mouse_Event(Mouse_Evnt.Left_button_Down);
                }
                else if (counter == 0)
                    JarvisMouse.Mouse_Event(Mouse_Evnt.Left_button_Up);
                */
            }

            prevGesture[0] = x[0];
            prevGesture[1] = x[1];
        }

        public static void parseCommand(string command)
        {
            string[] com_array = command.Split(' ');

            const string com_init = "Love";
            const string search_it = "like";

            if (com_array[0].Equals(com_init))
            {
                switch (com_array[1])
                {
                    case search_it:
                        //string chrom_path = @"C:\Users\Rahul Roy\AppData\Local\Google\Chrome\Application\chrome.exe";
                        //ExtProgramHandle chrome = new ExtProgramHandle(chrom_path, "Chrome_WidgetWin_1", "New Tab - Google Chrome");

                        string chrom_path = "iexplorer.exe";
                        ExtProgramHandle chrome = new ExtProgramHandle(chrom_path,null,null);
                        chrome.start_Program();
                        chrome.bring_to_ForeGround();
                        Thread.Sleep(50);

                        for (int i = 1; i < com_array.Length; i++)
                        {
                            SendKeys.SendWait(com_array[i]);
                            last_word_length = com_array[i].Length;
                        }
                        break;

                    case "BackSpace":
                        for (int i = 0; i < last_word_length; i++)
                        {
                            SendKeys.SendWait("{BACKSPACE}");
                        }
                        break;

                    case "Dictation":
                        for (int i = 1; i < com_array.Length; i++)
                        {
                            SendKeys.SendWait(com_array[i]);
                            last_word_length = com_array[i].Length;
                        }
                        break;
                }
            }
        }
    }

    internal enum comandHandlerState
    {
        Dictation,
        Browsing,
    }
}