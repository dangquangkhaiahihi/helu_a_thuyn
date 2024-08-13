using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CSMSBE.Core.Helper
{
    public interface IWebHelper
    {
        string GetCurrentIpAddress();
        string GetLocalIpAddress();
    }

    /// <summary>
    /// Represents a web helper
    /// </summary>
    public partial class WebHelper : IWebHelper
    {
        #region Fields 
        private readonly ILogger<WebHelper> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Ctor

        public WebHelper(ILogger<WebHelper> logger,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Check whether current HTTP request is available
        /// </summary>
        /// <returns>True if available; otherwise false</returns>
        protected virtual bool IsRequestAvailable()
        {
            if (_httpContextAccessor?.HttpContext == null)
                return false;

            try
            {
                if (_httpContextAccessor.HttpContext.Request == null)
                    return false;
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Is IP address specified
        /// </summary>
        /// <param name="address">IP address</param>
        /// <returns>Result</returns>
        protected virtual bool IsIpAddressSet(IPAddress address)
        {
            return address != null && address.ToString() != IPAddress.IPv6Loopback.ToString();
        }

        public virtual string GetCurrentIpAddress()
        {
            if (!IsRequestAvailable())
                return string.Empty;

            var result = string.Empty;
            try
            {
                //first try to get IP address from the forwarded header
                if (_httpContextAccessor.HttpContext.Request.Headers != null)
                {
                    var forwardedHttpHeaderKey = "X-FORWARDED-FOR";
                    if (!string.IsNullOrEmpty(""))
                    {
                        //but in some cases server use other HTTP header
                        //in these cases an administrator can specify a custom Forwarded HTTP header (e.g. CF-Connecting-IP, X-FORWARDED-PROTO, etc)
                        forwardedHttpHeaderKey = "";
                    }

                    var forwardedHeader = _httpContextAccessor.HttpContext.Request.Headers[forwardedHttpHeaderKey];
                    if (!StringValues.IsNullOrEmpty(forwardedHeader))
                        result = forwardedHeader.FirstOrDefault();
                }

                //if this header not exists try get connection remote IP address
                if (string.IsNullOrEmpty(result) && _httpContextAccessor.HttpContext.Connection.RemoteIpAddress != null)
                    result = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            catch
            {
                return string.Empty;
            }

            //some of the validation
            if (result != null && result.Equals(IPAddress.IPv6Loopback.ToString(), StringComparison.InvariantCultureIgnoreCase))
                result = IPAddress.Loopback.ToString();

            //"TryParse" doesn't support IPv4 with port number
            if (IPAddress.TryParse(result ?? string.Empty, out var ip))
                //IP address is valid 
                result = ip.ToString();
            else if (!string.IsNullOrEmpty(result))
                //remove port
                result = result.Split(':').FirstOrDefault();

            return result;
        }

        public virtual string GetLocalIpAddress()
        {
            try
            {
                // Lấy địa chỉ IP cục bộ từ HttpContext
                var ipAddress = _httpContextAccessor.HttpContext?.Connection?.LocalIpAddress?.ToString();

                if (string.IsNullOrEmpty(ipAddress))
                {
                    _logger.LogWarning("Không thể lấy được địa chỉ IP cục bộ.");
                    return string.Empty;
                }

                return ipAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy địa chỉ IP cục bộ.");
                return string.Empty;
            }
        }
        #endregion
    }
}
