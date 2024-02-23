using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VendaLanches.Models;

[Table("Categorias")]
public class Categoria
{
    [Key]
    public int CategoriaId { get; set; }

    [Required(ErrorMessage = "Informe o nome da categoria")]
    [Display(Name = "Nome")]
    [StringLength(100, ErrorMessage = "O tamanho máximo é 100 caracteres")]
    public string CategoriaNome { get; set; }

    [Required(ErrorMessage = "Informe a descrição da categoria")]
    [Display(Name = "Descrição")]
    [StringLength(200, ErrorMessage = "O tamanho máximo é 200 caracteres")]
    public string Descricao { get; set; }

    public List<Lanche> Lanches { get; set; }
}
