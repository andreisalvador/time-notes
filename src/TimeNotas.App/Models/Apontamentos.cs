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
            DatasHoras = new Dictionary<DateTime, IEnumerable<Apontamento>>();

            foreach (var item in _apontamentos.GroupBy(a => a.DataHora.Date))
                DatasHoras.Add(item.Key,  item.OrderBy(a => a.DataHora));

        }

        [Display(Name = "Horas extras")]
        public string HorasExtra { get; private set; }

        [Display(Name = "Horas faltantes")]
        public string HorasFaltantes { get; private set; }

        [Display(Name = "Data/Hora apontamentos")]
        public IDictionary<DateTime, IEnumerable<Apontamento>> DatasHoras { get; private set; }
    }
}
