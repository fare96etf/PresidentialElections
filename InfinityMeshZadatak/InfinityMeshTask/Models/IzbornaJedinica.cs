using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityMeshTask.Models
{
    public class IzbornaJedinica
    {
        public string Naziv { get; set; }
        public List<Kandidat> Kandidati { get; set; }
    }
}
