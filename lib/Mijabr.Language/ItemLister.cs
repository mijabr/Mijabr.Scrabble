using System.Collections.Generic;
using System.Linq;

namespace Mijabr.Language
{
    public class ItemLister : IItemLister
    {
        public string ToString(IEnumerable<string> list)
        {
            if (list == null || list.Count() == 0)
            {
                return string.Empty;
            }

            if (list.Count() == 1)
            {
                return list.Single();
            }

            return $"{string.Join(", ", list.ToArray(), 0, list.Count() - 1)} and {list.Last()}";
        }
    }
}
