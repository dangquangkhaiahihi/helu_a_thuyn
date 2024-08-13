using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSMS.Entity.CSMS_Entity;
using CSMS.Model.DTO.FilterRequest;

namespace CSMSBE.Infrastructure.Interfaces
{
    public interface ISomeTableRepository /*: IRepository<SomeTable, int>*/
    {
        SomeTable Create(SomeTable table);
        IQueryable<SomeTable> GetAll();
        SomeTable Get(int id);
        IQueryable<SomeTable> GetAll(SomeTableFilterRequest filter);
        SomeTable Update(SomeTable table);
        bool Remove(int id);
    }
}
