import { Component, OnInit } from '@angular/core';
import { comments } from '../shared/models/comment';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit {
  baseUrl = environment.apiUrl;
  comment: comments[];
  constructor(private http:HttpClient) { }

  ngOnInit(): void {
    this.getComments();
  }

  getComments(){
    return this.http.get<comments[]>("https://localhost:5001/api/" + 'Comment/yorumlar').subscribe(response =>{
      this.comment = response;
      console.log(this.comment);
    });

  };
  }

