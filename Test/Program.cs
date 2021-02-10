using System;
using System.Collections.Generic;
using ahbsd.lib.TLDCheck;
using ahbsd.lib.TLDCheck.IANA;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<string> tlds = new List<string>();

            tlds.Add("de");
            tlds.Add("a");
            tlds.Add("aa");
            tlds.Add("aaa");
            tlds.Add("aaaa");
            tlds.Add("us");
            tlds.Add("org");
            tlds.Add("nl");
            tlds.Add("fritz");
            tlds.Add("box");
            tlds.Add("iana");
            tlds.Add("alex");
            tlds.Add("it");

            Console.WriteLine("TLD-Check Test");
            Console.WriteLine("==============");
            Console.WriteLine();

            IANA_TLD ianaTLDs = new IANA_TLD();

            foreach (var item in tlds)
            {
                check(item, ianaTLDs);
            }

            Console.WriteLine();
            Console.WriteLine("Statistic:");
            Console.WriteLine("----------");
            Console.WriteLine();

            Console.WriteLine("Last answer from IANA: " +
                    $"{IANA_TLD.LastAnswer}");
            Console.WriteLine($"Last Response Status: {IANA_TLD.LastResponseStatus}");
            Console.WriteLine($"While running, there were {IANA_TLD.Reloads}" +
                " (re)loads.");
            Console.WriteLine("The last reload took " +
                $"{IANA_TLD.LastReloadTime.Minutes} minutes and " +
                $"{IANA_TLD.LastReloadTime.Seconds} seconds and " +
                $"{IANA_TLD.LastReloadTime.Milliseconds} milliseconds.");
        }

        static void check(string tld, IIANA_TLD iANA_TLD)
        {
            if (iANA_TLD.CheckTLD(tld))
            {
                Console.WriteLine($"'{tld}' exists.");
            }
            else
            {
                Console.WriteLine($"'{tld}' doesn't exists.");
            }
        }
    }
}
