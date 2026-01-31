using Unity.Netcode;
using UnityEngine;

namespace GtaLike.Player
{
    public class PlayerSpawner : NetworkBehaviour
    {
        [SerializeField] private NetworkObject playerPrefab;
        [SerializeField] private Transform spawnPoint;

        public void SpawnLocalPlayer()
        {
            if (!IsServer)
            {
                return;
            }

            var position = spawnPoint != null ? spawnPoint.position : Vector3.zero;
            var rotation = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;
            var prefab = playerPrefab != null ? playerPrefab : CreateDefaultPlayerPrefab();
            var playerInstance = Instantiate(prefab, position, rotation);
            playerInstance.SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId, true);
        }

        private NetworkObject CreateDefaultPlayerPrefab()
        {
            var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            player.name = "Player";
            player.transform.localScale = new Vector3(1f, 2f, 1f);
            player.AddComponent<CharacterController>();
            var networkObject = player.AddComponent<NetworkObject>();
            var controller = player.AddComponent<ThirdPersonController>();
            player.AddComponent<VehicleInteractor>();
            player.AddComponent<Weapons.WeaponController>();
            player.AddComponent<Weapons.Damageable>();

            var cameraObject = new GameObject("PlayerCamera");
            var camera = cameraObject.AddComponent<Camera>();
            var follow = cameraObject.AddComponent<FollowCamera>();
            follow.SetTarget(player.transform);
            controller.SetCameraPivot(camera.transform);

            return networkObject;
        }
    }
}
