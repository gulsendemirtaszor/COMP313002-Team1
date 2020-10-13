using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Books.Models
{
    public class EFConditionRepository : IConditionRepository
    {
        private ApplicationDbContext _context;

        public EFConditionRepository(ApplicationDbContext ctx)
        {
            this._context = ctx;
        }

        public IQueryable<Condition> Conditions => this._context.Conditions;

        public void SaveCondition(Condition condition)
        {
            if (condition.Id == 0)
            {
                this._context.Conditions.Add(condition);
            }
            else
            {
                Condition _conditionEntry = this._context.Conditions
                    .FirstOrDefault(c => c.Id == condition.Id);

                if (_conditionEntry != null)
                {
                    _conditionEntry.UpdateFrom(condition);
                }
            }

            this._context.SaveChanges();
        }

        public Condition DeleteCondition(int conditionID)
        {
            Condition _conditionEntry = this._context.Conditions
                .FirstOrDefault(c => c.Id == conditionID);

            if (_conditionEntry != null)
            {
                this._context.Conditions.Remove(_conditionEntry);
                this._context.SaveChanges();
            }

            return _conditionEntry;
        }
    }
}
