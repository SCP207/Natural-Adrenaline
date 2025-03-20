using Exiled.API.Enums;
using Exiled.API.Interfaces;
using NaturalAdrenaline.Enums;
using System.ComponentModel;

namespace NaturalAdrenaline {
    public class Config : IConfig {
        [Description("Is this plugin enabled?")]
        public bool IsEnabled { get; set; } = true;

        [Description("Is debug mode enabled?")]
        public bool Debug { get; set; } = false;

        [Description("Allows SCPs to get adrenaline (or not if false)")]
        public bool AllowScps { get; set; } = false;

        [Description("Contains all damage sources that don't give adrenaline")]
        public DamageType[] DamageType { get; set; } = { };

        [Description("Math\n\tScaling Type (Linear, Exponential, Logrithmic)")]
        public ScalingType ScalingType { get; set; } = ScalingType.Logrithmic;

        [Description("\tThe rate of change (base/coefficient) for AHP")]
        public float AhpRateOfChange { get; set; } = 1.06617606f;
        [Description("\tThe rate of change (base/coefficient) for Movement Boost")]
        public float MbRateOfChange { get; set; } = 1.26015997f;

        [Description("\tThe amount the damage gets divided by")]
        public float Divisor { get; set; } = 7;

        [Description("AHP\n\tThe max amount of AHP")]
        public int MaxAhp { get; set; } = 45;

        [Description("Movement Boost\n\tThe max intensity for movement boost")]
        public byte MaxIntensity { get; set; } = 15;

        [Description("\tThe duration of the movement boost")]
        public float EffectDuration { get; set; } = 4;

        [Description("The amount of time needed to wait before a player can get more adrenaline")]
        public float Duration { get; set; } = 10;
    }
}