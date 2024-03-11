using adonet.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adonet.Models
{
    public class ManagerModel
    {
        public string Surname {  get; set; }
        public string Name { get; set; }
        public string Secname {  get; set; }
        public IdName MainDep {  get; set; }
        public IdName? SecDep {  get; set; }
        public IdName? Chief {  get; set; }
        public List<IdName> Departments { get; set; }
        public List<IdName> Chiefs { get; set; }
        public ManagerModel (Manager entity)
        {
            Surname = entity.Surname;
            Name = entity.Name;
            Secname = entity.Secname;
            MainDep = entity.IdMainDep == default ? null! : new IdName 
            {
                Id = entity.MainDepartment.Id, 
                Name = entity.MainDepartment.Name 
            };
            SecDep=entity.SecondaryDepartment==null ? null
                : new IdName {
                Id = entity.SecondaryDepartment.Id,
                Name = entity.SecondaryDepartment.Name
            };
            Chief = entity.Chief == null ? null : new IdName
            {
                Id = entity.Chief.Id,
                Name = $"{entity.Chief.Surname} {entity.Chief.Name[0]}. {entity.Chief.Secname[0]}."
            };
        }
    }
}
