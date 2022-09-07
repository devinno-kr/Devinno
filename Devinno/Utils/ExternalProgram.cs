using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Devinno.Tool
{
    public class ExternalProgram
    {
        public Process Process { get; private set; } = null;

        public string Path { get; private set; }
        public string ProcessName { get; private set; }

        public ExternalProgram(string PATH)
        {
            this.Path = PATH;
            this.ProcessName = System.IO.Path.GetFileNameWithoutExtension(PATH);
        }

        public void Start()
        {
            var pss = Process.GetProcesses();

            var tps = pss.Where(x => x.ProcessName.ToLower() == ProcessName).FirstOrDefault();

            if (tps != null) Process = tps;
            else
            {
                Process = new Process();
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = Path;
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
                info.RedirectStandardOutput = true;
                info.RedirectStandardInput = true;
                info.RedirectStandardError = true;

                Process.StartInfo = info;
                Process.Start();
            }
        }

        public void Stop()
        {
            if (Process != null)
            {
                if (!Process.HasExited) Process.Kill();
                Process.Close();
                Process.Dispose();
                Process = null;
            }
        }
    }
}
