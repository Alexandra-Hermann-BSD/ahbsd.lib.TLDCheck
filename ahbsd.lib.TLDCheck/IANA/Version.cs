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
    /// Struct, that holds the given version.
    /// </summary>
    public struct Version : IEquatable<IVersion>, IVersion
    {
        /// <summary>
        /// Constructor with the version number.
        /// </summary>
        /// <param name="versionNr">The version number.</param>
        public Version(string versionNr)
        {
            string datepart = versionNr.Substring(0, 8);
            string nr = versionNr.Substring(8);

            int year = int.Parse(datepart.Substring(0, 4));
            int month = int.Parse(datepart.Substring(4, 2));
            int day = int.Parse(datepart.Substring(6));

            VersionNr = ulong.Parse(versionNr);
            Nr = byte.Parse(nr);
            Date = new DateTime(year, month, day).Date;
        }

        /// <summary>
        /// The version number.
        /// </summary>
        public readonly ulong VersionNr;

        #region implementation of IVersion
        /// <summary>
        /// Gets the date of the version.
        /// </summary>
        /// <value>The date of the version.</value>
        public DateTime Date { get; private set; }

        /// <summary>
        /// Gets the number of the dated version.
        /// </summary>
        /// <value>The number of the dated version.</value>
        public byte Nr { get; private set; }

        /// <summary>
        /// Determines whether the given other object eaquals this version.
        /// </summary>
        /// <param name="obj">The given other object.</param>
        /// <returns>
        /// <c>true</c> if the given other object eaquals this version,
        /// otherwise <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is IVersion version && Equals(version);
        }

        /// <summary>
        /// Determines whether the given other Version eaquals this version.
        /// </summary>
        /// <param name="other">The given other Version.</param>
        /// <returns>
        /// <c>true</c> if the given other Version eaquals this version,
        /// otherwise <c>false</c>.
        /// </returns>
        public bool Equals(IVersion other)
        {
            return Date == other.Date && Nr == other.Nr;
        }

        /// <summary>
        /// Gets the HashCode of this Version.
        /// </summary>
        /// <returns>The HashCode of this Version.</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(VersionNr);
        }

        /// <summary>
        /// Gets the version as string.
        /// </summary>
        /// <returns>The version as string.</returns>
        public override string ToString()
            => $"Version from {Date:yyyy-MM-dd}, #{Nr}";
        #endregion
    }
}
