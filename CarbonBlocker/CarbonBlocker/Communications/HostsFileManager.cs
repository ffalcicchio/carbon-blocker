﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarbonBlocker.Communications
{
    internal class HostsFileManager
    {
        private static ILog log = log4net.LogManager.GetLogger(typeof(HostsFileManager));
        private static string hostPath = @"C:\Windows\System32\drivers\etc\hosts";
        private static string beginCarbonBlockerSection = "#BEGIN CARBON BLOCKER SECTION#";
        private static string endCarbonBlockerSection = "#END CARBON BLOCKER SECTION#";
        public static void changeHostsFile(List<string> urls)
        {
            
            
            StringBuilder sb = new StringBuilder();
            sb.Append(beginCarbonBlockerSection);
            foreach (string url in urls)
            {
                string host = new Uri(url).Host.ToLower();
                sb.Append(Environment.NewLine + "127.0.0.1    " + host + Environment.NewLine);
                //Block site on /etc/hosts
                log.Info("************");
                log.Info("BLOCKING HOST " + host);
                log.Info("************");

            }
            sb.Append(endCarbonBlockerSection);


            //backup
            
            File.Copy(hostPath, hostPath + ".bck", true);
            string hostFileText = File.ReadAllText(hostPath);
            if (hostFileText.Contains(beginCarbonBlockerSection))
            {
                //Replace text
                int carbonBlockerSectionStartIndex = hostFileText.IndexOf(beginCarbonBlockerSection);
                int carbonBlockerSectionEndIndex = hostFileText.IndexOf(endCarbonBlockerSection) + endCarbonBlockerSection.Length;
                string carbonBlockerSection = hostFileText.Substring(carbonBlockerSectionStartIndex, carbonBlockerSectionEndIndex - carbonBlockerSectionStartIndex);
                string newHostFileText = hostFileText.Replace(carbonBlockerSection, sb.ToString());
                File.WriteAllText(hostPath, newHostFileText);
            }
            else
            {
                //Append text
                File.AppendAllText(hostPath, sb.ToString());
            }
        }
    }
}
