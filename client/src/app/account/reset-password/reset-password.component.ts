import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { ResetPassword } from 'src/app/shared/models/ResetPassword';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {
  resetPasswordForm: FormGroup;
  showSuccess: boolean;
  showError: boolean;
  errorMessage: string;
  private token: string;
  private email: string;

  constructor(private accountService:AccountService,private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.resetPasswordForm = new FormGroup({
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl(''),
      

  });
  
  this.resetPasswordForm.get('confirm').setValidators([Validators.required]);
  
    this.token = this.route.snapshot.queryParams['token'];
    this.email = this.route.snapshot.queryParams['email'];
}
public validateControl = (controlName: string) => {
  return this.resetPasswordForm.get(controlName).invalid && this.resetPasswordForm.get(controlName).touched
}
public hasError = (controlName: string, errorName: string) => {
  return this.resetPasswordForm.get(controlName).hasError(errorName)
}
public resetPassword = (resetPasswordFormValue) => {
  this.showError = this.showSuccess = false;
  const resetPass = { ... resetPasswordFormValue };
  const resetPassDto: ResetPassword = {
    password: resetPass.password,
    confirmPassword: resetPass.confirm,
    token: this.token,
    email: this.email
   
  }
 
  this.accountService.ResetPassword(resetPassDto)
  .subscribe({
    
   
     
    next:(_) => this.showSuccess = true,
    
    
  error: (err: HttpErrorResponse) => {
    this.showError = true;
    this.errorMessage = err.message;
  }})
}

}






