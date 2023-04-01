using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.Text;
using System.Net;
using System.Net.Sockets;
using Renci.SshNet;
using System.Threading;

public class Console : MonoBehaviour
{

    string stuHost = "stu.comp.nus.edu.sg";
    string stuUser = "xuanlc13";
    string stuPass = "inEAdtoX618rBgfr15qE";
    // string ultra96Host = "192.168.95.224";
    // Backup Board
    string ultra96Host = "192.168.95.245";
    string socketHost;
    int socketPort;
    Socket socket;
    ForwardedPortLocal port;
    SshClient stuClient;
    Thread recvThread;
    Thread sendThread;

    public JSONReader jsonReader;
    public TMP_Text consoleText;
    string textToUpdate;

    int player = GlobalStates.GetPlayerNo();
    // int player = 1; // TODO: set on connect

    int enemyPlayer;

    string recvState = "";
    bool newState = false;

    // Opponent in view and grenade can hit
    // 0 = no throw command 
    // 1 = hit
    // 2 = miss
    int grenadeHitStatus;

    void Start()
    {
        grenadeHitStatus = 0;
        enemyPlayer = (player == 1) ? 2 : 1;
    }

    void Update()
    {
        consoleText.text = textToUpdate;
    }

    public string getGameState()
    { // can switch to return gamestate class
        return recvState;
    }

    public bool hasNewGameState()
    {
        return newState;
    }

    public void setReadGameState()
    {
        newState = false;
    }

    // public bool hasGrenadeCheck()
    // {
    //     return grenadeCheck;
    // }

    public void setGrenadeCheck(bool status)
    {
        Debug.Log("Set grenade Check function called");
        // grenadeCheck = status;
        // Debug.Log(grenadeCheck);
    }

    public void setGrenadeHit(int status)
    {
        grenadeHitStatus = status;
    }

    public void connect()
    {
        // Tunnel to Ultra96
        stuClient = new SshClient(stuHost, stuUser, stuPass);
        stuClient.Connect();

        port = new ForwardedPortLocal("127.0.0.1", ultra96Host, 5004);
        stuClient.AddForwardedPort(port);
        port.Start();
        Debug.Log(port.BoundPort);

        // For testing
        // IPAddress[] IPs = Dns.GetHostAddresses("localhost");
        // string localhostName = "127.0.0.1"; //For testing on Jon's laptop
        // System.Int32 localhostPortNo = 5004;

        int socketPort = Convert.ToInt32(port.BoundPort);
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        // try
        // {
        //     socket.Connect(localhostName, localhostPortNo); // Testing by connecting to local host
        // }
        // catch (Exception e)
        // {
        //     Debug.Log(e);
        // }
        socket.Connect(port.BoundHost, socketPort); // For connecting to Ultra96

        try
        {
            // sendThread = new Thread(new ThreadStart(startSend));
            // sendThread.IsBackground = true;
            // sendThread.Start();

            recvThread = new Thread(new ThreadStart(startRecv));
            recvThread.IsBackground = true;
            recvThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    public void startRecv()
    {
        // Authorize
        byte[] message = Encoding.ASCII.GetBytes(player.ToString());
        socket.Send(message);

        byte[] data = new byte[1024];
        socket.Receive(data);
        string response = Encoding.UTF8.GetString(data);
        Debug.Log(response);

        textToUpdate = response;

        Debug.Log("connected");

        recvLoop();
    }

    public void recvLoop()
    {
        while (true)
        {
            try
            {
                string response = receiveMsg();
                Debug.Log("received: " + response);
                jsonReader.setTextJSON(response);
                Thread.Sleep(500);
                if (grenadeHitStatus == 1)
                {
                    Debug.Log("Grenade Hit message sent");
                    // textToUpdate = "Grenade Hit Message sent";
                    var posResponse = "{\"action\": \"grenade_hit\", \"player\": " + enemyPlayer + "}";
                    sendMsg(posResponse);
                    grenadeHitStatus = 0;
                }
                else if (grenadeHitStatus == 2)
                {
                    Debug.Log("Grenade Miss message sent");
                    // textToUpdate = "Grenade Hit Message sent";
                    var negResponse = "{\"action\": \"grenade_miss\", \"player\": " + enemyPlayer + "}";
                    sendMsg(negResponse);
                    grenadeHitStatus = 0;
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
    }

    public string receiveMsg()
    {
        try
        {
            var packet = "";
            var buffer = new byte[1];
            while (true)
            {
                var nextLength = socket.Receive(buffer, 0, 1, SocketFlags.None);
                var curChar = Convert.ToChar(buffer[0]);
                if (curChar == '_')
                {
                    break;
                }
                if (nextLength == 0)
                {
                    packet = "";
                    break;
                }
                packet += curChar;
            }
            var packetSize = int.Parse(packet);
            if (packetSize == 0)
            {
                disconnect();
                return "";
            }

            var data = new byte[1024];
            int byteRecv = socket.Receive(data, packetSize, SocketFlags.None);
            string response = Encoding.UTF8.GetString(data, 0, byteRecv);
            if (response.Length == 0)
            {
                disconnect();
                return "";
            }

            return response;
        }
        catch (SocketException e)
        {
            Debug.Log(e);
            disconnect();
            return "";
        }
    }

    public void handleMsg(string msg)
    {
        Debug.Log(msg);

        if (msg.Contains("throwcheck"))
        { // msg format: throwcheck:1
            string throwPlayer = msg.Split(':')[1];
            if (throwPlayer == player.ToString())
            {
                // grenadeCheck = true;
                // grenadeHit = true; // testing only
            }
        }
        else
        {
            recvState = msg;
            newState = true;
        }
    }

    public void startSend()
    {
        while (true)
        {
            textToUpdate = "thread running";
            if (grenadeHitStatus == 1)
            {
                Debug.Log("Grenade Hit message sent");
                // textToUpdate = "Grenade Hit Message sent";
                var posResponse = "{\"action\": \"grenade_hit\", \"player\": " + enemyPlayer + "}";
                sendMsg(posResponse);
                grenadeHitStatus = 0;
            }
            else if (grenadeHitStatus == 2)
            {
                Debug.Log("Grenade Miss message sent");
                // textToUpdate = "Grenade Hit Message sent";
                var negResponse = "{\"action\": \"grenade_miss\", \"player\": " + enemyPlayer + "}";
                sendMsg(negResponse);
                grenadeHitStatus = 0;
            }
            // grenadeCheck = false;
        }
    }

    public void sendMsg(string msg)
    {
        var msgLength = msg.Length;
        msg = msgLength + "_" + msg;
        Debug.Log("sent: " + msg);
        try
        {
            byte[] msgBytes = Encoding.ASCII.GetBytes(msg);
            socket.Send(msgBytes);
        }
        catch (Exception e)
        {
            Debug.Log(e);
            disconnect();
        }
    }

    public void disconnect()
    {
        recvThread.Abort();
        sendThread.Abort();
        socket.Close();
        port.Stop();
        stuClient.Disconnect();
        Debug.Log("disconnected");
    }

    public void close()
    {
        disconnect();
        Debug.Log("exit");
        consoleText.text = "exit";
        Application.Quit();
    }
}