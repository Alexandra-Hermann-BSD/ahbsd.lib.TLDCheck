using NUnit.Framework;
using ahbsd.lib.TLDCheck;
using ahbsd.lib.TLDCheck.IANA;
using ahbsd.lib.TLDCheck.Statistic;
using System.Collections.Generic;

namespace NUnitTest
{
    public class Tests
    {
        protected IIANA_TLD tlds;
        protected IDownloadStatistic downloadStatistic;
        protected IList<string> tldList;

        [SetUp]
        public void Setup()
        {
            tlds = new IANA_TLD();
            tldList = new List<string>();

            tldList.Add("de");
            tldList.Add("gov");
            tldList.Add("bullshit");
            tldList.Add("ru");
            tldList.Add("at");
            tldList.Add("au");
        }

        [Test]
        public void Test1()
        {
            bool tmp1, tmp2;

            tmp2 = true;

            foreach (var tld in tldList)
            {
                try
                {
                    tmp1 = tlds.CheckTLD(tld);
                }
                catch (System.Exception ex)
                {
                    tmp2 = false;
                    Assert.Fail(ex.Message);
                }
            }

            downloadStatistic = new DownloadStatistic(tlds);

            if (tmp2)
            {
                Assert.Pass();
            }
        }
    }
}