using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MailKit.Models
{
    [DataContract]
    public class Message<T>
    {
        [DataMember(Name = "success")]
        public bool Success { get; set; }

        [DataMember(Name = "data")]
        public T Data { get; set; }

        [DataMember(Name = "errors")]
        public List<string> Errors { get; set; }
    }
}