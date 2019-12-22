using System;
using GameCore.Base;
namespace GameCore.Interface
{
    public class Control
    {
        public bool Load(string save_name)
        {
            if(!Core.Instance.INeed.IsSaveExist(save_name) || !Core.Instance.INeed.IsSaveDirLegal(save_name))
                return true;
            Core.Instance.Save_Name = save_name;
            var world_info = Core.Instance.Load.WorldInfo();
            if(Core.Instance.DataInit(world_info))
                return true;
            if(Core.Instance.Load.Rules())
                return true;
            return false;
        }
        public bool Save(string save_name = null)
        {
            if(save_name == null)
                return true;
            Core.Instance.Save_Name = save_name;
            // if not cover, create new dir
            if(!Core.Instance.INeed.IsSaveExist(save_name)){
                if(Core.Instance.INeed.NewSaveDir(save_name))
                    return true;
                if(Core.Instance.INeed.IsSaveDirLegal(save_name))
                    return true;
            }
            // order: info, ruledata, cards
            if(Core.Instance.Save.WorldInfo())
                return true;
            if(Core.Instance.Save.Rules())
                return true;
            if(Core.Instance.Save.CardAll())
                return true;
            return false;
        }
        public bool CreateNew(WorldInfo world_info)
        {
            if(Core.Instance.DataInit(world_info))
                return true;
            // call many rule's function about newing world
            return false;
        }
        public bool Exit()
        {
            Core.Instance.DataRemove();
            return false;
        }
    }
}