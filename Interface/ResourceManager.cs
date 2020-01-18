using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

// TODO : model, action, face, body, thing, material...

namespace GameCore.Interface
{
    class ResourceManager
    {
        // models
        public Dictionary<int, string> Organism = new Dictionary<int, string>();
        public Dictionary<int, string> Scene = new Dictionary<int, string>();
        public Dictionary<int, string> Terrain = new Dictionary<int, string>();
        public Dictionary<int, string> Artical = new Dictionary<int, string>();
        // materials
        public Dictionary<int, string> Material = new Dictionary<int, string>();
        // effects
        public Dictionary<int, string> Partical = new Dictionary<int, string>();
        public bool FromJsonObject(JObject json)
        {
            if (json == null)
                return true;
            try
            {
                var rm = json.ToObject<ResourceManager>();
                Organism = rm.Organism;
                Scene = rm.Scene;
                Artical = rm.Artical;
                Terrain = rm.Terrain;
                Material = rm.Material;
                Partical = rm.Partical;
            } catch(Exception e)
            {
                return Core.State.WriteException(e);
            }
            return false;
        }
        public JObject ToJsonObject()
        {
            JObject json = new JObject();
            try
            {
                json = JObject.FromObject(this);
            }catch(Exception e)
            {
                return Core.State.WriteException<JObject>(e);
            }
            return json;
        }
    }
}
