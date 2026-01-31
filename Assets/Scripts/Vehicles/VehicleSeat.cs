using Unity.Netcode;
using UnityEngine;

namespace GtaLike.Vehicles
{
    public class VehicleSeat : NetworkBehaviour
    {
        [SerializeField] private CarController carController;
        [SerializeField] private Transform seatPoint;

        private NetworkObject currentDriver;

        public bool TryEnter(NetworkObject player)
        {
            if (currentDriver != null)
            {
                return false;
            }

            currentDriver = player;
            player.transform.position = seatPoint != null ? seatPoint.position : transform.position;
            player.gameObject.SetActive(false);
            carController.NetworkObject.ChangeOwnership(player.OwnerClientId);
            return true;
        }

        public void Exit()
        {
            if (currentDriver == null)
            {
                return;
            }

            currentDriver.gameObject.SetActive(true);
            currentDriver.transform.position = transform.position + transform.right * 2f;
            currentDriver = null;
        }
    }
}
