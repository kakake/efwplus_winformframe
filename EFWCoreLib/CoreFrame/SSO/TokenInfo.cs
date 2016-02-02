using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace EFWCoreLib.CoreFrame.SSO
{
    public class TokenInfo
    {
        public Guid tokenId { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime ActivityTime { get; set; }

        public string RemoteIp { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public bool IsValid { get; set; }
    }
}
