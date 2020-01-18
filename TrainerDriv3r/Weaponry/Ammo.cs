using System;
using TrainerDriv3r.Extensions;

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

            long newAddrress = processMemory.BaseToLong() + baseAddress;

            if (offset > 0)
                processMemory.Write(newAddrress, ammoByte, new int[] { offset });
        }

        public static void SetInfiniteAmmunition(ProcessMemory processMemory, bool isInfinite)
        {
            int baseAddress = 0x102179;
            long newAddrress = processMemory.BaseToLong() + baseAddress;

            byte[] assemblyInstruction;

            if(isInfinite)
                assemblyInstruction = BitConverter.GetBytes(0x90);
            else
                assemblyInstruction = BitConverter.GetBytes(0x4A);

            processMemory.Write((int)newAddrress, assemblyInstruction, 1);
        }
    }
}
