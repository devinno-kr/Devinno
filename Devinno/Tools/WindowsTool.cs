using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Tools
{
    [SupportedOSPlatform("windows")]
    public class WindowsTool
    {
        #region SetLocalTime
        public static void SetLocalTime(DateTime Time)
        {
            var v = new SYSTEMTIME()
            {
                wYear = (short)Time.Year,
                wMonth = (short)Time.Month,
                wDay = (short)Time.Day,
                //wDayOfWeek = (short)Time.DayOfWeek,
                wHour = (short)Time.Hour,
                wMinute = (short)Time.Minute,
                wSecond = (short)Time.Second,
                wMilliseconds = (short)Time.Millisecond,
            };
            Win32Tool.SetLocalTime(ref v);
        }
        #endregion
        #region SetSystemTime
        public static void SetSystemTime(DateTime Time)
        {
            var v = new SYSTEMTIME()
            {
                wYear = (short)Time.Year,
                wMonth = (short)Time.Month,
                wDay = (short)Time.Day,
                //wDayOfWeek = (short)Time.DayOfWeek,
                wHour = (short)Time.Hour,
                wMinute = (short)Time.Minute,
                wSecond = (short)Time.Second,
                wMilliseconds = (short)Time.Millisecond,
            };
            Win32Tool.SetSystemTime(ref v);
        }
        #endregion

        #region PreventSleep
        /// <summary>
        /// 절점모드 중지
        /// </summary>
        public static void PreventSleep()
        {
            Win32Tool.SetThreadExecutionState(ExecutionStateKinds.ES_CONTINUOUS |
                                                ExecutionStateKinds.ES_SYSTEM_REQUIRED |
                                                ExecutionStateKinds.ES_AWAYMODE_REQUIRED |
                                                ExecutionStateKinds.ES_DISPLAY_REQUIRED);
        }
        #endregion
        #region AllowSleep
        /// <summary>
        /// 절전모드 재개
        /// </summary>
        public static void AllowSleep()
        {
            Win32Tool.SetThreadExecutionState(ExecutionStateKinds.ES_CONTINUOUS);
        }
        #endregion
    }

    [SupportedOSPlatform("windows")]
    public class Win32Tool
    {
        #region Const
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONDBLCLK = 0xA3;
        public const int HT_CAPTION = 0x2;
        #endregion

        #region Interop
        [DllImportAttribute("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessagePC(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImportAttribute("user32.dll", EntryPoint = "ReleaseCapture")]
        public static extern bool ReleaseCapturePC();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern ExecutionStateKinds SetThreadExecutionState(ExecutionStateKinds esFlags);

        [DllImport("kernel32.dll")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetSystemTime(ref SYSTEMTIME st);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetLocalTime(ref SYSTEMTIME st);
        #endregion

        #region Method
        public static int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix) return -1;
            else return SendMessagePC(hWnd, Msg, wParam, lParam);
        }
        public static bool ReleaseCapture()
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix) { return false; }
            else return ReleaseCapturePC();
        }
        #endregion
    }

    #region enum : ExecutionStateKinds 
    [SupportedOSPlatform("windows")]
    [FlagsAttribute]
    public enum ExecutionStateKinds : uint
    {
        ES_AWAYMODE_REQUIRED = 0x00000040,
        ES_CONTINUOUS = 0x80000000,
        ES_DISPLAY_REQUIRED = 0x00000002,
        ES_SYSTEM_REQUIRED = 0x00000001
    }
    #endregion

    #region struct : SYSTEMTIME
    [SupportedOSPlatform("windows")]
    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEMTIME
    {
        public short wYear;
        public short wMonth;
        public short wDayOfWeek;
        public short wDay;
        public short wHour;
        public short wMinute;
        public short wSecond;
        public short wMilliseconds;
    }
    #endregion
}
