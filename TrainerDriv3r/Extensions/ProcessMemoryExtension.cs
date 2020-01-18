namespace TrainerDriv3r.Extensions
{
    public static class ProcessMemoryExtension
    {
        public static long BaseToLong(this ProcessMemory processMemory)
        {
            return processMemory.process.MainModule.BaseAddress.ToInt64();
        }
    }
}
