using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZankyouService
{
    public static class Monitor
    {
        /// <summary>
        /// Gets the current processes on the current system.
        /// </summary>
        /// <returns>Returns a list of SysProcess contained on this system.</returns>
        public static List<SysProcess> GetSystemProcesses()
        {
            List<SysProcess> processList = new List<SysProcess>();

            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                processList.Add(new SysProcess {
                    ProcesName = process.ProcessName,
                    ProcessMemory = process.VirtualMemorySize64
                });
            }

            return processList;
        }

        /// <summary>
        /// Gets the cpu and memory info of the current system.
        /// </summary>
        /// <returns>returns a comma separated value indicating cpu usage and memory info (available, total).</returns>
        public static string GetSystemUsage()
        {
            int cpuUsage = GetCpuUsage();
            int memAvailabe = GetMemoryAvailable();
            int memTotal = GetMemoryTotal();
            return cpuUsage + "," + memAvailabe + "," + memTotal;
        }

        private static int GetCpuUsage()
        {
            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            cpuCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            return (int)cpuCounter.NextValue();
        }

        /// <summary>
        /// Returns the total memory in megabytes
        /// </summary>
        private static int GetMemoryTotal()
        {
            return (int) (new Microsoft.VisualBasic.Devices.ComputerInfo().TotalPhysicalMemory / Math.Pow(1024, 2));
        }

        /// <summary>
        /// Gets the total Available memory in megabytes
        /// </summary>
        private static int GetMemoryAvailable()
        {
            var memCounter = new PerformanceCounter("Memory", "Available MBytes");
            memCounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            return (int) memCounter.NextValue();
        }
    }
}
