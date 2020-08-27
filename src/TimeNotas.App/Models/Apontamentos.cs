using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TimeNotas.App.Models
{
    public class Apontamentos
    {
        private readonly IEnumerable<Apontamento> _apontamentos;

        public Apontamentos(IEnumerable<Apontamento> apontamentos)
        {
            _apontamentos = apontamentos;
            DatasHoras = new Dictionary<string, IEnumerable<Apontamento>>();

            foreach (var item in _apontamentos.GroupBy(a => $"{a.DataHora.DayOfWeek} {a.DataHora.Date.ToString("dd/MM/yyyy")}"))
                DatasHoras.Add(item.Key,  item.OrderBy(a => a.DataHora));

        }

        [Display(Name = "Data/Hora apontamentos")]
        public IDictionary<string, IEnumerable<Apontamento>> DatasHoras { get; private set; }
    }
}
