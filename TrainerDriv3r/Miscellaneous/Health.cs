using System;
using System.Collections.Generic;
using System.Linq;
using TrainerDriv3r.Extensions;

namespace TrainerDriv3r.Miscellaneous
{
    public static class Health
    {
        private const int BaseAddress = 0x004B8448;
        private const int nop = 0x90;
        private static readonly byte[] nopInstruction = new byte[] { nop, nop, nop };
        private static readonly int[] offsetsHealth = new int[] { 0xC, 0x4, 0x8, 0x24 };

        public static float GetHealth(ProcessMemory processMemory)
        {
            long baseAddress = processMemory.BaseToLong() + BaseAddress;
            float health = processMemory.Read(baseAddress, offsetsHealth);
            return health * 100;
        }

        public static void SetGivenHealth(ProcessMemory processMemory, int percentage)
        {
            if (percentage > 100 ^ percentage < 0)
                throw new ArgumentOutOfRangeException("Percentage has to be between 0 and 100.");

            byte[] percentageByte = BitConverter.GetBytes(percentage / 100f);

            long baseAddress = processMemory.BaseToLong() + BaseAddress;

            processMemory.Write(baseAddress, percentageByte, offsetsHealth);
        }

        public static void AddHealth(ProcessMemory processMemory, int health)
        {
            float actualHealth = GetHealth(processMemory);

            if (actualHealth >= 100)
                return;

            SetGivenHealth(processMemory, (int)actualHealth + health);
        }

        public static void PlayerCrashVehicleDamage(ProcessMemory processMemory, bool enabled)
        {
            int carDamageBaseAddress = 0xEC7F3;
            long newCarDamageAddress = processMemory.BaseToLong() + carDamageBaseAddress;

            byte[] assemblyInstruction;

            if (enabled)
                assemblyInstruction = new byte[] { 0xD9, 0x56, 0x24 };
            else
                assemblyInstruction = nopInstruction;

            processMemory.Write((int)newCarDamageAddress, assemblyInstruction, (uint)assemblyInstruction.Length);
        }

        public static void ExplosionDamage(ProcessMemory processMemory, bool enabled)
        {
            int instructionCounter = 3;

            long[] explosionDamageAddress = { 0xECE52, 0xEC7F3, 0xEC805 };
            byte?[,] assemblyOriginalInstructions =
            {
                { 0xD9, 0x5E, 0x24, null },
                { 0xD9, 0x56, 0x24, null },
                { 0xC7, 0x46, 0x24, 0 }
            };

            byte?[,] assemblyModifiedInstructions =
            {
                { nop, nop, nop, null, null, null, null },
                { nop, nop, nop, null, null, null, null },
                { nop, nop, nop, nop, nop, nop , nop }
            };

            while (instructionCounter-- > 0)
            {
                explosionDamageAddress[instructionCounter] += processMemory.BaseToLong();

                byte[] assemblyInstruction = enabled ?
                    GetByteArray(assemblyOriginalInstructions, instructionCounter) :
                    GetByteArray(assemblyModifiedInstructions, instructionCounter);

                processMemory.Write((int)explosionDamageAddress[instructionCounter], assemblyInstruction, (uint)assemblyInstruction.Length);
            }

        }

        private static byte[] GetByteArray(byte?[,] assemblyInstructions, int instructionCounter)
        {
            byte?[] assemblyInstruction = Enumerable.Range(0, assemblyInstructions.GetLength(1))
                       .Select(x => assemblyInstructions[instructionCounter, x])
                       .ToArray();

            List<byte> byteList = new List<byte>();

            foreach (var @byte in assemblyInstruction)
            {
                if (@byte != null)
                    byteList.Add((byte)@byte);
            }

            return byteList.ToArray();
        }

        public static void ShotDamage(ProcessMemory processMemory, bool enabled)
        {
            int shotBaseAddress = 0xECAE6;
            long newShotDamageAddress = processMemory.BaseToLong() + shotBaseAddress;

            byte[] assemblyInstruction = enabled ? 
                new byte[] { 0xD9, 0x56, 0x24 } :
                nopInstruction;

            processMemory.Write((int)newShotDamageAddress, assemblyInstruction, (uint)assemblyInstruction.Length);
        }
    }
}
