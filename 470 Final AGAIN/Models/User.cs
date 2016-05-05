using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _470_Final_AGAIN.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public virtual TrainingData TrainingSet { get; set; }
    }
}