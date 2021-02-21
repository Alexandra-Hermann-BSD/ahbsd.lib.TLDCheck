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
using ahbsd.lib.TLDCheck.IANA;
using RestSharp;

namespace ahbsd.lib.TLDCheck.Statistic
{
    /// <summary>
    /// Interface for statistics of TLD (re)loads.
    /// </summary>
    public interface IDownloadStatistic : IComponent
    {
        /// <summary>
        /// Gets the last resonse status.
        /// </summary>
        /// <value>The last resonse status.</value>
        ResponseStatus LastResponseStatus { get; }
        /// <summary>
        /// Gets the last Headline.
        /// </summary>
        /// <value>The last Headline.</value>
        IHeadline LastHeadLine { get; }
        /// <summary>
        /// Gets the amount of total (re)loads.
        /// </summary>
        /// <value>The amount of total (re)loads.</value>
        ulong Reloads { get; }
        /// <summary>
        /// Gets the <see cref="TimeSpan"/> from the last (re)load.
        /// </summary>
        /// <value>The <see cref="TimeSpan"/> from the last (re)load.</value>
        TimeSpan LastReloadTime { get; }
        /// <summary>
        /// Gets the statistic as string.
        /// </summary>
        /// <returns>The statistic as string.</returns>
        string ToString();
    }
}
