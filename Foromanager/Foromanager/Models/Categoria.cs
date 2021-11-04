using System.Collections.Generic;

namespace Foromanager.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public int ForoId { get; set; }
        public string CategoriaNombre { get; set; }
        public ICollection<Foro> Foros { get; set; }

    }
}
