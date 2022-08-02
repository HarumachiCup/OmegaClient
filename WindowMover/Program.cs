using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ResizeTourneyClientWindows
{
    class Program
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT Rect);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

        static void Main(string[] args)
        {
            List<Process> processes = Process.GetProcessesByName("osu!").ToList();
            Console.WriteLine(processes[0]);
            //processes.Add(Process.GetProcessesByName("osu!tourney Tournament Client 1")[0]);
            int count = 0;
            foreach (Process p in processes)
            {
                if (!p.MainWindowTitle.Contains("Manager"))
                {
                    IntPtr handle = p.MainWindowHandle;
                    RECT Rect = new RECT();
                    if (GetWindowRect(handle, ref Rect))
                    {
                        int width = Rect.right - Rect.left;
                        int height = Rect.bottom - Rect.top;
                        MoveWindow(handle, -1024, count, width, height, true);
                    }
                    count += 600;
                }
                else
                {

                }
            }

        }
    }
}
