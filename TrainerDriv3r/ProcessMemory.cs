using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TrainerDriv3r
{
    public class ProcessMemory
    {
        public Process process { get; private set; }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr process, IntPtr baseAddress, [Out] byte[] buffer, int size, out IntPtr bytesRead);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

        public ProcessMemory(Process process)
        {
            this.process = process;
        }

        public float Read(long baseAddress, int[] offsets)
        {
            long foundAddress = GetTrueAddress(process.Handle, baseAddress, offsets);
            byte[] buffer = new byte[4];
            ReadProcessMemory(process.Handle, (IntPtr)foundAddress, buffer, buffer.Length, out IntPtr bytesRead);
            return BitConverter.ToSingle(buffer);
        }

        public void Write(long baseAddress, byte[] bytesToWrite, int[] offsets)
        {
            long foundAddress = GetTrueAddress(process.Handle, baseAddress, offsets);
            WriteProcessMemory(process.Handle, (IntPtr)foundAddress, bytesToWrite, (uint)bytesToWrite.Length, out int bytesWritten);
        }

        public void Write(int baseAddress, byte[] bytesToWrite, uint nSize)
        {
            WriteProcessMemory(process.Handle, (IntPtr)baseAddress, bytesToWrite, nSize, out int bytesWritten);
        }

        private long GetTrueAddress(IntPtr process, long baseAddress, int[] offsets)
        {
            foreach(var offset in offsets)
            {
                byte[] buffer = new byte[4];
                ReadProcessMemory(process, (IntPtr)baseAddress, buffer, buffer.Length, out IntPtr bytesRead);
                baseAddress = BitConverter.ToInt32(buffer, 0) + offset;
            }

            return baseAddress;
        }
    }
}
