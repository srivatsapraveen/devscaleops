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
    public class Demo02Model : PageModel
    {
        public string Message { get; private set; } = "PageModel in C#";
        private readonly ILogger<IndexModel> _logger;

        public Demo02Model(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            action_network ac = new action_network();
            int j = 0;
            int count = 10;

            if (Request.Query["counter"].Count > 0)
                count = Convert.ToInt32(Request.Query["counter"]);
            TimeSpan time = Common.Time(() =>
            {
                for (int j = 0; j < count; j++)
                {
                    var netA = ac.Retrieve(1).GetAwaiter().GetResult();
                    if (netA == null)
                        Console.WriteLine("No records found for id = 1");
                    else
                        Message += netA.netname;
                }
            });
            Console.WriteLine("Time taken was : " + time); Message += "Iterations was : " + j + "<br/>";
            Message += "Time taken was : " + time;
        }
    }
}
