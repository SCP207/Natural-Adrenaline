using Exiled.API.Enums;
using Exiled.API.Features;
using NaturalAdrenaline.Handlers;
using System;
using System.Collections.Generic;

namespace NaturalAdrenaline {
    public class Plugin : Plugin<Config> {
        public override string Author { get; } = "SCP-207";
        public override string Name { get; } = "Natural Adrenaline";
        public override string Prefix { get; } = "NA";
        public override PluginPriority Priority { get; } = PluginPriority.Default;
        public override Version RequiredExiledVersion { get; } = new(9, 5, 1);
        public override Version Version { get; } = new(1, 0, 1);

        public static Plugin Instance { get; private set; }

        public List<Player> CanGetAdrenaline { get; } = new();

        public override void OnEnabled() {
            Instance = this;
            RegisterCommandsAndEvents();

            base.OnEnabled();
        }

        public override void OnDisabled() {
            Instance = null;
            UnregisterCommandsAndEvents();

            base.OnDisabled();
        }

        private void RegisterCommandsAndEvents() => EventHandlers.Register();

        private void UnregisterCommandsAndEvents() => EventHandlers.Unregister();
    }
}