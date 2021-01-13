using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Devinno.Tools
{
    public class LauncherTool
    {
        #region IsAdministrator
        /// <summary>
        /// 관리자 권한 여부
        /// </summary>
        [SupportedOSPlatform("windows")]
        public static bool IsAdministrator
        {
            get
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                if (null != identity)
                {
                    WindowsPrincipal principal = new WindowsPrincipal(identity);
                    return principal.IsInRole(WindowsBuiltInRole.Administrator);
                }
                return false;
            }
        }
        #endregion

        #region Start( 프로그램명, 정상콜백, 비정상콜백, 중복실행, 관리자, 지연 )
        /// <summary>
        /// 프로그램 실행 도움
        /// * 디버그시엔 관리자 모드 사용불가
        /// </summary>
        /// <param name="ProgramName">프로그램 명</param>
        /// <param name="run">정상 콜백</param>
        /// <param name="faild">비정상 콜백</param>
        /// <param name="UseDuplicate">중복실행 여부</param>
        /// <param name="UseAdministrator">관리자실행 여부</param>
        /// <param name="Delay">지연</param>
        [SupportedOSPlatform("windows")]
        public static void Start(string ProgramName, Action run, Action faild, bool UseDuplicate, bool UseAdministrator, int Delay = 0)
        {
            if (UseAdministrator)
            {
                if (UseDuplicate)
                {
                    if (!IsAdministrator)
                    {
                        #region Admin
                        try
                        {
                            ProcessStartInfo procInfo = new ProcessStartInfo();
                            procInfo.UseShellExecute = true;
                            procInfo.FileName = Assembly.GetEntryAssembly().Location;
                            procInfo.WorkingDirectory = Environment.CurrentDirectory;
                            procInfo.Verb = "runas";
                            Process.Start(procInfo);
                        }
                        catch (Exception ex) { }
                        #endregion
                    }
                    else
                    {
                        #region Duplicate
                        bool bnew;
                        Mutex mutex = new Mutex(true, ProgramName, out bnew);

                        if (bnew)
                        {
                            if (Delay != 0) System.Threading.Thread.Sleep(Delay);
                            run?.Invoke();
                            mutex.ReleaseMutex();
                        }
                        else
                        {
                            faild?.Invoke();
                        }
                        #endregion
                    }
                }
                else
                {
                    if (!IsAdministrator)
                    {
                        #region Admin
                        try
                        {

                            ProcessStartInfo procInfo = new ProcessStartInfo();
                            procInfo.UseShellExecute = true;
                            //procInfo.FileName = Application.ExecutablePath;
                            procInfo.FileName = Assembly.GetEntryAssembly().Location;
                            procInfo.WorkingDirectory = Environment.CurrentDirectory;
                            procInfo.Verb = "runas";
                            Process.Start(procInfo);
                        }
                        catch (Exception ex) { }
                        #endregion
                    }
                    else
                    {
                        #region Run
                        if (Delay != 0) System.Threading.Thread.Sleep(Delay);
                        run?.Invoke();
                        #endregion
                    }
                }
            }
            else
            {
                if (UseDuplicate)
                {
                    #region Duplication
                    bool bnew;
                    Mutex mutex = new Mutex(true, ProgramName, out bnew);
                    if (bnew)
                    {
                        if (Delay != 0) System.Threading.Thread.Sleep(Delay);
                        run?.Invoke();
                        mutex.ReleaseMutex();
                    }
                    else
                    {
                        faild?.Invoke();
                    }
                    #endregion
                }
                else
                {
                    #region Run
                    if (Delay != 0) System.Threading.Thread.Sleep(Delay);
                    run?.Invoke();
                    #endregion
                }
            }
        }
        #endregion

        #region Start( 프로그램명, 정상콜백, 비정상콜백, 중복실행, 지연 )
        /// <summary>
        /// 프로그램 실행 도움
        /// * 디버그시엔 관리자 모드 사용불가
        /// </summary>
        /// <param name="ProgramName">프로그램 명</param>
        /// <param name="run">정상 콜백</param>
        /// <param name="faild">비정상 콜백</param>
        /// <param name="UseDuplicate">중복실행 여부</param>
        /// <param name="Delay">지연</param>
        public static void Start(string ProgramName, Action run, Action faild, bool UseDuplicate, int Delay = 0)
        {
            if (UseDuplicate)
            {
                #region Duplication
                bool bnew;
                Mutex mutex = new Mutex(true, ProgramName, out bnew);
                if (bnew)
                {
                    if (Delay != 0) System.Threading.Thread.Sleep(Delay);
                    run?.Invoke();
                    mutex.ReleaseMutex();
                }
                else
                {
                    faild?.Invoke();
                }
                #endregion
            }
            else
            {
                #region Run
                if (Delay != 0) System.Threading.Thread.Sleep(Delay);
                run?.Invoke();
                #endregion
            }
        }
        #endregion
    }
}
