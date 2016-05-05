using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _470_Final_AGAIN.Models
{
    class Recipe
    {
        public String ID { get; set; }
        public String rating { get; set; }
        public int[] flavors { get; set; }
        public String totalTimeInSeconds { get; set; }
    }
}
