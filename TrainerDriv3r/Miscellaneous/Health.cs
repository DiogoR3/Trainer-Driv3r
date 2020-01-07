using System;
using System.Linq;

namespace TrainerDriv3r.Miscellaneous
{
    public static class Health
    {
        private const int BaseAddress = 0x004B8448;
        private const int Maximum = 0x3F800000;
        private const int MinimumVisible = 0x3B9ACA00;
        private const int MinimumPossible = 0x33000000;

        private static readonly int[] offsets_Health = new int[] { 0xC, 0x4, 0x8, 0x24 };
        public static void SetGivenHealth(ProcessMemory processMemory, int percentage)
        {
            int newHealth = CalculateHealth(percentage);

            byte[] percentageByte = BitConverter.GetBytes(newHealth);

            long baseAddress = processMemory.process.MainModule.BaseAddress.ToInt64() + BaseAddress;

            processMemory.Write(baseAddress, percentageByte, offsets_Health);
        }

        private static int CalculateHealth(int percentage)
        {
            int[] percentagePermitted = new int[] { 1, 25, 50, 75, 100 };

            if (!percentagePermitted.Contains(percentage))
                throw new ArgumentOutOfRangeException("Percentage has to be 1, 25, 50, 75 or 100");

            int newHealth = default;

            switch (percentage)
            {
                case 1:
                    newHealth = MinimumVisible;
                    break;
                case 25:
                    newHealth = 1048500000;
                    break;
                case 50:
                    newHealth = 1057000000;
                    break;
                case 75:
                    newHealth = 1060100000;
                    break;
                case 100:
                    newHealth = Maximum;
                    break;
            }

            return newHealth;
        }
    }
}
