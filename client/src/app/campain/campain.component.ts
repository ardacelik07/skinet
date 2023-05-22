import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Campain } from '../shared/models/Campain';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-campain',
  templateUrl: './campain.component.html',
  styleUrls: ['./campain.component.scss']
})
export class CampainComponent implements OnInit {


    baseUrl = environment.apiUrl;
    kampanyalar :Campain[] = [];
  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getCampains();
  }

  getCampains(){
    return this.http.get<Campain[]>(this.baseUrl + 'Campain/kampanyalar' ).subscribe(response =>{
      this.kampanyalar = response;
      console.log(this.kampanyalar);
    });

  }
}
