using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace PusheenCustomExportCsv.Web.Models
{
    public class PusheenCsvResult : FileResult
    {
        private readonly IEnumerable<Pusheen> _pusheenData;

        public PusheenCsvResult(IEnumerable<Pusheen> pusheenData, string fileDownloadName) : base("text/csv")
        {
            _pusheenData = pusheenData;
            FileDownloadName = fileDownloadName;
        }

        public async override Task ExecuteResultAsync(ActionContext context)
        {
            var response = context.HttpContext.Response;
            context.HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + FileDownloadName });

            using (var streamWriter = new StreamWriter(response.Body)) {
              await streamWriter.WriteLineAsync(
                $"Pusheen, Food, SuperPower"
              );
              foreach (var p in _pusheenData)
              {
                await streamWriter.WriteLineAsync(
                  $"{p.Name}, {p.FavouriteFood}, {p.SuperPower}"
                );
                await streamWriter.FlushAsync();
              }
              await streamWriter.FlushAsync();
            }
        }

    }
}