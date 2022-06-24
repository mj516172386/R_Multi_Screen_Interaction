using UnityEngine;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine.UI;
using Net;
using System.Linq;
using System;

public class BroadCast : MonoBehaviour
{ 
    private string strInfo;
    private int port = 7788;
    const string specialText = "123!!@@##$$%%-MessageFormServerBroadCast";
    bool serverIsRun = false;
    Thread serverThread = null;
    UdpClient UdpSend = null;
    public SocketServer _Server;
    void StartServer()//服务器一直发消息
    {
        strInfo = "Starting Server...";
        if (serverThread != null && serverThread.IsAlive) return;
        serverThread = new Thread(() =>
        {
            UdpSend = new UdpClient();
            while (serverIsRun)
            {
                Thread.Sleep(500);
                byte[] buf = Encoding.Unicode.GetBytes(specialText);
                UdpSend.Send(buf, buf.Length, new IPEndPoint(IPAddress.Broadcast, port));
            }
            UdpSend.Close();
        });
        serverThread.IsBackground = true;
        InitServer();
        serverThread.Start();
        strInfo = "Server Started";
        print(strInfo);
    }
    void Start()
    {
        StartServer();
      ///  CourseMain.changeTipsText = ChangeTips;
    }
    void ExitGame()
    {
        StopServer();
        Application.Quit();
    }
    private void ChangeTips(string msg)
    {
        print("MJ:服务端发送消息" +msg);
    }


    void InitServer()
    {
        serverIsRun = true;
        StartDataListen();
        if (UdpSend != null) UdpSend.Close();
    }
    public void SendMsg(bool value) 
    {
        print(value);
        foreach (var item in _Server.GetConnectionList())
        {
            if (value)
                item.Send("000000000000");
            else
                item.Send("111111111111");
        }
    }
    void StartDataListen()
    {
        //创建服务器对象，默认监听本机0.0.0.0，端口12345
        IPAddress iip = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault(a => a.AddressFamily.ToString().Equals("InterNetwork"));
        Debug.Log(iip.ToString());
        _Server = new SocketServer(iip.ToString(), 7788);

        //处理从客户端收到的消息
        _Server.HandleRecMsg = new Action<byte[], SocketConnection, SocketServer>((bytes, client, theServer) =>
        {
            string msg = Encoding.UTF8.GetString(bytes);
            Debug.Log($"收到消息:{msg}");
        });

        //处理服务器启动后事件
        _Server.HandleServerStarted = new Action<SocketServer>(theServer =>
        {
            Debug.Log("服务已启动************");
        });

        //处理新的客户端连接后的事件
        _Server.HandleNewClientConnected = new Action<SocketServer, SocketConnection>((theServer, theCon) =>
        {
            Debug.Log($@"一个新的客户端接入，当前连接数：{theServer.GetConnectionCount()}");
        });

        //处理客户端连接关闭后的事件
        _Server.HandleClientClose = new Action<SocketConnection, SocketServer>((theCon, theServer) =>
        {
            Debug.Log($@"一个客户端关闭，当前连接数为：{theServer.GetConnectionCount()}");
        });

        //处理异常
        _Server.HandleException = new Action<Exception>(ex =>
        {
            Debug.Log(ex.Message);
        });

        //服务器启动
        _Server.StartServer();
    }
    void StopServer()
    {
        strInfo = "Stoping Server...";
        serverIsRun = false;
        if (UdpSend != null) UdpSend.Close();
        if (serverThread != null && serverThread.IsAlive) serverThread.Abort();
        strInfo = "Server Stoped";
        print("MJ Severs msg to =" + strInfo);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
            Application.Quit();
        }
    }

    private void OnDisable()
    {
        ExitGame();
    } 
}