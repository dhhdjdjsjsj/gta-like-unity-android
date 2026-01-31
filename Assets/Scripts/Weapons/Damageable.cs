using Unity.Netcode;
using UnityEngine;

namespace GtaLike.Weapons
{
    public class Damageable : NetworkBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        private readonly NetworkVariable<float> health = new();

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                health.Value = maxHealth;
            }
        }

        [ServerRpc]
        public void TakeDamageServerRpc(float amount)
        {
            health.Value = Mathf.Max(0f, health.Value - amount);
        }
    }
}
