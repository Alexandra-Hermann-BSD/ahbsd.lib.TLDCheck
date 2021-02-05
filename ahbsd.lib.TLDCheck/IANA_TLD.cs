//
//  Copyright 2021  Alexandra Hermann – Beratung, Software, Design
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace ahbsd.lib.TLDCheck
{
    /// <summary>
    /// Class to check a given TLD, if it is registered by
    /// IANA https://www.iana.org
    /// </summary>
    public class IANA_TLD : IIANA_TLD
    {
        /// <summary>
        /// Constant of the source by IANA.
        /// </summary>
        public const string Path = "/TLD/tlds-alpha-by-domain.txt";
        /// <summary>
        /// The Uri to the data host from IANA.
        /// </summary>
        public static readonly Uri dataIANA = new Uri("https://data.iana.org");

        /// <summary>
        /// Gets a List that contains all TLDs, that are listed by IANA.
        /// </summary>
        /// <value>
        /// A List that contains all TLDs, that are listed by IANA.
        /// </value>
        public static IList<string> TLDs { get; private set; }

        /// <summary>
        /// Gets the last answer (first line) from fetching the TLD-List.
        /// </summary>
        public static string LastAnswer { get; private set; }

        /// <summary>
        /// When was the last Reload?
        /// </summary>
        private static DateTime lastCheck;

        /// <summary>
        /// Gets the <see cref="TimeSpan"/> it took to reload data.
        /// </summary>
        public static TimeSpan LastReloadTime { get; private set; }
        /// <summary>
        /// Gets the amount of reloads.
        /// </summary>
        public static ulong Reloads { get; private set; }

        /// <summary>
        /// The RestClient to load the data from IANA.
        /// </summary>
        private static IRestClient RestClient;

        /// <summary>
        /// The default timespan, before a new reload will be done.
        /// </summary>
        protected TimeSpan defaultTimeSpan;

        /// <summary>
        /// Static constructor.
        /// </summary>
        static IANA_TLD()
        {
            TLDs = new List<string>();
            lastCheck = DateTime.MinValue;
            RestClient = new RestClient(dataIANA);
            Reloads = 0;
            LastReloadTime = default;
        }

        /// <summary>
        /// Constructor without any parameters.
        /// </summary>
        public IANA_TLD()
        {
            defaultTimeSpan = new TimeSpan(24, 0, 0);
        }

        /// <summary>
        /// Constructor with a parameter for reloading data timespan.
        /// </summary>
        /// <param name="waitToReload">
        /// The <see cref="TimeSpan"/> for reloading data.
        /// </param>
        public IANA_TLD(TimeSpan waitToReload)
        {
            defaultTimeSpan = waitToReload;
        }

        #region implementation of IIANA_TLD
        /// <summary>
        /// Checks the given TLD.
        /// </summary>
        /// <param name="tld">The given TLD.</param>
        /// <returns>
        /// <c>true</c> if the TLD is known by IANA, otherwise <c>false</c>.
        /// </returns>
        /// <remarks>
        /// It doesn't matter, if the given TLD has spaces around or is in upper
        /// or lower case, because this will be automatically corrected. 
        /// </remarks>
        public bool CheckTLD(string tld)
        {
            TimeSpan offset = DateTime.Now.Subtract(lastCheck);
            bool result;

            if (offset > defaultTimeSpan)
            {
                result = ReloadData(offset);
            }

            return StaticCheckTLD(tld);
        }

        /// <summary>
        /// Checks the given TLD and sets a <see cref="TimeSpan"/> how long to
        /// wait for the next update of data..
        /// </summary>
        /// <param name="tld">The given TLD.</param>
        /// <param name="timeSpan">A <see cref="TimeSpan"/> how long to
        /// wait for the next update of data.</param>
        /// <returns>
        /// <c>true</c> if the TLD is known by IANA, otherwise <c>false</c>.
        /// </returns>
        /// <remarks>
        /// It doesn't matter, if the given TLD has spaces around or is in upper
        /// or lower case, because this will be automatically corrected. 
        /// </remarks>
        public bool CheckTLD(string tld, TimeSpan timeSpan)
            => StaticCheckTLD(tld, timeSpan);

        /// <summary>
        /// Checks the TLD in the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The given <see cref="Uri"/>.</param>
        /// <returns>
        /// <c>true</c> if the TLD is known by IANA, otherwise <c>false</c>.
        /// </returns>
        public bool CheckTLD(Uri uri)
        {
            TimeSpan offset = DateTime.Now.Subtract(lastCheck);
            bool result;

            if (offset > defaultTimeSpan)
            {
                result = ReloadData(offset);
            }

            return CheckTLD(uri, offset);
        }

        /// <summary>
        /// Checks the TLD in the given <see cref="Uri"/> and sets a
        /// <see cref="TimeSpan"/> how long to wait for the next update of data.
        /// </summary>
        /// <param name="uri">The given <see cref="Uri"/>.</param>
        /// <param name="timeSpan">A <see cref="TimeSpan"/> how long to
        /// wait for the next update of data.</param>
        /// <returns>
        /// <c>true</c> if the TLD is known by IANA, otherwise <c>false</c>.
        /// </returns>
        public bool CheckTLD(Uri uri, TimeSpan timeSpan)
        {
            string tld = string.Empty;
            string[] url;

            if (uri != null)
            {
                url = uri.Host.Split('.');
                tld = url.Last();
            }

            return StaticCheckTLD(tld, timeSpan);
        }
        #endregion

        /// <summary>
        /// Reloads the data from IANA.
        /// </summary>
        /// <param name="offset">
        /// [optional] <see cref="TimeSpan"/> to wait from the last
        /// <see cref="ReloadData(TimeSpan)"/> to reload data.
        /// </param>
        /// <returns>
        /// <c>true</c> if the amount of TLDs is greater than zero.
        /// </returns>
        protected static bool ReloadData(TimeSpan offset = default)
        {
            bool result = false;
            TimeSpan currentOffset = default;

            if (lastCheck != DateTime.MinValue)
            {
                currentOffset = DateTime.Now.Subtract(lastCheck);
            }


            if (TLDs.Count == 0 && currentOffset == default) // nothing before…
            {
                result = (GetDataFromIANA() > 0);

                lastCheck = DateTime.Now;
            }
            else if (TLDs.Count > 0 && currentOffset > offset)
            {
                TLDs.Clear();

                result = (GetDataFromIANA() > 0);

                lastCheck = DateTime.Now;
            }

            return result;
        }

        /// <summary>
        /// Gets the data from IANA.
        /// </summary>
        /// <returns>The amount of loaded TLDs.</returns>
        private static int GetDataFromIANA()
        {
            int result = 0;
            DateTime start = DateTime.Now;
            DateTime end;
            IRestRequest request = new RestRequest(Path, Method.GET, DataFormat.None);
            IRestResponse response = RestClient.Execute(request);
            string[] lines = null;

            if (response.ResponseStatus == ResponseStatus.Completed
                || response.ResponseStatus == ResponseStatus.None)
            {
                lines = response.Content.Split('\n');
            }

            if (lines != null && lines.Length > 1)
            {
                result = lines.Length;
                LastAnswer = lines[0].Trim();

                for (int i = 1; i < lines.Length; i++)
                {
                    TLDs.Add(lines[i].Trim());
                }
            }

            end = DateTime.Now;
            LastReloadTime = end.Subtract(start);
            Reloads++;

            return result;
        }

        /// <summary>
        /// Checks whether the given TLD is in the List <see cref="TLDs"/>.
        /// </summary>
        /// <param name="tld">The given TLD.</param>
        /// <returns><c>true</c> if listed, otherwise <c>false</c>.</returns>
        public static bool StaticCheckTLD(string tld)
        {
            return TLDs.Contains(tld.Trim().ToUpper());
        }

        /// <summary>
        /// Checks whether the given TLD is in the List <see cref="TLDs"/>.
        /// </summary>
        /// <param name="tld">The given TLD.</param>
        /// <param name="timeSpan">Timespan to wait for Reload data.</param>
        /// <returns><c>true</c> if listed, otherwise <c>false</c>.</returns>
        public static bool StaticCheckTLD(string tld, TimeSpan timeSpan)
        {
            if (timeSpan != default)
            {
                ReloadData(timeSpan);
            }

            return TLDs.Contains(tld.Trim().ToUpper());
        }
    }
}
