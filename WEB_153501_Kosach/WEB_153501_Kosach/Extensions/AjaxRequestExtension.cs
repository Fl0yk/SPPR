using Azure.Core;

namespace WEB_153501_Kosach.Extensions
{
    public static class AjaxRequestExtension
    {
        public static bool IsAjaxRequest(this HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
