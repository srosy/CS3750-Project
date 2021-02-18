using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LMS.Pages
{
    public class HostModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccssor;

        public HostModel(IHttpContextAccessor httpContextAccssor)
        {
            _httpContextAccssor = httpContextAccssor;
        }

        public string UserAgent { get; set; }
        public string IPAddress { get; set; }

        public void OnGet()
        {
            UserAgent = _httpContextAccssor.HttpContext.Request.Headers["User-Agent"];
            IPAddress = _httpContextAccssor.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
