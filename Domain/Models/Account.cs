using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Account : BaseModel
    {
        [Required(ErrorMessage = "É necessário enviar o nome de usuário")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "É necessário enviar a senha")]
        public string Password { get; set; }

        [Required(ErrorMessage = "É necessário enviar o e-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "É necessário enviar o número de telefone")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "É necessário registrar a chave da binance")]
        public string ApiKey{ get; set; }

        [Required(ErrorMessage = "É necessário enviar a SecretKey")]
        public string SecretKey { get; set; }

        public string salt { get; set; }

        public string EncryptionKey { get; set; }

        public string EncryptionIV { get; set; }

    }
}
