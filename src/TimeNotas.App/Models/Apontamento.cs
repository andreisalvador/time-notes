using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeNotas.App.Models
{
    public class Apontamento
    {
        public Apontamento()
        {
            Id = Guid.NewGuid();
            DataHora = DateTime.Now;
            DataHora = DataHora.AddMilliseconds(-DataHora.Millisecond);
            DataHora = DataHora.AddSeconds(-DataHora.Second);
        }

        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data/Hora apontamento")]
        public DateTime DataHora { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public override string ToString()
            => DataHora.ToShortTimeString();
    }
}
