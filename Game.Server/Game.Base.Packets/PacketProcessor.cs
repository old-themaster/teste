// Decompiled with JetBrains decompiler
// Type: Game.Base.Packets.PacketProcessor
// Assembly: Game.Server, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 686A9271-7028-4253-9759-D87B403FFD71
// Assembly location: C:\WONDERTANK vReZero\Emulator\Road\Game.Server.dll

using Game.Base.Events;
using Game.Server;
using Game.Server.Packets.Client;
using log4net;
using System;
using System.Reflection;
using System.Threading;

namespace Game.Base.Packets
{
    public class PacketProcessor
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        protected IPacketHandler m_activePacketHandler;
        protected GameClient m_client;
        protected int m_handlerThreadID;
        protected static readonly IPacketHandler[] m_packetHandlers = new IPacketHandler[512];

        public PacketProcessor(GameClient client)
        {
            this.m_client = client;
        }


        public void HandlePacket(GSPacketIn packet)
        {
            int code = (int)packet.Code;
            Statistics.BytesIn += (long)packet.Length;
            ++Statistics.PacketsIn;
            IPacketHandler packetHandler = (IPacketHandler)null;

            if (m_client.ClientStep != 2 && (this.m_client.ClientStep != 1 || packet.Code != (short)1))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Sistema de Defesa By Kafflol:" + DateTime.Now.ToString() + "CODIG0 DO PACOTE " + code + "IP DE DESTINO " + m_client.Socket.RemoteEndPoint);
                CheckConnection.addbanlist("KiwiGuard Block Filter List ", m_client.Socket.RemoteEndPoint.ToString().Split(':')[0]);
                Console.WriteLine("{Bot Attack : " + DateTime.Now.ToString() + "} Packet code: Packet code: " + code + " IP:  IP: " + m_client.Socket.RemoteEndPoint);
                Console.ResetColor();
                m_client.Disconnect();
            }
            else if (code < PacketProcessor.m_packetHandlers.Length)
            {
                packetHandler = PacketProcessor.m_packetHandlers[code];
                if (packetHandler == null)
                    return;
                try
                {
                    packetHandler.ToString();
                }
                catch
                {
                    Console.WriteLine("______________ERROR______________");
                    Console.WriteLine("___ Received code: " + (object)code + " <" + string.Format("0x{0:x}", (object)code) + "> ____");
                    Console.WriteLine("_________________________________");
                }
            }
            else if (PacketProcessor.log.IsErrorEnabled)
            {
                PacketProcessor.log.ErrorFormat("Received packet code is outside of m_packetHandlers array bounds! " + this.m_client.ToString());
                PacketProcessor.log.Error((object)Marshal.ToHexDump(string.Format("===> <{2}> Packet 0x{0:X2} (0x{1:X2}) length: {3} (ThreadId={4})", (object)code, (object)(code ^ 168), (object)this.m_client.TcpEndpoint, (object)packet.Length, (object)Thread.CurrentThread.ManagedThreadId), packet.Buffer));
            }
            if (packetHandler == null)
                return;
            long tickCount = (long)Environment.TickCount;
            try
            {
                if (this.m_client != null)
                {
                    if (packet != null)
                    {
                        if (this.m_client.TcpEndpoint != "not connected")
                            packetHandler.HandlePacket(this.m_client, packet);
                    }
                }
            }
            catch (Exception ex)
            {
                if (PacketProcessor.log.IsErrorEnabled)
                {
                    string tcpEndpoint = this.m_client.TcpEndpoint;
                    PacketProcessor.log.Error((object)("Error while processing packet (handler=" + packetHandler.GetType().FullName + "  client: " + tcpEndpoint + ")"), ex);
                    PacketProcessor.log.Error((object)Marshal.ToHexDump("Package Buffer:", packet.Buffer, 0, packet.Length));
                }
            }
            long num = (long)Environment.TickCount - tickCount;
            this.m_activePacketHandler = (IPacketHandler)null;
            if (PacketProcessor.log.IsDebugEnabled)
                PacketProcessor.log.Debug((object)("Package process Time:" + (object)num + "ms!"));
            if (num <= 1500L)
                return;
            string tcpEndpoint1 = this.m_client.TcpEndpoint;
            if (!PacketProcessor.log.IsWarnEnabled)
                return;
            PacketProcessor.log.Warn((object)("(" + tcpEndpoint1 + ") Handle packet Thread " + (object)Thread.CurrentThread.ManagedThreadId + " " + (object)packetHandler + " took " + (object)num + "ms!"));
        }

        [ScriptLoadedEvent]
        public static void OnScriptCompiled(RoadEvent ev, object sender, EventArgs args)
        {
            Array.Clear((Array)PacketProcessor.m_packetHandlers, 0, PacketProcessor.m_packetHandlers.Length);
            int num = PacketProcessor.SearchPacketHandlers("v168", Assembly.GetAssembly(typeof(GameServer)));
            if (!PacketProcessor.log.IsInfoEnabled)
                return;
            PacketProcessor.log.Info((object)("PacketProcessor: Loaded " + (object)num + " handlers from GameServer Assembly!"));
        }

        public static void RegisterPacketHandler(int packetCode, IPacketHandler handler) => PacketProcessor.m_packetHandlers[packetCode] = handler;

        protected static int SearchPacketHandlers(string version, Assembly assembly)
        {
            int num = 0;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsClass && type.GetInterface("Game.Server.Packets.Client.IPacketHandler") != (Type)null)
                {
                    PacketHandlerAttribute[] customAttributes = (PacketHandlerAttribute[])type.GetCustomAttributes(typeof(PacketHandlerAttribute), true);
                    if (customAttributes.Length != 0)
                    {
                        ++num;
                        PacketProcessor.RegisterPacketHandler(customAttributes[0].Code, (IPacketHandler)Activator.CreateInstance(type));
                    }
                }
            }
            return num;
        }
    }
}
