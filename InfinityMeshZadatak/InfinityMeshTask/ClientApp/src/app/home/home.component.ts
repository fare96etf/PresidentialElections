import { Component, Inject, Input } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Izbori } from "./izbori";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})

export class HomeComponent {
  public izbori: Izbori;
  @Input() overide: boolean = false;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  public noviUpload(files: FileList){
    console.log(files);
    if(files && files.length > 0) {
       let file : File = files.item(0); 
         console.log(file.name);
         console.log(file.size);
         console.log(file.type);
         let reader: FileReader = new FileReader();
         reader.readAsText(file);
         reader.onload = (e) => {
            let csv: string = reader.result as string;
            let redovi: any = csv.split(/\r\n|\r|\n/);
            this.filterIzbore(redovi);
         }
      }
  }

  public filterIzbore(redovi) {
    this.http.post<any>(this.baseUrl + 'api/', {redovi: redovi, overide: this.overide}).subscribe(result => {
      this.izbori = result;
      console.log(this.izbori);
    }, error => console.error(error));
  }

}
