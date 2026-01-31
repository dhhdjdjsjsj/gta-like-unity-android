using UnityEngine;

namespace GtaLike.UI
{
    public class WeaponUI : MonoBehaviour
    {
        [SerializeField] private Weapons.WeaponController weaponController;

        public void SelectPistol()
        {
            weaponController.SelectWeapon(0);
        }

        public void SelectRifle()
        {
            weaponController.SelectWeapon(1);
        }

        public void SelectShotgun()
        {
            weaponController.SelectWeapon(2);
        }
    }
}
