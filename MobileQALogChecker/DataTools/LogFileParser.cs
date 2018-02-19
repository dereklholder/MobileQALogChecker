using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace MobileQALogChecker.DataTools
{
    public class LogFileParser
    {
        private string _logPath;
        private string _logString;
        private string _tempFilePath = AppDomain.CurrentDomain.BaseDirectory + "temp.txt";

        public LogFileParser(string logPath)
        {
            _logPath = logPath;
            _logString = File.ReadAllText(_logPath, Encoding.UTF8);
        }

        //Returns the raw XMl and an array of XML strings used for created the treeview and displaying the raw XMl.
        public string GetXmlEntries(string transactionIDOrderID, out string[] arrayOfXmlStrings)
        {

            //GetRidOfReceiptLineBreaks();

            List<string> gatewayResponseEntries = File.ReadLines(_logPath)
                                                      .Where(line => line.Contains("GatewayResponse"))
                                                      .Select(line => line)
                                                      .ToList();

            List<string> allTransactionsThatMatchCriteria = gatewayResponseEntries.FindAll(x => x.Contains(transactionIDOrderID))
                                                                                   .ToList();

            //Trims strings

            string[] trimmedStrings = new string[allTransactionsThatMatchCriteria.Count];
            for (int i = 0; i < allTransactionsThatMatchCriteria.Count; i++)
            {
                trimmedStrings[i] = allTransactionsThatMatchCriteria[i].ToString().Remove(0, allTransactionsThatMatchCriteria[i].IndexOf('<'));
                trimmedStrings[i] = trimmedStrings[i].Replace("<?xml version=\"1.0\"?>", "");
                if (trimmedStrings[i].Contains(@"<Receipt>"))
                {
                    trimmedStrings[i] = trimmedStrings[i] + @"</Receipt></GatewayResponse>";
                }
            }

            string returnValue = string.Join( Environment.NewLine, trimmedStrings);
            arrayOfXmlStrings = trimmedStrings;
            return returnValue;
        }

        //Implement Later, Receipt ruins solution, but not needed with workaround on line 44
        private void GetRidOfReceiptLineBreaks()
        {
            List<string> allLogFileLines = File.ReadAllLines(_logPath)
                                               .ToList();

            foreach (string line in allLogFileLines)
            {
                if (Regex.Match(line, @"^(?!.*-.*-.*::).*$").Success)
                {
                    line.Trim('\n');
                }
            }

            _logString = string.Join(",", allLogFileLines.ToArray());

            if (File.Exists(_tempFilePath))
            {
                File.Delete(_tempFilePath);
            }
            File.WriteAllText(_tempFilePath, _logString);
            _logPath = _tempFilePath;
        }
    }
}
