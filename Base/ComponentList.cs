using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore.Base
{
    //Still not usable, Do Not Use
    //class ComponentsList
    //{
    //    //multiple same type components allow
    //    private SortedList<int, List<Component>> components = new SortedList<int, List<Component>>();
    //    public IList<int> GetComponentTypeNumbers()
    //    {
    //        return components.Keys;
    //    }
    //    public bool HasComponent(int dst_num)
    //    {
    //        if (components.ContainsKey(dst_num))
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //    public bool HasComponents(int[] dst_nums)
    //    {
    //        foreach(int dst_num in dst_nums)
    //        {
    //            if (!components.ContainsKey(dst_num))
    //            {
    //                return false;
    //            }
    //        }
    //        return true;
    //    }
    //    public int GetComponentCount(int dst_num)
    //    {
    //        foreach (var node in components)
    //        {
    //            //if find the node
    //            if (node.Key == dst_num)
    //            {
    //                //check if list of components is good
    //                if (node.Value == null)
    //                    return 0;
    //                //return amount
    //                return node.Value.Count;
    //            }
    //        }
    //        return 0;
    //    }
    //    public List<Component> GetComponents(int dst_num)
    //    {
    //        foreach (var node in components)
    //        {
    //            if (node.Key == dst_num)
    //                return node.Value;
    //        }
    //        return null;
    //    }
    //    public void AddComponent(Component thing)
    //    {
    //        int number = thing.TypeNumber;
    //        if (!HasComponent(number))
    //        {
    //            components.Add(number, new List<Component>());
    //        }
    //        else
    //        {
    //            GetComponents(number).Add(thing);
    //        }
    //    }
    //    public void AddComponents(Component[] things)
    //    {
    //        // components of array could be multi type.
    //        if (things == null)
    //            goto end;
    //        int number;
    //        foreach (Component thing in things)
    //        {
    //            number = thing.TypeNumber;
    //            if (!HasComponent(number))
    //            {
    //                components.Add(number, new List<Component>());
    //            }
    //            else
    //            {
    //                GetComponents(number).Add(thing);
    //            }
    //        }
    //        end:
    //        _ = 0;
    //    }
    //    public void RemoveComponents(int dst_num)
    //    {
    //        if (HasComponent(dst_num))
    //        {
    //            components.Remove(dst_num);
    //        }
    //    }
    //}
}
