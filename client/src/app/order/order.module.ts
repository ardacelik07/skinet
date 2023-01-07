import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BasketComponent } from './basket.component';
import { OrderRoutingModule } from './order-routing.module';
import { OrderDetailComponent } from './order-detail/order-detail.component';



@NgModule({
  declarations: [
    BasketComponent,
    OrderDetailComponent
  ],
  imports: [
    CommonModule,
    OrderRoutingModule
  ]
})
export class OrderModule { }
