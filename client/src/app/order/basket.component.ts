import { Component, OnInit } from '@angular/core';
import { IOrder } from '../shared/models/order';
import { OrderService } from './order.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.scss']
})
export class BasketComponent implements OnInit {
 orders : IOrder[];
  constructor(private ordersService:OrderService) { }

  ngOnInit(): void {
    this.getOrders();
  }
  getOrders(){
    this.ordersService.getOrdersForUser().subscribe((orders:IOrder[])=>{

      this.orders = orders;
    },error =>{
      console.log(error);
    });
  }

}
