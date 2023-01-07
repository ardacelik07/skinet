import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { EmailValidator, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { forgotPassword } from 'src/app/shared/models/forgotPassword';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup
  successMessage: string;
  errorMessage: string;
  showSuccess: boolean;
  showError: boolean;
  forgetPassword : forgotPassword
  constructor(private accountService: AccountService,private router:Router ) { }

  ngOnInit(): void {
    this.forgotPasswordForm = new FormGroup({
      email: new FormControl('', [Validators.required, Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]),
  })
}



public forgotPassword = (forgotPasswordFormValue) => {
  this.showError = this.showSuccess = false;
  const forgotPass = { ...forgotPasswordFormValue };
  const forgotPassDto: forgotPassword = {
    email: forgotPass.email,
    clientURI: 'http://localhost:4200/authentication/resetpassword'
  }
  this.accountService.ForgotPassword(forgotPassDto)
  .subscribe({
    next: (_) => {
    this.showSuccess = true;
    this.successMessage = 'The link has been sent, please check your email to reset your password.'
  },
  error: (err: HttpErrorResponse) => {
    this.showError = true;
    this.errorMessage = err.message;
  }})
}










}