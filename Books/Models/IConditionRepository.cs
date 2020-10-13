using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public interface IConditionRepository
    {
        IQueryable<Condition> Conditions { get; }

        void SaveCondition(Condition condition);

        Condition DeleteCondition(int conditionID);
    }
}
