using InfinityMeshTask.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace InfinityMeshTask.Controllers
{
    [ApiController]
    [Route("api/")]
    public class IzboriController : ControllerBase
    {
        private List<Tuple<string, string>> kandidatiSifre = new List<Tuple<string, string>>
        {
            new Tuple<string, string>("DT", "Donald Trump"),
            new Tuple<string, string>("HC", "Hillary Clinton"),
            new Tuple<string, string>("JB", "Joe Biden"),
            new Tuple<string, string>("JFK", "John F. Kennedy"),
            new Tuple<string, string>("JR", "Jack Randall")
        };

        private List<Tuple<string, int>> overide = new List<Tuple<string, int>>
        {
            new Tuple<string, int>("John F. Kennedy", 3333)
        };

        private readonly ILogger<IzboriController> _logger;

        public IzboriController(ILogger<IzboriController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<IzbornaJedinica> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new IzbornaJedinica
            {
                
            })
            .ToArray();
        }

        [HttpPost]
        public IEnumerable<IzbornaJedinica> Post([FromBody] Tekst tekst)
        {
            if (System.IO.File.Exists("Examples/error_log"))
                System.IO.File.Delete("Examples/error_log");
            
            var result = new List<IzbornaJedinica>();
            foreach (var red in tekst.redovi)
            {
                var elementi = red.Split(", ").ToList();
                var nazivJedinice = elementi[0];
                elementi.Remove(nazivJedinice);

                var kandidati = new List<Kandidat>();
                var ukupnoGlasova = 0;

                for (int i = 0; i < elementi.Count/2; i++)
                {
                    var kandidat = new Kandidat();

                    try
                    {
                        kandidat.Naziv = kandidatiSifre.Where(x => x.Item1 == elementi[i * 2 + 1]).FirstOrDefault()?.Item2;

                        if (tekst.overide && kandidat.Naziv == "John F. Kennedy")
                        {
                            kandidat.BrojGlasova = overide[0].Item2.ToString();
                            ukupnoGlasova += overide[0].Item2;
                        }
                        else
                        {
                            int br;
                            if (!Int32.TryParse(elementi[i * 2], out br))
                            {
                                br = -1;
                                kandidat.Greska = true;
                                StreamWriter sw = new StreamWriter("Examples/error_log", append: true);
                                sw.WriteLine("Pogrešan format broja glasova za kandidata: " + kandidat.Naziv + " u državi " + nazivJedinice);
                                sw.Close();
                            }

                            kandidat.BrojGlasova = br == -1 ? "N/A" : br.ToString();
                            if (kandidat.BrojGlasova != "N/A")
                                ukupnoGlasova += br;
                        }
                        
                    }
                    catch(Exception e)
                    {
                        kandidat.Greska = true;
                    }

                    kandidati.Add(kandidat);
                }

                if (tekst.overide && !kandidati.Any(k => k.Naziv == "John F. Kennedy"))
                {
                    kandidati.Add(new Kandidat
                    {
                        Naziv = overide[0].Item1,
                        BrojGlasova = overide[0].Item2.ToString()
                    });
                }

                foreach (var k in kandidati)
                {
                    if (k.BrojGlasova != "N/A")
                        k.Udio = (((Decimal)(Int32.Parse(k.BrojGlasova)) / ukupnoGlasova) * 100).ToString("#.##");
                    else
                        k.Udio = "N/A";
                }


                var izbornaJedinica = new IzbornaJedinica
                {
                    Naziv = nazivJedinice,
                    Kandidati = kandidati
                };

                result.Add(izbornaJedinica);
            }


            return result;
        }

        public class Tekst
        {
            public List<string> redovi { get; set; }
            public bool overide { get; set; }
        }

    }
}
