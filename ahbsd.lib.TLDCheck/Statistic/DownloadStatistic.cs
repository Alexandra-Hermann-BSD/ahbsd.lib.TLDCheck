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
using System.ComponentModel;
using System.Text;
using ahbsd.lib.TLDCheck.IANA;
using RestSharp;

namespace ahbsd.lib.TLDCheck.Statistic
{
    /// <summary>
    /// Class with statistics for the IANA TLDs (re)loads.
    /// </summary>
    public class DownloadStatistic : Component, IDownloadStatistic
    {
        /// <summary>
        /// The <see cref="IIANA_TLD"/> the statistics are for.
        /// </summary>
        private readonly IIANA_TLD iANA_TLD;

        /// <summary>
        /// Constructor with a <see cref="IIANA_TLD"/> to get the statistic for.
        /// </summary>
        /// <param name="tLD">
        /// The <see cref="IIANA_TLD"/> to get the statistic for.
        /// </param>
        public DownloadStatistic(IIANA_TLD tLD)
            : base()
        {
            iANA_TLD = tLD;
        }

        /// <summary>
        /// Constructor with a <see cref="IIANA_TLD"/> to get the statistic for
        /// and a container.
        /// </summary>
        /// <param name="tLD">
        /// The <see cref="IIANA_TLD"/> to get the statistic for.
        /// </param>
        /// <param name="container">The container.∫</param>
        public DownloadStatistic(IIANA_TLD tLD, IContainer container)
            : base()
        {
            iANA_TLD = tLD;

            if (container != null)
            {
                container.Add(this, $"Statistic {LastHeadLine.Version}");
            }
        }

        #region implementation of IDownloadStatistic
        /// <summary>
        /// Gets the last resonse status.
        /// </summary>
        /// <value>The last resonse status.</value>
        public ResponseStatus LastResponseStatus => IANA_TLD.LastResponseStatus;
        /// <summary>
        /// Gets the last Headline.
        /// </summary>∫
        /// <value>The last Headline.</value>
        public IHeadline LastHeadLine => iANA_TLD.LastHaedline;
        /// <summary>
        /// Gets the amount of total (re)loads.
        /// </summary>
        /// <value>The amount of total (re)loads.</value>
        public ulong Reloads => IANA_TLD.Reloads;
        /// <summary>
        /// Gets the <see cref="TimeSpan"/> from the last (re)load.
        /// </summary>
        /// <value>The <see cref="TimeSpan"/> from the last (re)load.</value>
        public TimeSpan LastReloadTime => IANA_TLD.LastReloadTime;
        /// <summary>
        /// Gets the statistic as string.
        /// </summary>
        /// <returns>The statistic as string.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();

            result.AppendFormat("Last response status: '{0}'", LastResponseStatus);
            result.AppendLine();

            if (LastResponseStatus.Equals(ResponseStatus.Completed))
            {
                result.AppendFormat("Last HeadLine: '{0}'", LastHeadLine);
                result.AppendLine();
                result.AppendFormat(
                    "Last (re)load took {0} h, {1} m, {2} s, {3} ms.",
                    LastReloadTime.Hours,
                    LastReloadTime.Minutes,
                    LastReloadTime.Seconds,
                    LastReloadTime.Milliseconds);
                result.AppendLine();
            }

            if (Reloads == 1)
            {
                result.AppendLine("Total 1 load happened.");
            }
            else if (Reloads == 0)
            {
                result.AppendLine("No loads happened.");
            }
            else
            {
                result.AppendLine($"Total 1 load and {Reloads - 1} reloads happened.");
            }

            return result.ToString();
        }
        #endregion
    }
}
