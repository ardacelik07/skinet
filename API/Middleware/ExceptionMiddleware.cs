using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        public RequestDelegate Nexts { get; }
        private readonly ILogger<ExceptionMiddleware> loggers;
        private readonly IHostEnvironment envs;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,IHostEnvironment env)
        {
          envs = env;
           loggers = logger;
            Nexts = next;

        }

        public async Task InvokeAsync(HttpContext context){

          try{

                      await Nexts(context);
          }
          catch(Exception ex) {

                 
             loggers.LogError(ex, ex.Message);
             context.Response.ContentType ="application/json";
             context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;



            var response = envs.IsDevelopment()
             ? new ApiException((int)HttpStatusCode.InternalServerError,ex.Message, ex.StackTrace.ToString())
             : new ApiException((int)HttpStatusCode.InternalServerError);

             var options = new JsonSerializerOptions{PropertyNamingPolicy= JsonNamingPolicy.CamelCase};

             var json =JsonSerializer.Serialize(response,options);
             await context.Response.WriteAsync(json);
          }




        }
    }
}