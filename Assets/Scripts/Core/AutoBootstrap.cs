using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace GtaLike.Core
{
    public static class AutoBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            if (Object.FindObjectOfType<GameBootstrap>() != null)
            {
                return;
            }

            var root = new GameObject("GameBootstrap");
            var networkManager = Object.FindObjectOfType<NetworkManager>();
            if (networkManager == null)
            {
                networkManager = root.AddComponent<NetworkManager>();
                root.AddComponent<UnityTransport>();
            }

            var cityGenerator = root.AddComponent<City.CityGenerator>();
            var playerSpawner = root.AddComponent<Player.PlayerSpawner>();
            var discovery = root.AddComponent<Multiplayer.LanDiscovery>();

            var bootstrap = root.AddComponent<GameBootstrap>();
            bootstrap.GetType();
            root.hideFlags = HideFlags.DontSave;

            var bootstrapField = typeof(GameBootstrap).GetField("cityGenerator", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            bootstrapField?.SetValue(bootstrap, cityGenerator);
            var spawnerField = typeof(GameBootstrap).GetField("playerSpawner", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            spawnerField?.SetValue(bootstrap, playerSpawner);
            var discoveryField = typeof(GameBootstrap).GetField("lanDiscovery", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            discoveryField?.SetValue(bootstrap, discovery);
        }
    }
}
