using Unity.Netcode;
using UnityEngine;

namespace GtaLike.Weapons
{
    public class WeaponController : NetworkBehaviour
    {
        [SerializeField] private WeaponDefinition[] weapons;
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask hitMask;

        private int currentIndex;
        private float nextFireTime;

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) SelectWeapon(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SelectWeapon(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SelectWeapon(2);

            if (Player.MobileInputSource.FirePressed || Input.GetMouseButton(0))
            {
                TryFire();
            }
        }

        public void SelectWeapon(int index)
        {
            if (weapons == null || index < 0 || index >= weapons.Length)
            {
                return;
            }

            currentIndex = index;
        }

        private void TryFire()
        {
            var weapon = GetCurrentWeapon();
            if (weapon == null || Time.time < nextFireTime)
            {
                return;
            }

            nextFireTime = Time.time + 1f / weapon.fireRate;

            FireServerRpc(firePoint.position, firePoint.forward, currentIndex);
        }

        [ServerRpc]
        private void FireServerRpc(Vector3 origin, Vector3 direction, int weaponIndex)
        {
            var weapon = weapons[weaponIndex];
            if (Physics.Raycast(origin, direction, out var hit, weapon.range, hitMask))
            {
                var damageable = hit.collider.GetComponentInParent<Damageable>();
                if (damageable != null)
                {
                    damageable.TakeDamageServerRpc(weapon.damage);
                }
            }

            FireClientRpc(origin, direction, weaponIndex);
        }

        [ClientRpc]
        private void FireClientRpc(Vector3 origin, Vector3 direction, int weaponIndex)
        {
            var weapon = weapons[weaponIndex];
            if (weapon.muzzleFlashPrefab != null && firePoint != null)
            {
                Instantiate(weapon.muzzleFlashPrefab, firePoint.position, firePoint.rotation);
            }
        }

        private WeaponDefinition GetCurrentWeapon()
        {
            if (weapons == null || weapons.Length == 0)
            {
                return null;
            }

            return weapons[Mathf.Clamp(currentIndex, 0, weapons.Length - 1)];
        }
    }
}
