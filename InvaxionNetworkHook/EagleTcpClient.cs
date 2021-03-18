using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace InvaxionNetworkHook
{
    class EagleTcpClient
    {
        private class GamePackage
        {
            public uint MainCmd { get; set; }
            public uint ParaCmd { get; set; }
            public short DataLen { get; set; }
            public byte[] Data { get; set; }
        }

        private enum EagleStatus
        {
            ES_OK,
            ES_DescriptorPoolNull,
            ES_MsgNoNotFind,
            ES_DescriptorNull,
            ES_SerializeFailed,
            ES_CompressFailed,
            ES_SendFailed,
            ES_PrototypeMessageNull,
            ES_ParseDescFailed,
            ES_AddDescFailed,
            ES_CreateImporterFailed,
            ES_AlreadyCreateImporter,
            ES_UnSerializeFailed,
            ES_Disconnect
        }

        private readonly IPAddress ip;
        private readonly int port;
        private Socket client;
        private Thread recvThread;
        private byte[] recvBuf;
        private MemoryStream recvBufStream;
        private BinaryReader recvReader;
        private Queue<GamePackage> recvQueue;

        private bool Connected
        {
            get
            {
                return client != null && client.Connected;
            }
        }

        private EagleTcpClient(string ip, int port)
        {
            recvBuf = new byte[EagleTcp.MAX_TRUNKSIZE];
            recvBufStream = new MemoryStream(recvBuf);
            recvReader = new BinaryReader(recvBufStream);
            recvQueue = new Queue<GamePackage>();

            this.ip = IPAddress.Parse(ip);
            this.port = port;
        }

        private bool Connect()
        {
            try
            {
                // 创建连接
                IPEndPoint point = new IPEndPoint(ip, port);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Blocking = true;
                client.Connect(point);
                // 接收线程
                recvThread = new Thread(new ThreadStart(RecvThread));
                recvThread.Start();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Disconnect()
        {
            // 终止接收线程
            if (recvThread != null)
            {
                recvThread.Abort();
                recvThread = null;
            }
            // 关闭连接
            if (client != null)
            {
                if(client.Connected)
                    client.Shutdown(SocketShutdown.Both);
                client.Close();
                client = null;
            }
        }

        private int Send(uint mainCmd, uint paraCmd, byte[] msgContent)
        {
            if (client == null || !client.Connected)
                return (int)EagleStatus.ES_Disconnect;

            var buff = new MemoryStream();
            var writer = new BinaryWriter(buff);

            var preloadLen = 4 + 1 + 1 + 2;
            var dataLen = msgContent.Length;
            var pkgLen = dataLen + preloadLen;

            // pkgLen
            writer.Write(pkgLen - 4);
            // mainCmd
            writer.Write((byte)mainCmd);
            // paraCmd
            writer.Write((byte)paraCmd);
            // dataLen
            writer.Write((short)dataLen);
            // msgContent
            writer.Write(msgContent);

            client.Send(buff.GetBuffer(), (int)buff.Position, SocketFlags.None);

            return (int)EagleStatus.ES_OK;
        }

        private int Recv(ref int mainCmd, ref int paraCmd, byte[] msgContent)
        {
            if (client == null || !client.Connected)
                return -1;
            if (recvQueue.Count > 0)
            {
                var pkg = recvQueue.Dequeue();
                mainCmd = (int)pkg.MainCmd;
                paraCmd = (int)pkg.ParaCmd;
                pkg.Data.CopyTo(msgContent, 0);

                MyLogger.Log(string.Format("收到封包: MainCmd {0} ParaCmd {1} DataLen {2} RealLen {3}", pkg.MainCmd, pkg.ParaCmd, pkg.DataLen, pkg.Data.Length));

                return pkg.DataLen;
            }
            return 0;
        }

        private void RecvThread()
        {
            try
            {
                while (true)
                {
                    if (client == null || !client.Connected)
                        break;

                    client.Receive(recvBuf, 4, SocketFlags.None);
                    recvReader.BaseStream.Seek(0, SeekOrigin.Begin);

                    var pkgLen = recvReader.ReadInt32();

                    client.Receive(recvBuf, pkgLen, SocketFlags.None);
                    recvReader.BaseStream.Seek(0, SeekOrigin.Begin);

                    var pkg = new GamePackage
                    {
                        MainCmd = recvReader.ReadByte(),
                        ParaCmd = recvReader.ReadByte(),
                        DataLen = recvReader.ReadInt16()
                    };
                    pkg.Data = recvReader.ReadBytes(pkg.DataLen);

                    recvQueue.Enqueue(pkg);
                }
            }
            catch (ThreadAbortException)
            {

            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private static Dictionary<int, EagleTcpClient> clients = new Dictionary<int, EagleTcpClient>();

        public static bool ContectServer(string pszServerIP, int nServerPort, int tag)
        {
            MyLogger.Log(tag + " ContectServer");

            EagleTcpClient client;

            if (clients.ContainsKey(tag))
            {
                client = clients[tag];
            }
            else
            {
                client = new EagleTcpClient(pszServerIP, nServerPort);
                clients.Add(tag, client);
            }

            return client.Connect();
        }

        public static bool IsConnected(int tag)
        {
            if (!clients.ContainsKey(tag))
                return false;
            return clients[tag].Connected;
        }

        public static void DisconnectServer(int tag)
        {
            MyLogger.Log(tag + " DisconnectServer");

            if (clients.ContainsKey(tag))
                clients[tag].Disconnect();
        }

        public static int SendCmd(int tag, uint mainCmd, uint paraCmd, byte[] msgContent, int size)
        {
            MyLogger.Log(tag + " SendCmd");

            try
            {
                if (!clients.ContainsKey(tag))
                    return (int)EagleStatus.ES_Disconnect;
                return clients[tag].Send(mainCmd, paraCmd, msgContent);
            }
            catch (Exception e)
            {
                MyLogger.Error(e.Message + "\n" + e.StackTrace);
            }
            return (int)EagleStatus.ES_Disconnect;
        }

        public static int ParseCmd(int tag, ref int mainCmd, ref int paraCmd, byte[] msgContent)
        {
            if (!clients.ContainsKey(tag))
                return -1;
            return clients[tag].Recv(ref mainCmd, ref paraCmd, msgContent);
        }
    }
}
