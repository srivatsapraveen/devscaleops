using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using WebScale31.Code.Model;
using Microsoft.Extensions.Configuration;

namespace WebScale31.Code.Actions
{
    public class action_network
    {
        static string connectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("ConnectionStrings")["StorageConnectionString_SQL"];
        IDbConnection db = new SqlConnection(connectionString);

        /// <summary>
        /// The Table Service supports two main types of insert operations.
        ///  1. Insert - insert a new entity. If an entity already exists with the same PK + RK an exception will be thrown.
        ///  2. Replace - replace an existing entity. Replace an existing entity with a new entity.
        ///  3. Insert or Replace - insert the entity if the entity does not exist, or if the entity exists, replace the existing one.
        ///  4. Insert or Merge - insert the entity if the entity does not exist or, if the entity exists, merges the provided entity properties with the already existing ones.
        /// </summary>
        /// <param name="table">The sample table name</param>
        /// <param name="entity">The entity to insert or merge</param>
        /// <returns>A Task object</returns>
        public async Task<bool> CreateTable()
        {
            List<string> SQLs = GetTableScript();
            foreach (string sql in SQLs)
            {
                var sqlret = await db.ExecuteAsync(sql);
            }
            return true;
        }

        public List<string> GetTableScript()
        {
            List<string> dbscript = new List<string>();

            dbscript.Add("IF object_id('s01_site_network', 'U') is null create table s01_site_network (lastmodified datetime NOT NULL) ");
            dbscript.Add("IF COL_LENGTH('s01_site_network','netid') IS NULL ALTER TABLE [s01_site_network] ADD [netid] 	bigint identity	NOT NULL	");
            dbscript.Add("IF COL_LENGTH('s01_site_network','netcode') IS NULL ALTER TABLE [s01_site_network] ADD [netcode] 	varchar(10)	NULL	");
            dbscript.Add("IF COL_LENGTH('s01_site_network','netname') IS NULL ALTER TABLE [s01_site_network] ADD [netname] 	nvarchar(25)	NOT NULL	");
            dbscript.Add("IF COL_LENGTH('s01_site_network','nettype') IS NULL ALTER TABLE [s01_site_network] ADD [nettype] 	varchar(3)	NOT NULL	");
            dbscript.Add("IF COL_LENGTH('s01_site_network','netstatus') IS NULL ALTER TABLE [s01_site_network] ADD [netstatus] 	varchar(3)	NOT NULL	");
            dbscript.Add("IF COL_LENGTH('s01_site_network','pnetid') IS NULL ALTER TABLE [s01_site_network] ADD [pnetid] 	bigint	NOT NULL	");
            dbscript.Add("IF COL_LENGTH('s01_site_network','neturl') IS NULL ALTER TABLE [s01_site_network] ADD [neturl] 	nvarchar(25)	NULL	");
            dbscript.Add("IF COL_LENGTH('s01_site_network','netdesc') IS NULL ALTER TABLE [s01_site_network] ADD [netdesc] 	nvarchar(1000)	NULL	");
            dbscript.Add("IF object_id('PK_network', 'PK') is null ALTER TABLE s01_site_network ADD CONSTRAINT PK_network PRIMARY KEY NONCLUSTERED (netid)");

            return dbscript;
        }

        public async Task<vm_network> Insert(vm_network item)
        {
            String ins_query = "insert into s01_site_network " +
                " (lastmodified, netcode, netname, nettype, netstatus,pnetid, neturl, netdesc) " +
                " values (@lastmodified, @netcode, @netname, @nettype, @netstatus,@pnetid, @neturl, @netdesc) ";

            var affectedRows = await db.ExecuteAsync(ins_query, item);
            return item;
        }

        public async Task<vm_network> Replace(vm_network item)
        {
            String ins_query = "update s01_site_network set " +
                " lastmodified = @lastmodified, netcode = @netcode, netname = @netname, nettype = @nettype, netstatus = @netstatus,pnetid = @pnetid, neturl = @neturl, netdesc = @netdesc " +
                " where netid = @netid ";

            var affectedRows = await db.ExecuteAsync(ins_query, item);
            return item;
        }

        public async Task<vm_network> Merge(vm_network item)
        {
            String ins_query = "update s01_site_network set " +
                " lastmodified = @lastmodified, netcode = @netcode, netname = @netname, nettype = @nettype, netstatus = @netstatus,pnetid = @pnetid, neturl = @neturl, netdesc = @netdesc " +
                " where netid = @netid ";

            var affectedRows = await db.ExecuteAsync(ins_query, item);
            return item;
        }

        public async Task<vm_network> Retrieve(int pnetid)
        {
            String query = "select netid,netname from s01_site_network where netid=@netid";
            var net = await db.QuerySingleOrDefaultAsync<vm_network>(query, new { netid = pnetid });
            return net;
        }

        public async Task<IEnumerable<vm_network>> Query(string pneturl, string pnettype)
        {
            String query = "select netid,netname from s01_site_network where neturl like @neturl";
            var nets = await db.QueryAsync<vm_network>(query, new { neturl = pneturl });
            return nets;
        }

        //public async Task<vm_customer> Replace(vm_customer item)
        //{
        //    if (table == null) Init().Wait();
        //    TableOperation insert = TableOperation.Replace(item);
        //    TableResult result = await table.ExecuteAsync(insert);
        //    vm_customer insertedCustomer = result.Result as vm_customer;
        //    return insertedCustomer;
        //}

        //public async Task<vm_customer> Merge(vm_customer item)
        //{
        //    if (table == null) Init().Wait();
        //    TableOperation insert = TableOperation.Merge(item);
        //    TableResult result = await table.ExecuteAsync(insert);
        //    vm_customer insertedCustomer = result.Result as vm_customer;
        //    return insertedCustomer;
        //}

        //public async Task<vm_customer> InsesrtOrMerge(vm_customer item)
        //{
        //    if (table == null) Init().Wait();
        //    TableOperation insert = TableOperation.InsertOrMerge(item);
        //    TableResult result = await table.ExecuteAsync(insert);
        //    vm_customer insertedCustomer = result.Result as vm_customer;
        //    return insertedCustomer;
        //}

        //public async Task<vm_customer> InsertOrReplace(vm_customer item)
        //{
        //    if (table == null) Init().Wait();
        //    TableOperation insert = TableOperation.InsertOrReplace(item);
        //    TableResult result = await table.ExecuteAsync(insert);
        //    vm_customer insertedCustomer = result.Result as vm_customer;
        //    return insertedCustomer;
        //}

        //public async Task Delete(vm_customer item)
        //{
        //    if (table == null) Init().Wait();
        //    TableOperation insert = TableOperation.Delete(item);
        //    await table.ExecuteAsync(insert);
        //}

        //public async Task<vm_customer> Retrieve(string pk, string rk)
        //{
        //    if (table == null) Init().Wait();
        //    TableOperation insert = TableOperation.Retrieve<vm_customer>(pk,rk);
        //    TableResult result = await table.ExecuteAsync(insert);
        //    vm_customer retrievedCustomer = result.Result as vm_customer;
        //    return retrievedCustomer;
        //}

        //public async Task<vm_customer> Get(string pk, string rk)
        //{
        //    if (table == null) Init().Wait();
        //    TableOperation insert = TableOperation.Retrieve<vm_customer>(pk, rk);
        //    TableResult result = await table.ExecuteAsync(insert);
        //    vm_customer retrievedCustomer = result.Result as vm_customer;
        //    return retrievedCustomer;
        //}

    }
}
