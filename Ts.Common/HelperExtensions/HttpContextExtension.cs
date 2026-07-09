using Microsoft.AspNetCore.Http;
using System.Text;
namespace Ts.Common.HelperExtensions
{
    public static class HttpContextExtension
    {
        public static string BuildExceptionHtmlMessage(this HttpContext context, Exception exception)
        {
            var msgBuilder = new StringBuilder();
            msgBuilder.AppendLine($"<h1>Message: {exception.Message}</h1>");

            msgBuilder.AppendLine(context.User.Identity.IsAuthenticated
                ? $"Source: {exception.Source} User: {context.User.Claims.GetUserId()}<hr />"
                : $"Source: {exception.Source}<hr />");

            msgBuilder.AppendLine($"Request Path: {context.Request.Path}<hr />");

            msgBuilder.AppendLine($"QueryString: {context.Request.QueryString}<hr />");

            if (exception.StackTrace != null)
                msgBuilder.AppendLine(
                    $"Stack Trace: {exception.StackTrace.Replace(Environment.NewLine, "<br />")}<hr />");

            if (exception.InnerException != null)
                msgBuilder.AppendLine($"Inner Exception:<hr />{exception.InnerException?.Message.Replace(Environment.NewLine, "<br />")}<hr />");

            if (!context.Request.HasFormContentType || context.Request.Form == null || context.Request.Form.Count == 0)
                return msgBuilder.ToString();

            msgBuilder.AppendLine("<table border=\"1\"><tr><td colspan=\"2\">Form collection:</td></tr>");
            foreach (var (key, value) in context.Request.Form)
                msgBuilder.AppendLine($"<tr><td>{key}</td><td>{value}</td></tr>");
            msgBuilder.AppendLine("</table>");

            return msgBuilder.ToString();
        }

        public static string BuildExceptionTextMessage(this HttpContext context, Exception exception)
        {
            var msgBuilder = new StringBuilder();
            msgBuilder.AppendLine($"Message: {exception.Message}\n");

            msgBuilder.AppendLine(context.User.Identity.IsAuthenticated
                ? $"Source: {exception.Source} User: {context.User.Claims.GetUserId()}\n"
                : $"Source: {exception.Source}\n");

            msgBuilder.AppendLine($"Request Path: {context.Request.Path}\n");

            msgBuilder.AppendLine($"QueryString: {context.Request.QueryString}\n");

            if (exception.StackTrace != null)
                msgBuilder.AppendLine(
                    $"Stack Trace:\n{exception.StackTrace}\n");

            if (exception.InnerException != null)
                msgBuilder.AppendLine($"Inner Exception:\n{exception.InnerException?.Message}\n");

            if (context.Request.HasFormContentType && context.Request.Form != null && context.Request.Form.Count != 0)
            {
                msgBuilder.AppendLine("Form collection:\n");
                foreach (var (key, value) in context.Request.Form)
                    msgBuilder.AppendLine($"{key}: {value}\n");
            }

            msgBuilder.AppendLine("--End Of Exception--\n");
            return msgBuilder.ToString();
        }
    }
}
