﻿using BeatSaverUpdater.Migration;
using BeatSaverUpdater.UI;
using IPA;
using IPA.Loader;
using SiraUtil.Zenject;
using IPALogger = IPA.Logging.Logger;

namespace BeatSaverUpdater
{
    [Plugin(RuntimeOptions.DynamicInit), NoEnableDisable]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; } = null!;
        internal static IPALogger Log { get; private set; } = null!;
        internal static PluginMetadata Metadata { get; private set; } = null!;
        internal static bool PlaylistsLibInstalled { get; private set; } = false;

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public Plugin(IPALogger logger, Zenjector zenjector, PluginMetadata metadata)
        {
            Instance = this;
            Plugin.Log = logger;
            Metadata = metadata;
            zenjector.Install(Location.Menu, (Container) =>
            {
                Container.BindInterfacesTo<UpdateButton>().AsSingle();
                Container.Bind<PopupModal>().AsSingle();

                Container.BindInterfacesTo<FavouritesMigrator>().AsSingle();
                PlaylistsLibInstalled = PluginManager.GetPluginFromId("BeatSaberPlaylistsLib") != null;
                if (PlaylistsLibInstalled)
                {
                    Container.BindInterfacesTo<PlaylistMigrator>().AsSingle();
                }
            });
        }
    }
}
