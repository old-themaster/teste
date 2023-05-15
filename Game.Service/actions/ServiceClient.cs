// Decompiled with JetBrains decompiler
// Type: Game.Service.actions.ServiceClient
// Assembly: Road.Service, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 588BDF21-3113-47BA-B24B-354B1415A497
// Assembly location: C:\10.5\Emuladores\Road\Road.Service.exe

using System;
using System.IO;
using System.Net;

namespace Game.Service.actions
{
    internal class ServiceClient
    {
        public string MachineName() => Environment.MachineName;

        public string MachineUserName() => Environment.UserName;

        public int MachineProcess() => Environment.ProcessorCount;

        public string MachineSystem() => Environment.OSVersion.ToString();

        public string MachineAddress()
        {
            string str;
            try
            {
                using (StreamReader streamReader = new StreamReader(WebRequest.Create("http://checkip.dyndns.org").GetResponse().GetResponseStream()))
                    str = streamReader.ReadToEnd().Trim().Split(':')[1].Substring(1).Split('<')[0];
            }
            catch
            {
                str = "Nada encontrado.";
            }
            return str;
        }

        public void openConnect()
        {
            Console.WriteLine("+- Computador " + this.MachineName());
            Console.WriteLine("+- Endereço: " + this.MachineAddress());
            Console.WriteLine("+- Usuário" + this.MachineUserName());
            Console.WriteLine("+- Sistema" + this.MachineSystem());
        }
    }
}
