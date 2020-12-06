using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].tcp.SendData(packet);
    }

    private static void SendUDPData(int toClient, Packet packet)
    {
        packet.WriteLength();
        Server.clients[toClient].udp.SendData(packet);
    }

    private static void SendTCPDataToALL(Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++)
        {
            Server.clients[i].tcp.SendData(packet);
        }
    }
    private static void SendTCPDataToALL(int exceptClient, Packet packet)
    {
        packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++)
        {
            if (i != exceptClient)
                Server.clients[i].tcp.SendData(packet);
        }
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.maxPlayers; i++)
        {
            if (i != _exceptClient)
                Server.clients[i].udp.SendData(_packet);
        }
    }

    public static void Welcome(int toClient, string message)
    {
        using (Packet packet = new Packet((int)ServerPackets.welcome))
        {
            packet.Write(message);
            packet.Write(toClient);

            SendTCPData(toClient, packet);

        }
    }

    public static void SpawnPlayer(int toClient, Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.spawnPlayer))
        {
            packet.Write(player.id);
            packet.Write(player.userName);
            packet.Write(player.transform.position);
            packet.Write(player.transform.rotation);

            SendTCPData(toClient, packet);

        }
    }

    public static void PlayerPosition(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerPosition))
        {
            packet.Write(player.id);
            packet.Write(player.transform.position);
            SendUDPDataToAll(packet);
        }
    }

    public static void PlayerRotation(Player player)
    {
        using (Packet packet = new Packet((int)ServerPackets.playerPosition))
        {
            packet.Write(player.id);
            packet.Write(player.transform.rotation);
            SendUDPDataToAll(player.id, packet);
        }
    }
}
