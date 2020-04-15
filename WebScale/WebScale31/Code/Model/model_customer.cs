using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;

namespace WebScale31.Code.Model
{
    [Serializable]
    public class dm_customer : TableEntity
    {
        public dm_customer()
        {
        }
        public dm_customer(string pk, string rk)
        {
            PartitionKey = pk; RowKey = rk;
        }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    [Serializable]
    public class vm_customer : dm_customer
    {
        public vm_customer() : base() { }
        public vm_customer(string pk, string rk) : base(pk, rk) { }
    }

    [Serializable]
    public class vm_customerlist : List<vm_customer>
    {

    }
}
