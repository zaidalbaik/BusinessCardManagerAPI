using BusinessCardManagerAPI.Data.Models;
using BusinessCardManagerAPI.DTOs;
using BusinessCardManagerAPI.Enums;
using System.Net;

namespace BusinessCardManagerAPI.Services.Interfaces
{
    public interface IBusinessCardService
    {
        public Task<(HttpStatusCode StatusCode, IEnumerable<BusinessCard>? BusinessCards)> GetAllBusinessCardsAsync();
        public Task<(HttpStatusCode StatusCode, BusinessCard? BusinessCard)> GetBusinessCardAsync(string id);
        public Task<(HttpStatusCode StatusCode, string? Id)> CreateBusinessCardAsync(BusinessCardDTO businessCardDto);
        public Task<HttpStatusCode> DeleteBusinessCardAsync(string id);
        public Task<(HttpStatusCode StatusCode, string? FileContent, string? Message)> ExportBusinessCardAsync(string id, FileType fileType); 
        public Task<IEnumerable<BusinessCard>> GetFilteredBusinessCards(string? name, DateTime? dateOfBirth, string? phone, string? gender, string? email);
    }
}
