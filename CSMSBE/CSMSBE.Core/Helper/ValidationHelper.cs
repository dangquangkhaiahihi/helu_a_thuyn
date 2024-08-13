using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CSMSBE.Core.Helper
{
    public class ValidationHelper
    {
        // Validates an email address using a regular expression.
        public static bool IsValidEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return false;

                string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";

                // Check if the email matches the pattern
                return Regex.IsMatch(email, emailPattern);
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        // Validates a phone number using a regular expression.
        public static bool IsValidPhoneNumber(string number)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(number))
                        return false;

                    string phonePattern = @"^(03|05|07|08|09|01[2689])+([0-9]{8})\b";

                    // Check if the phone number matches the pattern
                    return Regex.IsMatch(number, phonePattern);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        public static bool IsValidDateTime(string input)
        {
            try
            {
                // Define the format of the date string
                // This can be adjusted according to the expected format of the input
                string[] formats = {
                    "MM/dd/yyyy HH:mm:ss", "MM-dd-yyyy HH:mm:ss",
                    "yyyy-MM-dd HH:mm:ss", "dd-MM-yyyy HH:mm:ss",
                    // Include formats without seconds if needed
                    "MM/dd/yyyy HH:mm", "MM-dd-yyyy HH:mm",
                    "yyyy-MM-dd HH:mm", "dd-MM-yyyy HH:mm"
                };
                DateTime parsedDate;
                // Try to parse the input string to a DateTime object
                bool isValidFormat = DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate);
                return isValidFormat;
                // Set the Kind property to Unspecified to match the PostgreSQL date without timezone
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

