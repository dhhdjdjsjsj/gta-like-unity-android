using UnityEngine;

namespace GtaLike.Weapons
{
    [CreateAssetMenu(menuName = "GTA-Like/Weapons/Weapon Definition")]
    public class WeaponDefinition : ScriptableObject
    {
        public string weaponName = "Weapon";
        public float fireRate = 8f;
        public float damage = 15f;
        public float range = 120f;
        public GameObject muzzleFlashPrefab;
    }
}
