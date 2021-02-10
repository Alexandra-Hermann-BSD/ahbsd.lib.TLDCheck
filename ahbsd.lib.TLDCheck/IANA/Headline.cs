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
using System.Linq;
using System.Text;

namespace ahbsd.lib.TLDCheck.IANA
{
    /// <summary>
    /// Class that splits the headline and holds the data.
    /// </summary>
    /// <remarks>
    /// e.g. at time of writing this class:
    /// # Version 2021020500, Last Updated Fri Feb  5 07:07:01 2021 UTC
    /// Part    0          1     0       1   2   3  4        5    6   7
    /// </remarks>
    public class Headline : Component, IHeadline
    {
        /// <summary>
        /// The input data.
        /// </summary>
        public readonly string Input;

        /// <summary>
        /// Constructor with the given headline.
        /// </summary>
        /// <param name="input">The given headline.</param>
        public Headline(string input)
            : base()
        {
            Input = input.Substring(1).Trim(); // remove the beginning #
            Split();
        }

        public Headline(string input, IContainer container)
            : base()
        {
            Input = input.Substring(1).Trim(); // remove the beginning #
            Split();

            if (container != null)
            {
                container.Add(this, $"Headline {Version}");

                if (Site == null)
                {
                    Site = new HeadlineSite(this, container);
                }
            }
        }

        /// <summary>
        /// Splits the <see cref="Input"/> into needed data.
        /// </summary>
        protected void Split()
        {
            string[] parts = Input.Split(',');
            string[] versionParts = parts[0].Split(' ');
            string[] updateParts = parts[1].Split(' ',
                StringSplitOptions.RemoveEmptyEntries);
            string[] timeParts = updateParts[5].Split(':');

            string month = updateParts[3];
            int day = int.Parse(updateParts[4]);
            int year = int.Parse(updateParts[6]);
            string timezone = updateParts[7];

            int hh = int.Parse(timeParts[0]);
            int mm = int.Parse(timeParts[1]);
            int ss = int.Parse(timeParts[2]);

            Version = new Version(versionParts.Last());

            LastUpdated = new DateTime(
                year,
                GetMonth(month),
                day,
                hh,
                mm,
                ss,
                0,
                DateTimeKind.Utc);
        }

        /// <summary>
        /// Gets the month nr from the string.
        /// </summary>
        /// <returns>The month nr</returns>
        private int GetMonth(string month)
        {
            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"};
            int pos = -2;

            for (int i = 0; i < months.Length; i++)
            {
                if (months[i].Equals(month))
                {
                    pos = i;
                    break;
                }
            }

            return pos + 1;
        }

        /// <summary>
        /// Gets the Headline as string.
        /// </summary>
        /// <returns>The Headline as string.</returns>
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat("{0} | Last Update: {1} UTC",
                Version,
                LastUpdated.ToString("s"));
            return result.ToString();
        }

        #region implementation of IHeadline
        /// <summary>
        /// Gets the version of the data.
        /// </summary>
        /// <value>The version of the data.</value>
        public IVersion Version { get; private set; }

        /// <summary>
        /// Gets the date of last update.
        /// </summary>
        /// <value>The date of last update.</value>
        public DateTime LastUpdated { get; private set; }
        /// <summary>
        /// This Component can not raise events.
        /// </summary>
        protected override bool CanRaiseEvents => false;
        #endregion

        private class HeadlineSite : ISite
        {
            public HeadlineSite(IHeadline headline, IContainer container)
            {
                Component = headline;
                Container = container;

                if (headline != null && container != null)
                {
                    Name = $"Headline {headline.Version}";
                }
            }

            public HeadlineSite(
                IHeadline headline, IContainer container, string name)
            {
                Component = headline;
                Container = container;
                Name = name;
            }
            public IComponent Component { get; private set; }

            public IContainer Container { get; private set; }

            public bool DesignMode { get; private set; }

            public string Name { get; set; }

            public object GetService(Type serviceType) => null;
        }
    }
}
