﻿using Ether.Network.Common;
using Rhisis.Network.ISC.Structures;
using Rhisis.Network;
using Rhisis.Network.Packets;
using System.Collections.Generic;
using System.Linq;

namespace Rhisis.Login.Packets
{
    public static class LoginPacketFactory
    {
        /// <summary>
        /// Sends a login error.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="error"></param>
        public static void SendLoginError(INetUser client, ErrorType error)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.ERROR);
                packet.Write((int)error);

                client.Send(packet);
            }
        }

        /// <summary>
        /// Sends the available server list.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="username"></param>
        /// <param name="clusters"></param>
        public static void SendServerList(INetUser client, string username, IEnumerable<ClusterServerInfo> clusters)
        {
            using (var packet = new FFPacket())
            {
                packet.WriteHeader(PacketType.SRVR_LIST);
                packet.Write(0);
                packet.Write<byte>(1);
                packet.Write(username);
                packet.Write(clusters.Sum(x => x.WorldServers.Count) + clusters.Count());

                foreach (ClusterServerInfo cluster in clusters)
                {
                    packet.Write(-1);
                    packet.Write(cluster.Id);
                    packet.Write(cluster.Name);
                    packet.Write(cluster.Host);
                    packet.Write(0);
                    packet.Write(0);
                    packet.Write(1);
                    packet.Write(0);

                    foreach (WorldServerInfo world in cluster.WorldServers)
                    {
                        packet.Write(cluster.Id);
                        packet.Write(world.Id);
                        packet.Write(world.Name);
                        packet.Write(world.Host);
                        packet.Write(0);
                        packet.Write(0);
                        packet.Write(1);
                        packet.Write(100); // Capacity
                    }
                }

                client.Send(packet);
            }
        }
    }
}
