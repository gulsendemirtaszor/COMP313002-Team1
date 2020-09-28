using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models.ViewModels
{
    public class ConditionsListViewModel
    {
        public Condition condition { get; set; }

        public IEnumerable<Condition> Conditions { get; set; }
    }
}
