using System;
using System.Collections.Generic;

namespace WebScale31.Code.Model
{
    [Serializable]
    public class dm_network
    {
        public DateTime lastmodified { get; set; }
        public double netid { get; set; }
        public string netcode { get; set; }
        public string netuid { get; set; }
        public string netname { get; set; }
        public string netstatus { get; set; }
        public string nettype { get; set; }
        public double pnetid { get; set; }
        public string neturl { get; set; }
        public string netdesc { get; set; }
        public string netB { get; set; }
        public string netC { get; set; }
    }

    [Serializable]
    public class vm_network : dm_network
    {

    }

    [Serializable]
    public class vm_networklist : List<vm_network>
    {

    }
}
