using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum ESectionType
    {
        [Description("Vitrine")]
        ShopWindow,
        [Description("Prateleira")]
        Shelf,
        [Description("Frios")]
        Cold
    }
}
