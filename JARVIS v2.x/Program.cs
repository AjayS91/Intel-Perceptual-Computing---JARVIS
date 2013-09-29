using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Jarvis
{
    internal class ProgramHandle
    {
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [STAThread]
        private static void Main()
        {
            try
            {
                #region //IE Handle

                /*
                string ie_path = @"C:\Program Files\Internet Explorer\iexplore.exe";
                string ie_win_class = "IEFrame";
                string ie_win_nm = "New Tab - Windows Internet Explorer";
                ExtProgramHandle ie_handle = new ExtProgramHandle(ie_path, ie_win_class, ie_win_nm);
                ie_handle.bring_to_ForeGround();
                ie_handle.set_show_state(ShowState.SW_FORCEMINIMIZE);
                Thread.Sleep(2000);
                ie_handle.exitApplication();
                Thread.Sleep(2000);
                */

                #endregion //IE Handle

                #region //Chrome Handle

                /*
                string chrom_path = @"C:\Users\Rahul Roy\AppData\Local\Google\Chrome\Application\chrome.exe";
                ExtProgramHandle chrome = new ExtProgramHandle(chrom_path, "Chrome_WidgetWin_1", "New Tab - Google Chrome");
                //MessageBox.Show(chrome.win_class + "   " + chrome.win_name + "   " + chrome.prog_path);
                //chrome.set_show_state(ShowState.SW_MINIMIZE);
                chrome.bring_to_ForeGround();
                chrome.p.Exited += new EventHandler(p_Exited);

                chrome.exitApplication();
                */

                #endregion //Chrome Handle

                #region //GUI MAIN APPLICATION

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                PXCMSession session = null;
                pxcmStatus sts = PXCMSession.CreateInstance(out session);

                if (sts >= pxcmStatus.PXCM_STATUS_NO_ERROR)
                {
                    MainScreen mscreen = new MainScreen(session);
                    MainFormGR GR = new MainFormGR(session, mscreen);
                    MainFormVR VR = new MainFormVR(session, mscreen);
                    mscreen.binding(GR, VR);
                    //mouse query thread
                    JarvisMouse myMouse = new JarvisMouse(mscreen);
                    Thread mouse_worker = new Thread(myMouse.query_mouse);
                    mouse_worker.Start();

                    Application.Run(mscreen);
                    try
                    {
                        mouse_worker.Abort();
                        GR.Dispose();
                        VR.Dispose();
                        mscreen.Dispose();
                        session.Dispose();
                        Thread.Sleep(50);
                    }
                    catch (Exception i)
                    {
                    }
                    finally
                    {
                        Application.Exit();
                    }
                }

                #endregion //GUI MAIN APPLICATION
            }
            catch (Exception e)
            {
            }
        }
    }
}