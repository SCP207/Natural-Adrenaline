using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using NaturalAdrenaline.Enums;
using System.Collections.Generic;
using UnityEngine;

using static Exiled.Events.Handlers.Player;
using static Exiled.Events.Handlers.Server;

namespace NaturalAdrenaline.Handlers {
    public static class EventHandlers {
        public static void Register() {
            RoundStarted += OnRoundStarted;
            Hurt += OnPlayerHurt;
        }

        public static void Unregister() {
            RoundStarted -= OnRoundStarted;
            Hurt -= OnPlayerHurt;
        }

        #region Server Events
        private static void OnRoundStart() {
            Plugin.Instance.CanGetAdrenaline.Clear();
            Timing.KillCoroutines();
        }
        #endregion

        #region Player Events
        private static void OnPlayerHurt(HurtEventArgs ev) {
            if (Plugin.Instance.Config.AllowScps && ev.Player.IsScp) return;

            if (Plugin.Instance.CanGetAdrenaline.Contains(ev.Player)) return;
            Plugin.Instance.CanGetAdrenaline.Add(ev.Player);

            var AHP = GetAmount(ev.Amount, Plugin.Instance.Config.AhpRateOfChange);

            if (ev.Player.ArtificialHealth + AHP < Plugin.Instance.Config.MaxAhp)
                ev.Player.AddAhp(AHP);
            else if (ev.Player.ArtificialHealth < Plugin.Instance.Config.MaxAhp)
                ev.Player.ArtificialHealth = Plugin.Instance.Config.MaxAhp;

            var MB = (byte)Mathf.Clamp(GetAmount(ev.Amount, Plugin.Instance.Config.MbRateOfChange), 0, 255);
            var oldIntensity = ev.Player.GetEffect(EffectType.MovementBoost).Intensity;

            if (oldIntensity + MB < Plugin.Instance.Config.MaxIntensity)
                ev.Player.ChangeEffectIntensity(EffectType.MovementBoost,
                    (byte)Mathf.Clamp(oldIntensity + MB, 0, 255),
                    Plugin.Instance.Config.EffectDuration);
            else if (oldIntensity < Plugin.Instance.Config.MaxIntensity)
                ev.Player.ChangeEffectIntensity(EffectType.MovementBoost,
                    Plugin.Instance.Config.MaxIntensity,
                    Plugin.Instance.Config.EffectDuration);

            Timing.RunCoroutine(AllowReceivingAdrenaline(ev.Player));
        }
        #endregion

        #region Math
        private static float GetAmount(float damage, float rateOfChange) => Plugin.Instance.Config.ScalingType switch {
            ScalingType.Linear =>
                rateOfChange * (damage * Plugin.Instance.Config.Multiplier),
            ScalingType.Exponential =>
                Mathf.Pow(rateOfChange, damage * Plugin.Instance.Config.Multiplier) - 1,
            ScalingType.Logrithmic =>
                Mathf.Log(damage * Plugin.Instance.Config.Multiplier + 1, rateOfChange)
        };
        #endregion

        #region Coroutines
        private static IEnumerator<float> AllowReceivingAdrenaline(Player player) {
            yield return Timing.WaitForSeconds(Plugin.Instance.Config.Duration);

            if (Plugin.Instance.CanGetAdrenaline.Contains(player))
                Plugin.Instance.CanGetAdrenaline.Remove(player);
        }
        #endregion
    }
}