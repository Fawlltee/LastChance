using BepInEx;
using BepInEx.Logging;
using Comfort.Common;
using EFT;
using UnityEngine;

namespace LastChanceSlowMo
{
    [BepInPlugin("com.Fawll.lastchanceslowmo", "Last Chance SlowMo", "1.0.0")]
    [BepInProcess("EscapeFromTarkov.exe")]
    public class Plugin : BaseUnityPlugin
    {
        private static ManualLogSource Logger;

        private void Awake()
        {
            Logger = BepInEx.Logging.Logger.CreateLogSource("LastChanceSlowMo");
            Logger.LogInfo("Last Chance SlowMo Mod Loading");

            LastChanceBulletTime.Enable();

            DamageHandler.Enable();

            Logger.LogInfo("Last Chance SlowMo Mod Loaded");
        }
    }
}
