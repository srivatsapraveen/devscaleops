using System;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using WebScale31.Code.Model;
using System.Linq;

namespace WebScale31.Code.Actions
{
    public class action_customer
    {
        string tableName = "demotbl";
        CloudTable table;
        private async Task Init()
        {
            table = await Common.CreateTableAsync(tableName);
        }

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
        public async Task<vm_customer> Insert(vm_customer item)
        {
            if (table == null) Init().Wait();
            if (item == null)
            {
                throw new ArgumentNullException("entity");
            }

            try
            {

                // Create the InsertOrReplace table operation
                TableOperation insert = TableOperation.Insert(item);

                // Execute the operation.
                TableResult result = await table.ExecuteAsync(insert);
                vm_customer insertedCustomer = result.Result as vm_customer;

                return insertedCustomer;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
        }

        public async Task<vm_customer> Replace(vm_customer item)
        {
            if (table == null) Init().Wait();
            TableOperation insert = TableOperation.Replace(item);
            TableResult result = await table.ExecuteAsync(insert);
            vm_customer insertedCustomer = result.Result as vm_customer;
            return insertedCustomer;
        }

        public async Task<vm_customer> Merge(vm_customer item)
        {
            if (table == null) Init().Wait();
            TableOperation insert = TableOperation.Merge(item);
            TableResult result = await table.ExecuteAsync(insert);
            vm_customer insertedCustomer = result.Result as vm_customer;
            return insertedCustomer;
        }

        public async Task<vm_customer> InsesrtOrMerge(vm_customer item)
        {
            if (table == null) Init().Wait();
            TableOperation insert = TableOperation.InsertOrMerge(item);
            TableResult result = await table.ExecuteAsync(insert);
            vm_customer insertedCustomer = result.Result as vm_customer;
            return insertedCustomer;
        }

        public async Task<vm_customer> InsertOrReplace(vm_customer item)
        {
            if (table == null) Init().Wait();
            TableOperation insert = TableOperation.InsertOrReplace(item);
            TableResult result = await table.ExecuteAsync(insert);
            vm_customer insertedCustomer = result.Result as vm_customer;
            return insertedCustomer;
        }

        public async Task Delete(vm_customer item)
        {
            if (table == null) Init().Wait();
            TableOperation insert = TableOperation.Delete(item);
            await table.ExecuteAsync(insert);
        }

        public async Task<vm_customer> Retrieve(string pk, string rk)
        {
            if (table == null) Init().Wait();
            TableOperation insert = TableOperation.Retrieve<vm_customer>(pk, rk);
            TableResult result = await table.ExecuteAsync(insert);
            vm_customer retrievedCustomer = result.Result as vm_customer;
            return retrievedCustomer;
        }

        public async Task<IQueryable<vm_customer>> Query(string p_email)
        {
            if (table == null) await Init();

            IQueryable<vm_customer> linqQuery = table.CreateQuery<vm_customer>()
                        .Where(x => x.Email == p_email)
                        .Select(x => new vm_customer() { PartitionKey = x.PartitionKey, RowKey = x.RowKey, Email = x.Email });

            return linqQuery;
        }

    }
}
