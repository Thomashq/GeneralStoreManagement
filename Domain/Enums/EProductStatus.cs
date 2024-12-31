using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum EProductStatus
    {
        [Description("Disponível")]
        Available,
        [Description("Não disponível")]
        Unavailabe,
        [Description("Fora de estoque")]
        OutOfStorage
    }
}
