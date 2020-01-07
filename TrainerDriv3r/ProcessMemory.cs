﻿using System;
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
            foreach (var offset in offsets)
                baseAddress = ReadInt32(process, (uint)baseAddress) + offset;

            return baseAddress;
        }

        private int ReadInt32(IntPtr hProcess, uint dwAddress)
        {
            byte[] buffer = new byte[4];
            ReadProcessMemory(hProcess, (IntPtr)dwAddress, buffer, 4, out IntPtr bytesread);
            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
