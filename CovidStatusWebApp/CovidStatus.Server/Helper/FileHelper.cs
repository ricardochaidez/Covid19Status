using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.JSInterop;

namespace CovidStatus.Server.Helper
{
    public class FileHelper
    {
        private IJSRuntime _jsRuntime;

        public FileHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task DownloadFileFromBytes(string fileName, byte[] fileBytes)
        {
            string mimeType = GetMimeType(fileName);

            await _jsRuntime.InvokeAsync<object>(
                "FileSaveAs",
                Convert.ToBase64String(fileBytes), fileName, mimeType);
        }

        private string GetMimeType(string fileName)
        {
            string contentType;
            new FileExtensionContentTypeProvider().TryGetContentType(fileName, out contentType);
            return contentType ?? "application/octet-stream";
        }
    }
}
