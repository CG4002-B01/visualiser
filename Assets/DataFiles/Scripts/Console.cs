using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using System.Text;
using System.Net.Sockets;
using Renci.SshNet;
using System.Threading;

public class Console : MonoBehaviour {

    string stuHost = "stu.comp.nus.edu.sg";
    string stuUser = "xuanlc13";
    string stuPass = "inEAdtoX618rBgfr15qE";
    string ultra96Host = "192.168.95.224";
    string socketHost;
    int socketPort;
    Socket socket;
    ForwardedPortLocal port;
    SshClient stuClient;
    Thread recvThread;

    IEnumerator coroutine;
    public TMP_Text consoleText;

    public void connect() {
        // Tunnel
        stuClient = new SshClient(stuHost, stuUser, stuPass);
        stuClient.Connect();

        port = new ForwardedPortLocal("127.0.0.1", ultra96Host, 5001);
        stuClient.AddForwardedPort(port);
        port.Start();
        Debug.Log(port.BoundPort);

        // Socket
        int socketPort = Convert.ToInt32(port.BoundPort);
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        
        socket.Connect(port.BoundHost, socketPort);

        // Authorize
        byte[] message = Encoding.ASCII.GetBytes("1"); //player.ToString()
        socket.Send(message);

        byte[] data = new byte[1024];
        socket.Receive(data);
        string response = Encoding.UTF8.GetString(data);

        consoleText.text = response;
        Debug.Log("connected");

        coroutine = recvLoop();
        StartCoroutine(coroutine);
    }

    IEnumerator recvLoop() {
        while (true) {
            string response = receiveMsg();
            Debug.Log("received: " + response);
            consoleText.text = response;

            handleMsg(response);
            yield return null;
        }
    }
    
    public string receiveMsg() {
        try {
            var packet = "";
            var buffer = new byte[1];
            while (true) {
                var nextLength = socket.Receive(buffer,0, 1, SocketFlags.None);
                var curChar = Convert.ToChar(buffer[0]);
                if (curChar == '_') {
                    break;
                }
                if(nextLength == 0) {
                    packet = "";
                    break;
                }
                packet += curChar;
            }
            var packetSize = int.Parse(packet);
            if (packetSize == 0) {
                disconnect();
                return "";
            }

            int byteRecv = 0;
            var data = new byte[1024];
            while (byteRecv < packetSize) {
                int bytes = socket.Receive(data, byteRecv, packetSize - byteRecv, SocketFlags.None);
                byteRecv += bytes;
                if (bytes == 0) {
                    disconnect();
                    return "";
                }
            }

            string response = Encoding.UTF8.GetString(data, 0, byteRecv);
            if (response.Length == 0) {
                disconnect();
                return "";
            }

            return response;
        }
        catch (SocketException e) {
            Debug.Log(e);
            disconnect();
            return "";
        }
    }

    public void handleMsg(string msg) {
        Debug.Log(msg);
        if (msg.Contains("\"action\": \"throw\", \"player\":")) {
            Debug.Log("grenade");
            //check if have opp in view
            var response = "{\"action\": \"grenade\"}"; // standard response
            sendMsg(response);
        }
    }
    
    public void sendMsg(string msg) {
        var msgLength = msg.Length;
        msg = msgLength + "_" + msg;
        try{
            byte[] msgBytes = Encoding.ASCII.GetBytes(msg);
            socket.Send(msgBytes);
        }   catch (Exception e) {
            Debug.Log(e);
            disconnect();
        }
    }

    public void disconnect() {
        // recvThread.Abort();
        StopCoroutine(coroutine);
        socket.Close();
        port.Stop();
        stuClient.Disconnect();
        Debug.Log("disconnected");
    }

    public void close() {
        Debug.Log("exit");
        consoleText.text = "exit";
        Application.Quit();
    }
}
