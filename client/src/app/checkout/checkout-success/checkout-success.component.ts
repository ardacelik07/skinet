import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { comments } from 'src/app/shared/models/comment';
import { IOrder } from 'src/app/shared/models/order';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-checkout-success',
  templateUrl: './checkout-success.component.html',
  styleUrls: ['./checkout-success.component.scss']
})
export class CheckoutSuccessComponent implements OnInit {
order: IOrder;
comments:string;
commentform:FormGroup;
 model:any={};
baseUrl = environment.apiUrl;
  constructor(private router: Router,private http:HttpClient) { 
    const navigation = this.router.getCurrentNavigation();
    const state = navigation && navigation.extras && navigation.extras.state;
    if(state){
      this.order = state as IOrder;
    }
  }

  ngOnInit(): void {
  }

 
  addcomment(model : any){
    
      return this.http.post(this.baseUrl + 'Comment/addcomment',model).subscribe(() =>{
          this.router.navigateByUrl("yorumlar");
      });
     
  }

}
