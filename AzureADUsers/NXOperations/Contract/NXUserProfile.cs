using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ActiveDirectorySearch
{
    [DataContract]
    public class NXUserProfile 
    {
        [DataMember(Name = "Department")]
        public string Department { get; set; }
        [DataMember(Name = "EMailAddress")]
        public string EMailAddress { get; set; }
        [DataMember(Name = "FullName")]
        public string FullName { get; set; }
        [DataMember(Name = "Manager")]
        public string Manager { get; set; }
        [DataMember(Name = "RegisteredDate")]
        public DateTime RegisteredDate { get; set; }
        [DataMember(Name = "UserName")]
        public string UserName { get; set; }
        [DataMember(Name = "Title")]
        public string Title{ get; set; }
    }
}
