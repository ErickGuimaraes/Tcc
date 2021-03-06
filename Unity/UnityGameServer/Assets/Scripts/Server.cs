﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server 
{
    public static int maxPlayers { get; private set; }
    public static int port { get; private set; }
    public static Dictionary<int, Client> clients = new Dictionary<int, Client>();
    public delegate void PacketHandler(int fromClient, Packet packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static void Start(int _maxPlayers, int _port)
    {
        maxPlayers = _maxPlayers;
        port = _port;

        Debug.Log("Server starting...");
        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectionCallback), null);

        udpListener = new UdpClient(port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Debug.Log($"Server running on {port}");
    }

    private static void TCPConnectionCallback(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectionCallback), null);

        Debug.Log($"Incoming conection from {_client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= maxPlayers; i++)
        {
            if (clients[i].tcp.socket == null)
            {
                Debug.Log("teste ok " + i);
                clients[i].tcp.Connect(_client);
                return;
            }
        }

        //There is no more avaliable slots to new clients connections...
        Debug.Log($"{_client.Client.RemoteEndPoint} failed to connect: Server is full");
    }

    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] data = udpListener.EndReceive(_result, ref _clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (data.Length < 4)
            {
                return;
            }

            using (Packet _packet = new Packet(data))
            {
                int clientId = _packet.ReadInt();

                if (clientId == 0)
                    return;

                if (clients[clientId].udp.endPoint == null)
                {
                    clients[clientId].udp.Connect(_clientEndPoint);
                    return;
                }

                if (clients[clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                    clients[clientId].udp.HandleData(_packet);
            }
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error receiving UDP data: {_ex}");
        }
    }

    public static void SendUDPData(IPEndPoint clientEndPoint, Packet packet)
    {
        try
        {
            if (clientEndPoint != null)
                udpListener.BeginSend(packet.ToArray(), packet.Length(), clientEndPoint, null, null);
        }
        catch (Exception _ex)
        {
            Debug.Log($"Error sending data to {clientEndPoint} via UDP: {_ex}");
        }
    }

    private static void InitializeServerData()
    {
        Debug.Log(maxPlayers + " MAX PLAYER");
        for (int i = 1; i <= maxPlayers; i++)
        {
            clients.Add(i, new Client(i));
        }
        
        int val1 = (int)ClientPackets.welcomeReceived;
        packetHandlers = new Dictionary<int, PacketHandler>();
        Debug.Log("HDHDHA + " + val1);
        Debug.Log("HDHDHA HAUHAUHAHU antes");
        PacketHandler pckt1 = ServerHandle.WelcomeReceived;
        Debug.Log("HDHDHA HAUHAUHAHU " + pckt1);
        packetHandlers.Add(val1, pckt1);
        Debug.Log("HDHDHA HAUHAUHAHU 4434");
        val1 = (int)ClientPackets.playerMovement;
        Debug.Log("HDHDHA HAUHAUHAHU 904095");
        pckt1 = ServerHandle.PlayerMovement;
        Debug.Log("HDHDHA HAUHAUHAHU 90409555541");
        packetHandlers.Add(val1, pckt1);

/*            {
                {(int)ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
                {(int)ClientPackets.playerMovement, ServerHandle.PlayerMovement }
            };*/
        Debug.Log("Initialized packets");
    }
}
