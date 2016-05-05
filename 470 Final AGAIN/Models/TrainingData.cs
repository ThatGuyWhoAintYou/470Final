using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace _470_Final_AGAIN.Models
{
    public class TrainingData
    {
        public String ID { get; set; }
        public String Salty { get; set; }
        public String Sour { get; set; }
        public String Sweet { get; set; }
        public String Bitter { get; set; }
        public String Meaty { get; set; }
        public String Piquant { get; set; }
        public String Rating { get; set; }
        public String PrepTime { get; set; }
        public String ShowUser { get; set; }
    }

    public class TrainingDBContext: DbContext
    {
      

        public DbSet<TrainingData> TrainingDatas { get; set; }

        public DbSet<User> Users { get; set; }
    }

}
