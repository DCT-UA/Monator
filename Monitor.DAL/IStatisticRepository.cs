using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCT.Monitor.Entities;
using Monitor.DAL;

namespace Monitor.DAL
{
    public interface IRequestRepository: IBaseRepository<PageRequest, Guid>
    {
    }
}
