using UnityEngine;

namespace GtaLike.Map
{
    public class MapUIController : MonoBehaviour
    {
        [SerializeField] private GameObject fullMapRoot;

        public void ToggleMap()
        {
            if (fullMapRoot == null)
            {
                return;
            }

            fullMapRoot.SetActive(!fullMapRoot.activeSelf);
        }
    }
}
