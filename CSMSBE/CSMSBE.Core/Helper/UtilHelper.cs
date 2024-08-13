using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace CSMSBE.Core.Helper
{
    public static class UtilHelper
    {
        public static string BuildPathWithParentPath(string targetFolder, string parentPath, string childPath)
        {
            return $"{targetFolder}/{parentPath}/{childPath}";
        }


        public static string ConvertToUnSign(string s)
        {
            if (string.IsNullOrEmpty(s)) return s;
            string stFormD = s.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();
            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }
            sb = sb.Replace('Đ', 'D');
            sb = sb.Replace('đ', 'd');
            return (sb.ToString().Normalize(NormalizationForm.FormD)).ToLower();
        }

        public static string SenderRemoveOfsSpecialCharacter(string input)
        {

            input = input.Replace("&", "va");
            input = input.Replace("%", "per");
            input = input.Replace(",", ".");
            input = input.Replace("~", ".");
            input = input.Replace("`", ".");
            input = input.Replace("_", ".");
            input = input.Replace("?", ".");
            input = input.Replace("}", ".");
            input = input.Replace("{", ".");
            input = input.Replace("'", ".");
            input = input.Replace("<", ".");
            input = input.Replace(">", ".");
            input = input.Replace("/", ".");
            input = input.Replace(@"\", ".");
            return input;

        }

        public static string RemoveAccents(this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            char[] chars = text
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c)
                != UnicodeCategory.NonSpacingMark).ToArray();

            return new string(chars).Normalize(NormalizationForm.FormC);
        }

        public static string Slugify(this string phrase)
        {
            // Remove all accents and make the string lower case.  
            string output = phrase.RemoveAccents().ToLower();
            output = output.Replace('Đ', 'D');
            output = output.Replace('đ', 'd');
            // Remove all special characters from the string.  
            output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");

            // Remove all additional spaces in favour of just one.  
            output = Regex.Replace(output, @"\s+", " ").Trim();

            // Replace all spaces with the hyphen.  
            output = Regex.Replace(output, @"\s", "-");
            while (output.Contains("--"))
                output = output.Replace("--", "-");

            // Return the slug.  
            return output;
        }

        public static string GetCurrentDevice(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue(Constant.DeviceType, out var deviceType);
            return deviceType;
        }

        public static string GetCurrentToken(this HttpContext httpContext)
        {
            httpContext.Request.Headers.TryGetValue(Constant.TOKEN_CSMS, out var token);
            return token;
        }

        public static Coordinate GetCoordinate(string address, string googleApiKey)
        {
            var httpClient = new HttpClient();
            address = address + ",Việt Nam";
            string paramAddress = string.Format("address={0}", address);
            var requestUri = new Uri(string.Format(Constant.AnalysisAddressMessage.Url, paramAddress, googleApiKey));
            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    return null;
                }

                var response = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                JObject addressObj = JObject.Parse(response);
                var lstReverstGeoCoding = addressObj["results"].ToObject<List<GeoCoding>>();

                if (lstReverstGeoCoding != null && lstReverstGeoCoding.Count > 0)
                {
                    return lstReverstGeoCoding.FirstOrDefault()?.geometry?.location;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetAddressFromLngLat(double longtitude, double latitude, string googleApiKey)
        {
            var httpClient = new HttpClient();
            string paramLatlng = string.Format("latlng={0},{1}", latitude, longtitude);
            var requestUri = new Uri(string.Format(Constant.AnalysisAddressMessage.Url, paramLatlng, googleApiKey));
            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = httpClient.GetAsync(requestUri).Result;
                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    return string.Empty;
                }
                var response = httpResponseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                JObject addressObj = JObject.Parse(response);
                var lstReverstGeoCoding = addressObj["results"].ToObject<List<GeoCoding>>();
                if (lstReverstGeoCoding != null && lstReverstGeoCoding.Count > 0)
                {
                    return lstReverstGeoCoding.FirstOrDefault().formatted_address;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            HashSet<TResult> set = new HashSet<TResult>();

            foreach (var item in source)
            {
                var selectedValue = selector(item);

                if (set.Add(selectedValue))
                    yield return item;
            }
        }

        public static object GetPropValue(this object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
        public static void SetPropertyValue(this object obj, string propName, object value)
        {
            obj.GetType().GetProperty(propName).SetValue(obj, value, null);
        }
        public static string getStringValue(object cell)
        {
            if (cell == null)
            {
                return "";
            }
            else
            {
                if (cell.ToString() == "Thiếu thông tin" || cell.ToString() == "thiếu thông tin")
                {
                    return "";
                }
                else if (cell.ToString() == "99999999999999")
                {
                    return "00010101000000";
                }
                else
                    return cell.ToString().Trim();
            }
        }
        public static DateTime getDatetimeValue(object cell)
        {
            try
            {
                if (cell == null)
                {
                    return DateTime.ParseExact("00010101000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }
                else
                {
                    string value = cell.ToString().Trim();
                    if (value == "Thiếu thông tin" || value == "thiếu thông tin")
                    {
                        return DateTime.ParseExact("00010101000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    }
                    else if (value.Contains("999999"))
                    {

                        return DateTime.ParseExact("00010101000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                        return DateTime.ParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int getNumberValue(object cell)
        {
            try
            {
                if (cell == null)
                {
                    return 0;
                }
                else
                {
                    if (cell.ToString() == "Thiếu thông tin" || cell.ToString() == "thiếu thông tin" || cell.ToString() == "")
                    {
                        return 0;
                    }
                    else
                        return int.Parse(cell.ToString().Trim());
                }
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
    }
}
