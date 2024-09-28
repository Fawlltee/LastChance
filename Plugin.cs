using System;
using System.Linq;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.Logging;
using Comfort.Common;
using EFT;
using EFT.UI;
using UnityEngine;

namespace LastChanceSlowMo
{
    internal class LastChanceBulletTime : MonoBehaviour
    {
        internal static Player player;
        internal static bool startBulletTime = false;
        private float bulletTimeDuration = 0f;

        protected static ManualLogSource Logger
        {
            get; private set;
        }

        private LastChanceBulletTime()
        {
            if (Logger == null)
            {
                Logger = BepInEx.Logging.Logger.CreateLogSource(nameof(LastChanceBulletTime));
                Logger.LogInfo("Last Chance Mod Loading");
            }
        }

        internal static void Enable()
        {
            if (Singleton<IBotGame>.Instantiated)
            {
                var gameWorld = Singleton<GameWorld>.Instance;
                gameWorld.GetOrAddComponent<LastChanceBulletTime>();
                Logger.LogDebug("LastChanceBulletTime enabled");
            }
        }

        private void Start()
        {
            player = Singleton<GameWorld>.Instance.MainPlayer;
            startBulletTime = false;
        }

        public async Task Update()
        {
            if (!BulletTime.PluginEnabled.Value)
            {
                return;
            }

            CheckHealthAndTriggerBulletTime();
        }

        private void CheckHealthAndTriggerBulletTime()
        {
            if (player == null || !player.IsAlive)
            {
                StopBulletTime();
                return;
            }

            var commonHealth = player.HealthController.GetBodyPartHealth(EBodyPart.Common).Current;
            var headHealth = player.HealthController.GetBodyPartHealth(EBodyPart.Head).Current;
            var lightBleed = player.HealthController.GetDamageType(EDamageType.LightBleeding) > 0;
            var heavyBleed = player.HealthController.GetDamageType(EDamageType.HeavyBleeding) > 0;

            if ((commonHealth <= 50 && !lightBleed && !heavyBleed) || headHealth <= 20)
            {
                if (!startBulletTime)
                {
                    if (commonHealth <= 20 || headHealth <= 5)
                    {
                        bulletTimeDuration = UnityEngine.Random.Range(7f, 12f) * 1.5f;
                    }
                    else
                    {
                        bulletTimeDuration = UnityEngine.Random.Range(7f, 12f);
                    }
                    StartBulletTime();
                }
            }
            else
            {
                if (startBulletTime)
                {
                    StopBulletTime();
                }
            }
        }

        private async void StartBulletTime()
        {
            startBulletTime = true;
            Time.timeScale = BulletTime.BulletTimeScale.Value;
            await Task.Delay(TimeSpan.FromSeconds(bulletTimeDuration));
            StopBulletTime();
        }

        private void StopBulletTime()
        {
            startBulletTime = false;
            Time.timeScale = 1.0f;
        }
    }
}
