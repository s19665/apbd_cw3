using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstApi.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            httpContext.Request.EnableBuffering();
            var bodyStream = string.Empty;
            var path = @"Middlewares\requestsLog.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Loggs:");
                }
            }
            using (var reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8, true, 1024, true))
            using (StreamWriter sw = File.AppendText(path))
            {
                bodyStream = await reader.ReadToEndAsync();
                sw.Write(httpContext.Request.Method);
                sw.Write(" ");
                sw.Write(httpContext.Request.Path);
                sw.Write(" ");
                sw.Write(bodyStream);
                sw.Write(" ");
                sw.WriteLine(httpContext.Request.QueryString);
                httpContext.Request.Body.Seek(0, SeekOrigin.Begin);
            }
            await _next(httpContext);
        }
    }
}
