using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

namespace GtaLike.Multiplayer
{
    public class LanDiscovery : MonoBehaviour
    {
        [SerializeField] private int discoveryPort = 47777;
        [SerializeField] private float broadcastInterval = 1f;
        [SerializeField] private string gameId = "GtaLike";

        private UdpClient udpClient;
        private float nextBroadcastTime;
        private readonly List<IPEndPoint> discoveredHosts = new();

        public IReadOnlyList<IPEndPoint> DiscoveredHosts => discoveredHosts;

        public void StartDiscovery()
        {
            udpClient = new UdpClient(discoveryPort) { EnableBroadcast = true };
            udpClient.BeginReceive(OnReceive, null);
        }

        private void Update()
        {
            if (udpClient == null)
            {
                return;
            }

            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsHost && Time.time >= nextBroadcastTime)
            {
                BroadcastHost();
                nextBroadcastTime = Time.time + broadcastInterval;
            }
        }

        private void BroadcastHost()
        {
            var payload = Encoding.UTF8.GetBytes(gameId);
            udpClient.Send(payload, payload.Length, new IPEndPoint(IPAddress.Broadcast, discoveryPort));
        }

        private void OnReceive(IAsyncResult result)
        {
            if (udpClient == null)
            {
                return;
            }

            var endpoint = new IPEndPoint(IPAddress.Any, discoveryPort);
            var data = udpClient.EndReceive(result, ref endpoint);
            var message = Encoding.UTF8.GetString(data);

            if (message == gameId && !discoveredHosts.Contains(endpoint))
            {
                discoveredHosts.Add(endpoint);
            }

            udpClient.BeginReceive(OnReceive, null);
        }

        public void ConnectToHost(IPEndPoint host)
        {
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            if (transport != null)
            {
                transport.SetConnectionData(host.Address.ToString(), transport.ConnectionData.Port);
                NetworkManager.Singleton.StartClient();
            }
        }

        private void OnDestroy()
        {
            udpClient?.Close();
        }
    }
}
