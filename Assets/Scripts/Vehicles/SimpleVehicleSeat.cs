using UnityEngine;

namespace GtaLike.Vehicles
{
    public class SimpleVehicleSeat : MonoBehaviour
    {
        [SerializeField] private SimpleCarController carController;
        [SerializeField] private Transform seatPoint;

        private GameObject currentDriver;

        public bool TryEnter(GameObject player)
        {
            if (currentDriver != null || carController == null)
            {
                return false;
            }

            currentDriver = player;
            currentDriver.SetActive(false);
            carController.SetActive(true);
            return true;
        }

        public void Exit()
        {
            if (currentDriver == null)
            {
                return;
            }

            currentDriver.SetActive(true);
            currentDriver.transform.position = (seatPoint != null ? seatPoint.position : transform.position) + transform.right * 2f;
            carController.SetActive(false);
            currentDriver = null;
        }
    }
}
