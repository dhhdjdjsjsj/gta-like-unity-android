using UnityEngine;
using GtaLike.Vehicles;

namespace GtaLike.Player
{
    public class SimpleVehicleInteractor : MonoBehaviour
    {
        [SerializeField] private float interactRange = 2.5f;
        [SerializeField] private LayerMask vehicleLayer;

        private SimpleVehicleSeat currentSeat;

        private void Update()
        {
            FindSeat();

            if (MobileInputSource.FirePressed)
            {
                ToggleVehicle();
            }
        }

        private void FindSeat()
        {
            var hits = Physics.OverlapSphere(transform.position, interactRange, vehicleLayer);
            currentSeat = hits.Length > 0 ? hits[0].GetComponentInParent<SimpleVehicleSeat>() : null;
        }

        private void ToggleVehicle()
        {
            if (currentSeat == null)
            {
                return;
            }

            if (!currentSeat.TryEnter(gameObject))
            {
                currentSeat.Exit();
            }
        }
    }
}
