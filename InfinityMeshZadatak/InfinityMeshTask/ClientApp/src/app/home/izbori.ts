export interface Izbori {
  izborneJedinice: IzbornaJedinica[];
}

interface IzbornaJedinica {
    naziv: string;
    kandidati: Kandidat[];
  }

  // interface Kandidat {
  //   naziv: string;
  //   glasovi: string;
  //   procenat: string;
  //   greska: string;
  // }
    
  interface Kandidat {
    naziv: string;
    brojGlasova: string;
    udio: string;
    greska: boolean;
  }