using System;
using BepInEx;
using BepInEx.Logging;
using Comfort.Common;
using EFT;
using UnityEngine;

namespace LastChanceSlowMo
{
    internal class DamageHandler : MonoBehaviour
    {
        internal static Player player;
        private LastChanceBulletTime bulletTimeComponent;

        protected static ManualLogSource Logger
        {
            get; private set;
        }

        private DamageHandler()
        {
            if (Logger == null)
            {
                Logger = BepInEx.Logging.Logger.CreateLogSource(nameof(DamageHandler));
                Logger.LogInfo("Damage Handler Mod Loading");
            }
        }

        internal static void Enable()
        {
            if (Singleton<IBotGame>.Instantiated)
            {
                var gameWorld = Singleton<GameWorld>.Instance;
                gameWorld.GetOrAddComponent<DamageHandler>();
                Logger.LogDebug("DamageHandler enabled");
            }
        }

        private void Start()
        {
            player = Singleton<GameWorld>.Instance.MainPlayer;
            bulletTimeComponent = player.GetComponent<LastChanceBulletTime>();
        }

        public void OnPlayerDamaged(Player attacker)
        {
            if (bulletTimeComponent != null)
            {
                float duration = UnityEngine.Random.Range(7f, 12f);
                bulletTimeComponent.TriggerBulletTime(duration);
            }
        }
    }
}
