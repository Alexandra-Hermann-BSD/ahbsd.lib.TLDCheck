using System;
using ahbsd.lib.TLDCheck;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("TLD-Check Test");
            Console.WriteLine("==============");
            Console.WriteLine();

            IANA_TLD ianaTLDs = new IANA_TLD();

            check("de", ianaTLDs);
            check("a", ianaTLDs);
            check("aa", ianaTLDs);
            check("aaa", ianaTLDs);
            check("aaaa", ianaTLDs);
            check("us", ianaTLDs);
            check("org", ianaTLDs);
            check("nl", ianaTLDs);
            check("gov", ianaTLDs);
            check("fritz", ianaTLDs);
            check("iana", ianaTLDs);
            check("alex", ianaTLDs);
            check("it", ianaTLDs);

            Console.WriteLine();
            Console.WriteLine("Statistic:");
            Console.WriteLine("----------");
            Console.WriteLine();

            Console.WriteLine("Last answer from IANA: " +
                    $"{IANA_TLD.LastAnswer}");
            Console.WriteLine($"While running, there were {IANA_TLD.Reloads}" +
                " (re)loads.");
            Console.WriteLine("The last reload took " +
                $"{IANA_TLD.LastReloadTime.Minutes} minutes and " +
                $"{IANA_TLD.LastReloadTime.Seconds} seconds and " +
                $"{IANA_TLD.LastReloadTime.Milliseconds} milliseconds.");
        }

        static void check(string tld, IANA_TLD iANA_TLD)
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
