using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class AccountDTO
    {
        public long Id { get; set; } // Presume-se que haja um ID na classe BaseModel

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string apiKey { get; set; }

        public string apiSecret { get; set; }
    }
}
