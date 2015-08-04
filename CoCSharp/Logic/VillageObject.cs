using CoCSharp.Data;
using Newtonsoft.Json;
using System;

namespace CoCSharp.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class VillageObject
    {
        /// <summary>
        /// 
        /// </summary>
        public VillageObject(Village village)
        {
            ID = -1;
            Village = village;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public VillageObject(int id, Village village)
        {
            ID = id;
            Village = village;
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("data")]
        public int ID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("x")]
        public virtual int X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("y")]
        public virtual int Y { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public virtual Village Village { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public virtual CoCData Data { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public virtual bool CanUpgrade
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public virtual bool IsUpgrading
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void FinishConstruction()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void CancelConstruction()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Construct()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Upgrade()
        {
            throw new NotImplementedException();
        }
    }
}
