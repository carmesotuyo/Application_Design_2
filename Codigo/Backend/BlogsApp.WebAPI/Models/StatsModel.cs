using System;
using System.Collections;

namespace BlogsApp.WebAPI.Models
{
	public class StatsModel
	{
		public KeyValuePair<string, int> Jan { get; set; }
        public KeyValuePair<string, int> Feb { get; set; }
        public KeyValuePair<string, int> Mar { get; set; }
        public KeyValuePair<string, int> Apr { get; set; }
        public KeyValuePair<string, int> May { get; set; }
        public KeyValuePair<string, int> Jun { get; set; }
        public KeyValuePair<string, int> Jul { get; set; }
        public KeyValuePair<string, int> Aug { get; set; }
        public KeyValuePair<string, int> Sep { get; set; }
        public KeyValuePair<string, int> Oct { get; set; }
        public KeyValuePair<string, int> Nov { get; set; }
        public KeyValuePair<string, int> Dec { get; set; }

        public StatsModel()
		{
		}
    }
}

