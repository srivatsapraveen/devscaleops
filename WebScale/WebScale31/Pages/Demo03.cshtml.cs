using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using WebScale31.Code;
using WebScale31.Code.Model;
using WebScale31.Code.Actions;

namespace WebScale31.Pages
{
    public class Demo03Model : PageModel
    {
        public string Message { get; private set; } = "PageModel in C#";
        private readonly ILogger<IndexModel> _logger;

        public Demo03Model(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            action_customer ac = new action_customer("StorageConnectionString_CMS");
            int j = 0;

            int count = 10;

            if (Request.Query["counter"].Count > 0)
                count = Convert.ToInt32(Request.Query["counter"]);

            if (Common.IsAzureCosmosdbTable())
                Message += "DB is Az COSMOS <br/>";
            else
                Message += "DB is Az TABLE<br/>";

            TimeSpan time = Common.Time(() =>
            {
                for (j = 0; j < count; j++)
                {
                    var it = ac.Query("Walter@contoso.com").GetAwaiter().GetResult();
                    foreach (vm_customer i in it)
                    {
                        Message += i.RowKey + ":" + i.PartitionKey + ":" + i.Email + "<br/>";
                    }
                }
            });
            Message += "Iterations was : " + j + "<br/>";
            Message += "Time taken was : " + time;
        }
    }
}
