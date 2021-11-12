using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using week08right.Abstraction;

namespace week08right.Entities
{
    public class CarFactory
    {
        public Toy CreateNew()
        {
            return new Car();
        }
    }
}
