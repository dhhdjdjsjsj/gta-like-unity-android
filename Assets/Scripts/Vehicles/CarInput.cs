using Unity.Netcode;
using UnityEngine;

namespace GtaLike.Vehicles
{
    public class CarInput : NetworkBehaviour
    {
        [SerializeField] private CarController carController;

        private void Update()
        {
            if (!IsOwner || carController == null)
            {
                return;
            }

            var axis = Player.MobileInputSource.MoveAxis;
            if (axis.sqrMagnitude < 0.01f)
            {
                axis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }

            var brake = Input.GetKey(KeyCode.Space);
            carController.SetInput(axis.x, axis.y, brake);
        }
    }
}
