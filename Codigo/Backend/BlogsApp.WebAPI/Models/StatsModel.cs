﻿using System;
using System.Collections;

namespace BlogsApp.WebAPI.Models
{
	public class StatsModel
	{
		public int Jan { get; set; }
        public int Feb { get; set; }
        public int Mar { get; set; }
        public int Apr { get; set; }
        public int May { get; set; }
        public int Jun { get; set; }
        public int Jul { get; set; }
        public int Aug { get; set; }
        public int Sep { get; set; }
        public int Oct { get; set; }
        public int Nov { get; set; }
        public int Dec { get; set; }

        public IEnumerable<int> stats;

        public StatsModel()
		{
            this.stats = new List<int>() { Jan, Feb, Mar, Apr, May, Jun, Jul, Aug, Sep, Oct, Nov, Dec };
		}
    }
}

