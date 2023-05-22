import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { HomeComponent } from './home/home.component';
import { ProductDetailsComponent } from './shop/product-details/product-details.component';
import { ShopComponent } from './shop/shop.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { AuthGuard } from './core/guards/auth.guard';
import { CampainComponent } from './campain/campain.component';
import { CommentsComponent } from './comments/comments.component';

const routes: Routes = [
  
  {path: '',component: HomeComponent,data: {breadcrumb: 'Ana Sayfa'}},
  {path: 'test-error',component: TestErrorComponent, data: {breadcrumb: 'test Errors'}},
  {path: 'Campains',component: CampainComponent, data: {breadcrumb: 'Kampanyalar'}},
  {path: 'yorumlar',component: CommentsComponent, data: {breadcrumb: 'yorumlar'}},
  {path: 'server-error',component: ServerErrorComponent, data: {breadcrumb: 'Server Errors'}},
  {path: 'not-found',component: NotFoundComponent, data: {breadcrumb: 'Not-found Errors'}},
  {path:'shop',loadChildren: () => import('./shop/shop.module').then(mod => mod.ShopModule), data: {breadcrumb: 'Market'}},
  {path:'basket',loadChildren: () => import('./basket/basket.module').then(mod => mod.BasketModule), data: {breadcrumb: 'Sepet'}},
  {path:'checkout', canActivate:[AuthGuard], loadChildren: () => import('./checkout/checkout.module').then(mod => mod.CheckoutModule), data: {breadcrumb: 'Ödeme'}},
  {path:'order', canActivate:[AuthGuard], loadChildren: () => import('./order/order.module').then(mod => mod.OrderModule), data: {breadcrumb: 'Order'}},
  {path:'account',loadChildren: () => import('./account/account.module').then(mod => mod.AccountModule), data: {breadcrumb: {skip:true}}},
  {path:'**',redirectTo:'not-found',pathMatch:'full'},
 
 
  

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
