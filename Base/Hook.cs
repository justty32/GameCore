using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    public class HookManager
    {
        /*
         * Just go create entities unrestricted,
         * cause all of those will visit same list of registered hooks.
         */
        private SortedList<string, object> hooks = new SortedList<string, object>();
        public bool RegisterHook<Tinput, Toutput>(Hook<Tinput, Toutput> hook, string hook_name) {
            //If there is already the same name hook, Return True
            if (hooks.ContainsKey(hook_name))
                return true;
            hooks.Add(hook_name, hook);
            return false;
        }
        public bool RegisterHook<Tinput>(Hook<Tinput> hook, string hook_name) {
            //If there is already the same name hook, Return True
            if (hooks.ContainsKey(hook_name))
                return true;
            hooks.Add(hook_name, hook);
            return false;
        }
        public bool RegisterHook(Hook hook, string hook_name) {
            //If there is already the same name hook, Return True
            if (hooks.ContainsKey(hook_name))
                return true;
            hooks.Add(hook_name, hook);
            return false;
        }
        public bool HasRegistered(string hook_name) => hooks.ContainsKey(hook_name);
        public IHookUser<Tinput, Toutput> GetHookUser<Tinput, Toutput>(string hook_name)
        {
            return hooks?[hook_name] as IHookUser<Tinput, Toutput>;
        }
        public IHookUser<Tinput> GetHookUser<Tinput>(string hook_name)
        {
            return hooks?[hook_name] as IHookUser<Tinput>;
        }
        public IHookUser GetHookUser(string hook_name)
        {
            return hooks?[hook_name] as IHookUser;
        }
        public int BindHook<Tinput, Toutput>(string hook_name, Func<Tinput, Toutput> func)
        {
            //Return func_index in hook's func_list
            var hook = hooks[hook_name] as Hook<Tinput, Toutput>;
            return (hook == null) ? -1 : hook.BindHook(func);
        }
        public int BindHook<Tinput>(string hook_name, Action<Tinput> func)
        {
            //Return func_index in hook's func_list
            var hook = hooks[hook_name] as Hook<Tinput>;
            return (hook == null) ? -1 : hook.BindHook(func);
        }
        public int BindHook(string hook_name, Action func)
        {
            //Return func_index in hook's func_list
            var hook = hooks[hook_name] as Hook;
            return (hook == null) ? -1 : hook.BindHook(func);
        }
        public void UnbindHook<Tinput, Toutput>(string hook_name, Func<Tinput, Toutput> func)
        {
            var hook = hooks[hook_name] as Hook<Tinput, Toutput>;
            if (hook != null)
                if (hook.HasFunc(func))
                    hook.UnbindHook(func);
        }
        public void UnbindHook<Tinput>(string hook_name, Action<Tinput> func)
        {
            var hook = hooks[hook_name] as Hook<Tinput>;
            if (hook != null)
                if (hook.HasFunc(func))
                    hook.UnbindHook(func);
        }
        public void UnbindHook(string hook_name, Action func)
        {
            var hook = hooks[hook_name] as Hook;
            if (hook != null)
                if (hook.HasFunc(func))
                    hook.UnbindHook(func);
        }
        public void UnbindHookAt<Tinput, Toutput>(string hook_name, int func_index)
        {
            var hook = hooks[hook_name] as Hook<Tinput, Toutput>;
            if (hook != null)
                    hook.UnbindHookAt(func_index);
        }
        public void UnbindHookAt<Tinput>(string hook_name, int func_index)
        {
            var hook = hooks[hook_name] as Hook<Tinput>;
            if (hook != null)
                if (hook != null)
                    hook.UnbindHookAt(func_index);
        }
        public void UnbindHookAt(string hook_name, int func_index)
        {
            var hook = hooks[hook_name] as Hook;
            if (hook != null)
                if (hook != null)
                    hook.UnbindHookAt(func_index);
        }
    }
    public interface IHookUser<Tinput, Toutput>
    {
        int BindHook(Func<Tinput, Toutput> func);
        void UnbindHook(Func<Tinput, Toutput> func);
        void UnbindHookAt(int func_index);
        int GetFuncIndex(Func<Tinput, Toutput> func);
        bool HasFunc(Func<Tinput, Toutput> func);
        int GetFuncAmount();
    }
    public interface IHookUser<Tinput>
    {
        int BindHook(Action<Tinput> func);
        void UnbindHook(Action<Tinput> func);
        void UnbindHookAt(int func_index);
        int GetFuncIndex(Action<Tinput> func);
        bool HasFunc(Action<Tinput> func);
        int GetFuncAmount();
    }
    public interface IHookUser
    {
        int BindHook(Action func);
        void UnbindHook(Action func);
        void UnbindHookAt(int func_index);
        int GetFuncIndex(Action func);
        bool HasFunc(Action func);
        int GetFuncAmount();
    }
    public class Hook<Tinput, Toutput> : IHookUser<Tinput, Toutput>
    {
        protected List<Func<Tinput, Toutput>> ts = new List<Func<Tinput, Toutput>>();
        public string RegisteredName { get; }
        public bool HasRegistered { get; }
        private Hook() { }
        public Hook(string name) {
            RegisteredName = name;
            HasRegistered = true;
            StringBuilder stringBuilder = new StringBuilder(RegisteredName, 500);
            if (Core.Instance.HookManager.RegisterHook<Tinput, Toutput>(this, name))
            {
                stringBuilder.Append("_1");
                for (int i = 2; i < 100; i++)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    stringBuilder.Append(i);
                    if (!Core.Instance.HookManager.RegisterHook<Tinput, Toutput>(this, stringBuilder.ToString()))
                        goto end;
                }
                stringBuilder = null;
                HasRegistered = false;
            }
            end:
            RegisteredName = (stringBuilder != null) ? (stringBuilder.ToString()) : (null);
        }
        public IHookUser<Tinput, Toutput> GetHookUser() => this;
        public Func<Tinput, Toutput> GetFuncAt(int index) => (index < 0 || index > ts.Count) ? (null) : (ts?[index]);
        public void CallAll(Tinput in_thing)
        {
            if (in_thing != null)
                foreach (var iter in ts)
                {
                    iter(in_thing);
                }
        }
        public bool CallAll(Tinput in_thing, List<Toutput> out_things)
        {
            if (out_things == null)
                return true;
            else if (out_things.Count != ts.Count)
                return true;
            else
            {
                for (int i = 0; i < ts.Count; i++)
                {
                    out_things[i] = ts[i](in_thing);
                }
            }
            return false;
        }
        public void CallAll(List<Tinput> in_things)
        {
            if (in_things != null)
                if (in_things.Count == ts.Count)
                {
                    for (int i = 0; i < ts.Count; i++)
                    {
                        ts[i](in_things[i]);
                    }
                }
        }
        public bool CallAll(List<Tinput> in_things, List<Toutput> out_things)
        {
            if (in_things == null)
                return true;
            if (in_things.Count != ts.Count)
                return true;
            if (out_things == null)
                return true;
            else if (out_things.Count != ts.Count)
                return true;
            else
            {
                for (int i = 0; i < ts.Count; i++)
                {
                    out_things[i] = ts[i](in_things[i]);
                }
            }
            return false;
        }
        public void CallAt(int index, Tinput in_thing)
        {
            if (index < 0 || index > ts.Count) { }
            else if (ts[index] == null) { }
            else
            {
                ts[index](in_thing);
            }
        }
        public void CallAt(int index, Tinput in_thing, Toutput out_thing)
        {
            if (index < 0 || index > ts.Count) { }
            else if (ts[index] == null) { }
            else
            {
                out_thing = ts[index](in_thing);
            }
        }
        public int BindHook(Func<Tinput, Toutput> func)
        {
            //Return func_index in hook's func_list
            if (HasFunc(func))
                return GetFuncIndex(func);
            ts.Add(func);
            return ts.Count - 1;
        }
        public void UnbindHook(Func<Tinput, Toutput> func) => ts.Remove(func);
        public void UnbindHookAt(int func_index)
        {
            if (func_index < ts.Count && func_index >= 0)
                ts.RemoveAt(func_index);
        }
        public bool HasFunc(Func<Tinput, Toutput> func) => ts.Contains(func);
        public int GetFuncAmount() => ts.Count;
        public int GetFuncIndex(Func<Tinput, Toutput> func) => ts.FindIndex(pref => pref.Equals(func));
    }
    public class Hook<Tinput> : IHookUser<Tinput>
    {
        protected List<Action<Tinput>> ts = new List<Action<Tinput>>();
        public string RegisteredName { get; }
        public bool HasRegistered { get; }
        private Hook() { }
        public Hook(string name)
        {
            RegisteredName = name;
            HasRegistered = true;
            StringBuilder stringBuilder = new StringBuilder(RegisteredName, 500);
            if (Core.Instance.HookManager.RegisterHook<Tinput>(this, name))
            {
                stringBuilder.Append("_1");
                for (int i = 2; i < 100; i++)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    stringBuilder.Append(i);
                    if (!Core.Instance.HookManager.RegisterHook<Tinput>(this, stringBuilder.ToString()))
                        goto end;
                }
                stringBuilder = null;
                HasRegistered = false;
            }
            end:
            RegisteredName = (stringBuilder != null) ? (stringBuilder.ToString()) : (null);
        }
        public IHookUser<Tinput> GetHookUser() => this;
        public Action<Tinput> GetFuncAt(int index) => (index < 0 || index > ts.Count) ? (null) : (ts?[index]);
        public void CallAll(Tinput in_thing)
        {
            if (in_thing != null)
                foreach (var iter in ts)
                {
                    iter(in_thing);
                }
        }
        public void CallAll(List<Tinput> in_things)
        {
            if (in_things != null)
                if (in_things.Count == ts.Count)
                {
                    for (int i = 0; i < ts.Count; i++)
                    {
                        ts[i](in_things[i]);
                    }
                }
        }
        public void CallAt(int index, Tinput in_thing)
        {
            if (index < 0 || index > ts.Count) { }
            else if (ts[index] == null) { }
            else
            {
                ts[index](in_thing);
            }
        }
        public int BindHook(Action<Tinput> func)
        {
            //Return func_index in hook's func_list
            if (HasFunc(func))
                return GetFuncIndex(func);
            ts.Add(func);
            return ts.Count - 1;
        }
        public void UnbindHook(Action<Tinput> func) => ts.Remove(func);
        public void UnbindHookAt(int func_index)
        {
            if (func_index < ts.Count && func_index >= 0)
                ts.RemoveAt(func_index);
        }
        public bool HasFunc(Action<Tinput> func) => ts.Contains(func);
        public int GetFuncAmount() => ts.Count;
        public int GetFuncIndex(Action<Tinput> func) => ts.FindIndex(pref => pref.Equals(func));
    }
    public class Hook : IHookUser
    {
        protected List<Action> ts = new List<Action>();
        public string RegisteredName { get; }
        public bool HasRegistered { get; }
        private Hook() { }
        public Hook(string name)
        {
            RegisteredName = name;
            HasRegistered = true;
            StringBuilder stringBuilder = new StringBuilder(RegisteredName, 500);
            if (Core.Instance.HookManager.RegisterHook(this, name))
            {
                stringBuilder.Append("_1");
                for (int i = 2; i < 100; i++)
                {
                    stringBuilder.Remove(stringBuilder.Length - 1, 1);
                    stringBuilder.Append(i);
                    if (!Core.Instance.HookManager.RegisterHook(this, stringBuilder.ToString()))
                        goto end;
                }
                stringBuilder = null;
                HasRegistered = false;
            }
            end:
            RegisteredName = (stringBuilder != null) ? (stringBuilder.ToString()) : (null);
        }
        public IHookUser GetHookUser() => this;
        public int GetFuncsAmount() => ts.Count;
        public Action GetFuncAt(int index) => (index < 0 || index > ts.Count) ? (null) : (ts?[index]);
        public void CallAll()
        {
            foreach (var iter in ts)
            {
                iter();
            }
        }
        public void CallAt(int index)
        {
            if (index < 0 || index > ts.Count) { }
            else if (ts[index] == null) { }
            else
            {
                ts[index]();
            }
        }
        public int BindHook(Action func)
        {
            //Return func_index in hook's func_list
            if (HasFunc(func))
                return GetFuncIndex(func);
            ts.Add(func);
            return ts.Count - 1;
        }
        public void UnbindHook(Action func) => ts.Remove(func);
        public void UnbindHookAt(int func_index)
        {
            if (func_index < ts.Count && func_index >= 0)
                ts.RemoveAt(func_index);
        }
        public bool HasFunc(Action func) => ts.Contains(func);
        public int GetFuncAmount() => ts.Count;
        public int GetFuncIndex(Action func) => ts.FindIndex(pref => pref.Equals(func));
    }
}
