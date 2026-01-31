using Unity.Netcode;
using UnityEngine;

namespace GtaLike.Core
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private City.CityGenerator cityGenerator;
        [SerializeField] private Player.PlayerSpawner playerSpawner;
        [SerializeField] private Multiplayer.LanDiscovery lanDiscovery;

        private void Awake()
        {
            if (cityGenerator != null)
            {
                cityGenerator.Generate();
            }
        }

        private void Start()
        {
            if (NetworkManager.Singleton == null)
            {
                return;
            }

            if (!NetworkManager.Singleton.IsListening)
            {
                NetworkManager.Singleton.StartHost();
            }

            if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
            {
                if (playerSpawner != null)
                {
                    playerSpawner.SpawnLocalPlayer();
                }
            }

            if (lanDiscovery != null)
            {
                lanDiscovery.StartDiscovery();
            }
        }
    }
}
