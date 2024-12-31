using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Product : BaseModel
    {
        [Required(ErrorMessage = "É necessário enviar o nome")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Por favor informe o status do produto")]
        public int Status { get; set; }

        [Required(ErrorMessage = "Por favor informe a quantidade")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Por favor informe a seção")]
        public int Section { get; set; }

        [Required(ErrorMessage = "Por favor informe o preço")]
        public decimal Price{ get; set; }
    }
}
