using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Marketplace.Interview.Business.Basket;
using Marketplace.Interview.Business.Core;

namespace Marketplace.Interview.Business.Core
{
    public class UnitOfWork<T>: IUnitOfWork<T>
    {
        private static readonly string file = Path.Combine(Environment.GetEnvironmentVariable("temp"), "basket.xml");

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T GetAll()
        {
            using (var sr = new StreamReader(file))
            {
                return SerializationHelper.DataContractDeserialize<T>(sr.ReadToEnd());
            }
        }

        public void Commit(T t)
        {
            using (var sw = new StreamWriter(file, false))
            {
                sw.Write(SerializationHelper.DataContractSerialize(t));
            }
        }
        
    }
}
