using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DP.Patients.Controllers.Model
{
    public class Patient
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public DateTime TestDate { get; set; }

        public string Email { get; set; }

    }
}
