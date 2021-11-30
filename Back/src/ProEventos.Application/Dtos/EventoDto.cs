using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string DataEvento { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [MinLength(3, ErrorMessage = "{0} deve ter no mínimo 3 caracteres!")]
        [MaxLength(50, ErrorMessage = "{0} deve ter no máximo 50 caracteres!")]
        public string Tema { get; set; }
        
        [Display(Name = "Qtd pessoas")]
        [Range(1, 120000, ErrorMessage = "A quantidade de pessoas deve estar entre 1 e 120.000")]
        public int QtdPessoas { get; set; }
        
        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", ErrorMessage = "Não é uma imagem válida. (gif, jpg, jpeg, bmp, png)")]
        public string ImagemURL { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Phone(ErrorMessage = "O campo {0} deve ser um telefone válido!")]
        public string Telefone { get; set; }
        
        [Required(ErrorMessage = "O campo {0} é obrigatório!")]
        [Display(Name = "e-mail")]
        [EmailAddress(ErrorMessage = "O campo {0} precisa ser um e-mail válido!")]
        public string Email { get; set; }
        
        public IEnumerable<LoteDto> Lotes { get; set; }
        public IEnumerable<RedeSocialDto> RedesSociais { get; set; }
        public IEnumerable<PalestranteDto> Palestrantes { get; set; }
    }
}