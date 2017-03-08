using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Interview.Business.Core
{
    interface IUnitOfWork<T> : IDisposable
    {
        T GetAll();
        void Commit(T t);
    }
}
