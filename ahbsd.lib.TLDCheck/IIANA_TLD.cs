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
using ahbsd.lib.TLDCheck.IANA;

namespace ahbsd.lib.TLDCheck
{
    /// <summary>
    /// Interface for checking if TLDs are known by IANA.
    /// </summary>
    public interface IIANA_TLD
    {
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
        bool CheckTLD(string tld);
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
        bool CheckTLD(string tld, TimeSpan timeSpan);
        /// <summary>
        /// Checks the TLD in the given <see cref="Uri"/>.
        /// </summary>
        /// <param name="uri">The given <see cref="Uri"/>.</param>
        /// <returns>
        /// <c>true</c> if the TLD is known by IANA, otherwise <c>false</c>.
        /// </returns>
        bool CheckTLD(Uri uri);
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
        bool CheckTLD(Uri uri, TimeSpan timeSpan);
        /// <summary>
        /// Gets the last headline.
        /// </summary>
        /// <value>The last headline.</value>
        IHeadline LastHaedline { get; }
    }
}