using System;

namespace TrainerDriv3r.Weaponry
{
    public static class Ammo
    {
        public static void SetGivenAmmunition(ProcessMemory processMemory, Weapon weapon, int quantity)
        {
            int baseAddress = 0x004DA7F4;

            byte[] ammoByte = BitConverter.GetBytes(quantity);

            int offset = default;

            switch (weapon)
            {
                case Weapon.Beretta92F: offset = 0x4C; break;
                case Weapon.Beretta92F_Silenced: offset = 0x80; break;
                case Weapon.Heckler_KochMP5: offset = 0xE8; break;
                case Weapon.SPAS12: offset = 0x150; break;
                case Weapon.Uzi: offset = 0xB4; break;
                case Weapon.MAC11: offset = 0x1B8; break;
                case Weapon.M16: offset = 0x11C; break;
                case Weapon.M79_Grenade_Launcher: offset = 0x184; break;
            }

            long newAddrress = processMemory.process.MainModule.BaseAddress.ToInt64() + baseAddress;

            if (offset > 0)
                processMemory.Write(newAddrress, ammoByte, new int[] { offset });
        }

        public static void SetInfiniteAmmunition(ProcessMemory processMemory)
        {
            int baseAddress = 0x102179;
            long newAddrress = processMemory.process.MainModule.BaseAddress.ToInt64() + baseAddress;
            var assemblyNop = BitConverter.GetBytes(0x90);
            assemblyNop = BitConverter.GetBytes(0x4A);
            processMemory.Write((int)newAddrress, assemblyNop, 1);
        }

        private static void GetNewAdrress(int[] offset)
        {

        }
    }
}
