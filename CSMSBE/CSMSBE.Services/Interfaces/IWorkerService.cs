using CSMS.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Services.Interfaces
{
    public interface IWorkerService
    {
        CurrentUserDTO GetCurrentUser();
    }
}
