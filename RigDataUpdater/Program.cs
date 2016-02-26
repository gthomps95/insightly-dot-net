using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using InsightlySDK;
using log4net;
using LinqKit;

namespace RigDataUpdater
{
    class Program
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));
        private static Insightly Insightly { get; set; }

        public static void Main(string[] args)
        {
            ConfigureLog4Net();
            Logger.InfoFormat("Starting version {0}...", GetVersion());

            var apiKey = args[0];
            Insightly = new Insightly(apiKey);

            var program = new Program();
            program.Execute();

            Console.WriteLine("Press enter to continue...");
            Console.Read();
        }

        private static void ConfigureLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private void Execute()
        {
            var orgs = GetOrganizations().ToList();
            Logger.InfoFormat("Found {0} orgs", orgs.Count());

            ProcessRigData(orgs);
        }

        private void ProcessRigData(List<InsightlyOrg> orgs)
        {
            var reader = new ExcelReader(@"d:\rig_data\1ILP_20160226.XLS");
            var notFound = 0;
            var found = 0;
            var foundOrgs = new List<string>();

            File.Delete(@"d:\rig_data\orgs_nf.txt");
            File.Delete(@"d:\rig_data\orgs_f.txt");

            foreach (var rigData in reader.RigData.Where(rd => rd.Orientation.Equals("HOR")))
            {
                var searchName = MakeSearchName(rigData.Company);
                if (foundOrgs.Contains(searchName))
                    continue;
                foundOrgs.Add(searchName);

                var org = orgs.FirstOrDefault(o => o.OrgSearchName == searchName);

                if (org == null)
                {
                    notFound++;
                    Logger.InfoFormat("Org {0} was not found.", rigData.Company);
                    File.AppendAllText(@"d:\rig_data\orgs_nf.txt", $"{rigData.Company}|{rigData.State}|{rigData.County}|{rigData.Contact}\n");
                }
                else
                {
                    found++;
                    Logger.InfoFormat("Org {0} was found, id is {1}", rigData.Company, org.OrgId);
                    File.AppendAllText(@"d:\rig_data\orgs_f.txt", $"{rigData.Company}|{org.OrgId.ToString()}|{org.OrgName}\n");
                }

            }

            Logger.InfoFormat("Completed file.  Found: {0}, Not found: {1}.", found, notFound);

        }

        private IEnumerable<InsightlyOrg> GetOrganizations()
        {
            var orgs = Insightly.GetOrganizations();

            return orgs.Select(o => new InsightlyOrg
            {
                OrgId = Convert.ToInt32(o["ORGANISATION_ID"]),
                OrgName = o["ORGANISATION_NAME"].ToString(),
                OrgSearchName = MakeSearchName(o["ORGANISATION_NAME"].ToString())
            });
        }

        private string MakeSearchName(string name)
        {
            var result = name
                .ToLower();

            var excessWords = new[] {"oil", "gas", "energy", "petroleum",
                "production", "operating", "exploration", "development", "investments", "resources",
                "group", "company", "inc", "llc", "usa", "co.", "lp"};

            excessWords.ForEach(word => result = result.Replace(word, ""));

            const string specialChars = " -,.()&";
            specialChars.ForEach(c => result = result.Replace(c.ToString(), ""));

            return result;
        }
        public static String GetVersion()
        {
            return FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).FileVersion;
        }
    }

    public class InsightlyOrg
    {
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        public string OrgSearchName { get; set; }
    }

    public class RigData
    {
        //CODE1	API	STATE	DISTRICT	COUNTY	COM	AD1	CIT	STA	ZIP	CONTACTS	TL1	Field	LEASE	LEGAL	ACTIVITY	COMMENTS	wellorient
        public string RowNum { get; set; }
        public string Code { get; set; }
        public string ApiCode { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string County { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string AddressState { get; set; }
        public string Zip { get; set; }
        public string Contact { get; set; }
        public string Phone { get; set; }
        public string Field { get; set; }
        public string Lease { get; set; }
        public string Legal { get; set; }
        public string Activity { get; set; }
        public string Comments { get; set; }
        public string Orientation { get; set; }
    }
}
