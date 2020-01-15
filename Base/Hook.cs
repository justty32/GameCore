using System;
using System.Collections.Generic;
using System.Text;

// mostly use class Util for place_holder, as for Tinput, Toutput

namespace GameCore.Base
{
    public class HookManager
    {
        public Dictionary<string, IHookServer> Servers = new Dictionary<string, IHookServer>();
        public bool Register(string register_name, IHookServer hook_server)
        {
            if (hook_server == null || register_name == null)
                return true;
            if (Servers.ContainsKey(register_name))
                return true;
            Servers.Add(register_name, hook_server);
            return false;
        }
        public bool UnRegister(string register_name)
        {
            if (register_name == null)
                return true;
            if (!Servers.ContainsKey(register_name))
                return true;
            Servers.Remove(register_name);
            return false;
        }
        public bool HasRegister<Tinput, Toutput>(string register_name)
        {
            if (register_name == null)
                return false;
            if (!Servers.ContainsKey(register_name))
                return false;
            if (Servers[register_name] is HookServer<Tinput, Toutput>)
                return true;
            return false;
        }
        public bool Bind<Tinput, Toutput>(string server_name, HookClient<Tinput, Toutput> client)
        {
            if (server_name == null || client == null)
                return true;
            if (HasRegister<Tinput, Toutput>(server_name))
                return (Servers[server_name] as HookServer<Tinput, Toutput>).Bind(client);
            else
                return true;
        }
        public HookClient<Tinput, Toutput> Bind<Tinput, Toutput>(string server_name, Func<Tinput, Toutput> function)
        {
            if (server_name == null || function == null)
                return null;
            if (HasRegister<Tinput, Toutput>(server_name))
                return (Servers[server_name] as HookServer<Tinput, Toutput>).Bind(function);
            else
                return null;
        }
        public bool UnBind<Tinput, Toutput>(string server_name, HookClient<Tinput, Toutput> client)
        {
            if (server_name == null || client == null)
                return true;
            if (HasRegister<Tinput, Toutput>(server_name))
                return (Servers[server_name] as HookServer<Tinput, Toutput>).UnBind(client.Order);
            else
                return true;
        }
        public bool UnBind(string server_name, int order)
        {
            if (server_name == null)
                return true;
            if (Servers.ContainsKey(server_name))
                return Servers[server_name].UnBind(order);
            else
                return true;
        }
    }
    public interface IHookServer
    {
        bool UnBind(int order);
    }
    public class HookServer<Tinput, Toutput> : IHookServer
    {
        public List<HookClient<Tinput, Toutput>> Clients { get; private set; } = new List<HookClient<Tinput, Toutput>>();
        public string RegisteredName = null;
        public HookServer(string register_name = null)
        {
            Register(register_name);    
        }
        public bool Register(string register_name)
        {
            if (Core.HookManager.Register(register_name, this))
                return true;
            RegisteredName = register_name;
            return false;
        }
        public bool UnRegister() {
            RegisteredName = null;
            return Core.HookManager.UnRegister(RegisteredName); 
        }
        public bool Bind(HookClient<Tinput, Toutput> client)
        {
            if (client == null)
                return true;
            client.Order = Clients.Count;
            Clients.Add(client);
            client.Server = this;
            return false;
        }
        public HookClient<Tinput, Toutput> Bind(Func<Tinput, Toutput> function)
        {
            if (function == null)
                return null;
            var c = new HookClient<Tinput, Toutput>(function);
            c.Order = Clients.Count;
            Clients.Add(c);
            c.Server = this;
            return c;
        }
        public bool UnBind(int client_order)
        {
            if (client_order < 0 || client_order >= Clients.Count)
                return true;
            if (Clients?[client_order] == null)
                return true;
            Clients[client_order].Order = -1;
            Clients[client_order].Server = null;
            Clients.RemoveAt(client_order);
            for (int i = client_order; i < Clients.Count; i++)
                Clients[i].Order--;
            return false;
        }
        public bool UnBind(HookClient<Tinput, Toutput> client)
        {
            if (client == null)
                return true;
            UnBind(client.Order);
            return false;
        }
    }
    public class HookClient<Tinput, Toutput>
    {
        public int Order = -1;
        public Func<Tinput, Toutput> Function = null;
        public HookServer<Tinput, Toutput> Server = null;
        public HookClient(Func<Tinput, Toutput> function)
        {
            Function = function;
        }
        public bool UnBind()
        {
            if (Server == null)
                return true;
            Server.UnBind(Order);
            return false;
        }
    }
}