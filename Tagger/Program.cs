using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InsightlySDK;
using log4net;
using Newtonsoft.Json.Linq;

namespace Tagger
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (Program));
        public static void Main(string[] args)
        {
            ConfigureLog4Net();

            try
            {
                var apiKey = args[0];
                var i = new Insightly(apiKey);

                Logger.Info("Testing API .....");
                Logger.Info("Testing organization");

                var org = i.GetOrganization(69896301);
                Logger.InfoFormat("Org name is {0}.", org["ORGANISATION_NAME"]);

                var tags = org["TAGS"];
                foreach (var tag in tags)
                {
                    Logger.InfoFormat("Tag is {0}", tag["TAG_NAME"]);
                }

                var array = tags as JArray;
                array.Add(new JObject(new JProperty("TAG_NAME", "gt-test")));

                i.AddOrganization(org);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            Console.WriteLine("Press enter to continue...");
            Console.Read();
        }

        private static void ConfigureLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();
        }
    }
}
