using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CSMSBE.Core.Helper
{
    public class FileHelper
    {
        private static readonly string[] _allowedExtensions = { ".jpg", ".png", ".gif", ".jpeg", ".tiff", ".docx", ".doc", ".xls", ".xlsx", ".pdf", ".txt" };

        // 1. Check if folder path not exist, create the folder
        public static void EnsureFolderExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        // 2. Handle IFormFile and store it in specific path
        public static async Task<string> SaveFileAsync(IFormFile file, string path)
        {
            EnsureFolderExists(path);
            if (file == null || file.Length == 0)
            {
                throw new Exception("File không tồn tại hoặc dữ liệu trống!");
            }

            var uniqueName = GetUniqueFileName(path, file.FileName);
            var filePath = Path.Combine(path, uniqueName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the relative path
            return Path.GetRelativePath(Directory.GetCurrentDirectory(), filePath);
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                throw new Exception($"File {path} does not exist!");
            }
        }

        // 3. Download the file
        public static async Task<FileResult> DownloadFile(string fileName, string filePath)
        {
            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var fileType = GetContentType(filePath);
            return new FileContentResult(fileBytes, fileType)
            {
                FileDownloadName = fileName
            };
        }

        // 4. Send file data to frontend to preview
        public static async Task<FileResult> PreviewFileAsync(string filePath)
        {
            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var fileType = GetContentType(filePath);
            return new FileContentResult(fileBytes, fileType);
        }

        // 5. Check file type allowed to upload from client
        public static bool IsFileTypeAllowed(IFormFile file, string[] fileExtensionsAllowed)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return Array.Exists(fileExtensionsAllowed, e => e.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        public static async Task<FileContentResult> DownloadFileAsync(string fileName, string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File không tồn tại.", filePath);
            }

            var fileBytes = await File.ReadAllBytesAsync(filePath);
            var fileType = GetContentType(filePath);
            return new FileContentResult(fileBytes, fileType)
            {
                FileDownloadName = fileName
            };
        }

        public static string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types.ContainsKey(ext) ? types[ext] : "application/octet-stream";
        }

        public static string GetUniqueFileName(string fileDirectory, string fileName)
        {
            var fileExtension = Path.GetExtension(fileName);
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string pattern = @$"\(\d+\)$";
            fileNameWithoutExtension = Regex.Replace(fileNameWithoutExtension, pattern, string.Empty).Trim();
            var newFileName = fileNameWithoutExtension + fileExtension;    
            var counter = 1;
            while (File.Exists(Path.Combine(fileDirectory, newFileName)))
            {
                newFileName = $"{fileNameWithoutExtension} ({counter}){fileExtension}";
                counter++;
            }

            return newFileName;
        }

        private static Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/vnd.ms-word" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".png", "image/png" },
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".gif", "image/gif" },
                { ".csv", "text/csv" }
            };
        }
    }
}
