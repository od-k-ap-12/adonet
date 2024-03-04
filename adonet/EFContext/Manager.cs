using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adonet.EFContext
{
    public class Manager
    {
        public Guid Id { get; set; }
        public String Surname { get; set; } = null!;
        public String Name { get; set; } = null!;
        public String Secname { get; set; } = null!;
        public Guid IdMainDep { get; set; }
        public Guid? IdSecDep { get; set; }
        public Guid? IdChief { get; set; }
        public DateTime? DeleteDt { get; set; }
        public int? Age { get; set; }
        public Department? MainDepartment { get; set; }
        public Department? SecondaryDepartment { get; set; }
        public Manager? Chief {  get; set; }
        public IEnumerable<Manager>? Subordinates { get; set; }
        public IEnumerable<Sale> Sales { get; set; }
    }
}
