using Unity.Netcode;
using UnityEngine;

namespace GtaLike.Multiplayer
{
    public class NetworkSessionController : MonoBehaviour
    {
        public void StartHost()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.StartHost();
            }
        }

        public void StartClient()
        {
            if (NetworkManager.Singleton != null)
            {
                NetworkManager.Singleton.StartClient();
            }
        }
    }
}
