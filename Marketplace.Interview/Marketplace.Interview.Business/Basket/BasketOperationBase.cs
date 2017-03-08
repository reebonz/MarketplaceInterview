using System;
using System.Collections.Generic;
using System.IO;
using Marketplace.Interview.Business.Core;

namespace Marketplace.Interview.Business.Basket
{
    public abstract class BasketOperationBase: UnitOfWork<Basket>
    {
        protected Basket GetBasket()
        {
            return GetAll();
        }

        protected void SaveBasket(Basket basket)
        {
            Commit(basket);
        }
    }
}