using Unity.Netcode;
using UnityEngine;
using GtaLike.Vehicles;

namespace GtaLike.Player
{
    public class VehicleInteractor : NetworkBehaviour
    {
        [SerializeField] private float interactRange = 2f;
        [SerializeField] private LayerMask vehicleLayer;

        private VehicleSeat currentSeat;

        private void Update()
        {
            if (!IsOwner)
            {
                return;
            }

            FindSeat();

            if (MobileInputSource.ActionPressed)
            {
                TryToggleVehicle();
            }
        }

        private void FindSeat()
        {
            var hits = Physics.OverlapSphere(transform.position, interactRange, vehicleLayer);
            currentSeat = hits.Length > 0 ? hits[0].GetComponentInParent<VehicleSeat>() : null;
        }

        private void TryToggleVehicle()
        {
            if (currentSeat == null)
            {
                return;
            }

            if (currentSeat.TryEnter(GetComponent<NetworkObject>()))
            {
                return;
            }

            currentSeat.Exit();
        }
    }
}
