using BusinessCardManagerAPI.Data.Models;
using BusinessCardManagerAPI.DTOs;
using System.Net;

namespace BusinessCardManagerAPI.Services.Interfaces
{
    public interface IBusinessCardService
    {
        public Task<(HttpStatusCode StatusCode, List<BusinessCard>? BusinessCards)> GetAllBusinessCardsAsync();
        public Task<(HttpStatusCode StatusCode, BusinessCard? BusinessCard)> GetBusinessCardAsync(string id);
        public Task<(HttpStatusCode StatusCode, string? Id)> CreateBusinessCardAsync(BusinessCardDTO businessCardDto);
        public Task<HttpStatusCode> DeleteBusinessCardAsync(string id);

        public Task<HttpStatusCode> ImportXmlAsync(IFormFile file);
        public Task<(HttpStatusCode StatusCode, string? XMLText)> ExportToXmlAsync(string id);

        Task<HttpStatusCode> ImportCsvAsync(IFormFile file);
        Task<(HttpStatusCode StatusCode, string? CsvText)> ExportToCsvAsync(string id);
    }
}
