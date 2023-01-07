import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BasketComponent } from './basket.component';
import { OrderDetailComponent } from './order-detail/order-detail.component';

const routes : Routes = [
  
  {path: '',component:BasketComponent},
  {path:':id',component:OrderDetailComponent, data: {breadcrumb:{alias:'OrderDetailed'}}}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports:[RouterModule]
})
export class OrderRoutingModule { }
