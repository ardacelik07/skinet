import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { IBrand } from '../shared/models/brand';
import { IProduct } from '../shared/models/product';
import { IType } from '../shared/models/productType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChild('search', {static:false}) searchTerm:ElementRef;
 products :IProduct[];
 brands:IBrand[];
 types: IType[];
shopParams = new ShopParams();
totalCount:number;
 sortOptions = [
  {name: 'alfabetik',value:'name'},
  {name:'Fiyat: küçükten büyüğe',value: 'priceAsc'},
  {name: 'Fiyat: Büyükten Küçüğe',value: 'priceDesc'}
];

  constructor(private shopService: ShopService) { }

  ngOnInit() {

  }

  ngAfterViewInit(): void {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  ngOnDestroy(): void {
    
  }
  
  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe(response =>{
      this.products =response.data;
      this.shopParams.pageNumber =response.pageIndex;
      this.shopParams.pagesize =response.pageSize;
      this.totalCount = response.count;

   }, error =>{
         console.log(error);
   });
  }

  getBrands(){
    this.shopService.getBrands().subscribe(response =>{

      this.brands = [{id: 0,name: 'hepsi'}, ...response];
    },error =>{
      console.log(error);
    }
    );
  }
  getTypes(){
    this.shopService.getTypes().subscribe(response =>{

      this.types =[{id: 0,name: 'hepsi'}, ...response]; 
    },error =>{
      console.log(error);
    }
    );
  }
  onBrandSelected(brandId: number){
    this.shopParams.brandId =brandId;
    this.shopParams.pageNumber =1;
    this.getProducts();
  }
  onTypeSelected(typeId: number){
    this.shopParams.typeId =typeId;
    this.shopParams.pageNumber =1;
    this.getProducts();
  }
  onSortSelected(sort: string){
    this.shopParams.sort = sort;
    this.getProducts();
  }
  onPageChanged(event :any){
    if(this.shopParams.pageNumber !==event){
      this.shopParams.pageNumber =event;
      this.getProducts();

    }
    
  }
  onSearch(){
    this.shopParams.search =this.searchTerm.nativeElement.value;
    this.shopParams.pageNumber =1;
    this.getProducts();
  }
  onReset(){
    this.searchTerm.nativeElement.value = '';
    this.shopParams = new ShopParams();
    this.getProducts();
  }

}
