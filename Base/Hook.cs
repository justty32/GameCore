using System;
using System.Collections.Generic;

// mostly use class Util for place_holder, as for Tin, Tout

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
        public bool HasRegister<Tin, Tout>(string register_name)
        {
            if (register_name == null)
                return false;
            if (!Servers.ContainsKey(register_name))
                return false;
            if (Servers[register_name] is Hook<Tin, Tout>)
                return true;
            return false;
        }
        public bool Bind<Tin, Tout>(string server_name, HookClient<Tin, Tout> client)
        {
            if (server_name == null || client == null)
                return true;
            if (HasRegister<Tin, Tout>(server_name))
                return (Servers[server_name] as Hook<Tin, Tout>).Bind(client);
            else
                return true;
        }
        public HookClient<Tin, Tout> Bind<Tin, Tout>(string server_name, Func<Tin, Tout> function)
        {
            if (server_name == null || function == null)
                return null;
            if (HasRegister<Tin, Tout>(server_name))
                return (Servers[server_name] as Hook<Tin, Tout>).Bind(function);
            else
                return null;
        }
        public bool UnBind<Tin, Tout>(string server_name, HookClient<Tin, Tout> client)
        {
            if (server_name == null || client == null)
                return true;
            if (HasRegister<Tin, Tout>(server_name))
                return (Servers[server_name] as Hook<Tin, Tout>).UnBind(client.Order);
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
    public class Hook<Tin, Tout> : IHookServer
    {
        public List<HookClient<Tin, Tout>> Clients { get; private set; } = new List<HookClient<Tin, Tout>>();
        public string RegisteredName = null;
        public Hook(string register_name = null)
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
        public bool UnRegister()
        {
            RegisteredName = null;
            return Core.HookManager.UnRegister(RegisteredName);
        }
        public List<Tout> CallAll(List<Tin> tins)
        {
            if (tins == null)
                return null;
            if (tins.Count != Clients.Count)
                return null;
            List<Tout> results = new List<Tout>(Clients.Count);
            for (int i = 0; i < Clients.Count; i++)
            {
                results.Add(Clients[i].Function(tins[i]));
            }
            return results;
        }
        public List<Tout> CallAll(Tin tin)
        {
            List<Tout> results = new List<Tout>(Clients.Count);
            for (int i = 0; i < Clients.Count; i++)
            {
                results.Add(Clients[i].Function(tin));
            }
            return results;
        }
        public Tout Call(int index, Tin parameter)
        {
            if (index < 0 || index >= Clients.Count)
                return default;
            return Clients[index].Function(parameter);
        }
        public bool Bind(HookClient<Tin, Tout> client)
        {
            if (client == null)
                return true;
            client.Order = Clients.Count;
            Clients.Add(client);
            client.Server = this;
            return false;
        }
        public HookClient<Tin, Tout> Bind(Func<Tin, Tout> function)
        {
            if (function == null)
                return null;
            var c = new HookClient<Tin, Tout>(function);
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
        public bool UnBind(HookClient<Tin, Tout> client)
        {
            if (client == null)
                return true;
            UnBind(client.Order);
            return false;
        }
    }
    public class HookClient<Tin, Tout>
    {
        public int Order = -1;
        public Func<Tin, Tout> Function = null;
        public Hook<Tin, Tout> Server = null;
        public HookClient(Func<Tin, Tout> function)
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