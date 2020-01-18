using System;
using System.Collections.Generic;
using System.Text;

namespace Mijabr.Language
{
    public interface IItemLister
    {
        string ToString(IEnumerable<string> list);
    }
}
