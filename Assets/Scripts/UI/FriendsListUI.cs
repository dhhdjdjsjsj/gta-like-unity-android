using System.Net;
using TMPro;
using UnityEngine;

namespace GtaLike.UI
{
    public class FriendsListUI : MonoBehaviour
    {
        [SerializeField] private Multiplayer.LanDiscovery lanDiscovery;
        [SerializeField] private TMP_Text listText;

        private void Update()
        {
            if (lanDiscovery == null || listText == null)
            {
                return;
            }

            listText.text = "";
            foreach (IPEndPoint host in lanDiscovery.DiscoveredHosts)
            {
                listText.text += $"{host.Address}\n";
            }
        }

        public void ConnectFirst()
        {
            if (lanDiscovery == null || lanDiscovery.DiscoveredHosts.Count == 0)
            {
                return;
            }

            lanDiscovery.ConnectToHost(lanDiscovery.DiscoveredHosts[0]);
        }
    }
}
