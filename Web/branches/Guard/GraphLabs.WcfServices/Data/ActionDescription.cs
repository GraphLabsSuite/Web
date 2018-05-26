using System;
using System.Runtime.Serialization;

namespace GraphLabs.WcfServices.Data
{
    /// <summary> Вариант задания </summary>
    [DataContract]
    public class ActionDescription
    {
        /// <summary> Описание действия </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary> Штрафные баллы </summary>
        [DataMember]
        public short Penalty { get; set; }

        /// <summary> Временная отметка </summary>
        [DataMember]
        public DateTime TimeStamp { get; set; }
    }
}
