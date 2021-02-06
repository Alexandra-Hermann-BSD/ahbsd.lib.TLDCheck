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

namespace ahbsd.lib.TLDCheck.IANA
{
    /// <summary>
    /// An interface to show the version in details.
    /// </summary>
    public interface IVersion
    {
        /// <summary>
        /// Gets the date of the version.
        /// </summary>
        /// <value>The date of the version.</value>
        DateTime Date { get; }
        /// <summary>
        /// Gets the number of the dated version.
        /// </summary>
        /// <value>The number of the dated version.</value>
        byte Nr { get; }

        /// <summary>
        /// Determines whether the given other object eaquals this version.
        /// </summary>
        /// <param name="obj">The given other object.</param>
        /// <returns>
        /// <c>true</c> if the given other object eaquals this version,
        /// otherwise <c>false</c>.
        /// </returns>
        bool Equals(object obj);
        /// <summary>
        /// Determines whether the given other Version eaquals this version.
        /// </summary>
        /// <param name="other">The given other Version.</param>
        /// <returns>
        /// <c>true</c> if the given other Version eaquals this version,
        /// otherwise <c>false</c>.
        /// </returns>
        bool Equals(IVersion other);
        /// <summary>
        /// Gets the HashCode of this Version.
        /// </summary>
        /// <returns>The HashCode of this Version.</returns>
        int GetHashCode();
        /// <summary>
        /// Gets the version as string.
        /// </summary>
        /// <returns>The version as string.</returns>
        string ToString();
    }
}