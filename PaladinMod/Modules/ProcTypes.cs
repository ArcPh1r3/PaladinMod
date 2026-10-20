using R2API;

namespace PaladinMod.Modules
{
    public class ProcTypes
    {
        public static ModdedProcType SoulTetherShareProcType;

        public static void Init()
        {
            SoulTetherShareProcType = R2API.ProcTypeAPI.ReserveProcType();
        }
    }
}
