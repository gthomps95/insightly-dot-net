using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Excel;
using log4net;

namespace RigDataUpdater
{
    public class ExcelReader
    {
        // ReSharper disable once UnusedMember.Local
        private static readonly ILog Logger = LogManager.GetLogger(typeof(ExcelReader));
        private readonly string _filePath;

        public ExcelReader(string filePath)
        {
            _filePath = filePath;
        }

        public IEnumerable<RigData> RigData
        {
            get
            {
                using (var excelReader = GetExcelReader())
                {
                    //var count = 0;
                    var dataSet = excelReader.AsDataSet();

                    foreach (var table in dataSet.Tables.OfType<DataTable>())
                    {
                        foreach (var row in table.Rows.OfType<DataRow>().Skip(1))
                        {
                            var result = new RigData
                            {
                                Code = row[0].ToString().Trim(),
                                ApiCode = row[1].ToString().Trim(),
                                State = row[2].ToString().Trim(),
                                District = row[3].ToString().Trim(),
                                County = row[4].ToString().Trim(),
                                Company = row[5].ToString().Trim(),
                                Address = row[6].ToString().Trim(),
                                City = row[7].ToString().Trim(),
                                AddressState = row[8].ToString().Trim(),
                                Zip = row[9].ToString().Trim(),
                                Contact = row[10].ToString().Trim(),
                                Phone = row[11].ToString().Trim(),
                                Field = row[12].ToString().Trim(),
                                Lease = row[13].ToString().Trim(),
                                Legal = row[14].ToString().Trim(),
                                Activity = row[15].ToString().Trim(),
                                Comments = row[16].ToString().Trim(),
                                Orientation = row[17].ToString().Trim()
                            };

                            yield return result;
                        }
                    }
                }
            }
        }

        private IExcelDataReader GetExcelReader()
        {
            return ExcelReaderFactory.CreateBinaryReader(File.Open(_filePath, FileMode.Open, FileAccess.Read));
        }
    }
}
