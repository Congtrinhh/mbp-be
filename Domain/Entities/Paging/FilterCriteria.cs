using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Paging
{
    public class FilterCriteria
    {
        public string Field { get; set; }
        public string Operator { get; set; }
        public object Value { get; set; }
        public List<FilterCriteria> NestedFilters { get; set; }
        public string LogicalOperator { get; set; }
    }
}
