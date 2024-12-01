using BusinessCardManagerAPI.Data;
using BusinessCardManagerAPI.Data.Models;
using BusinessCardManagerAPI.DTOs;
using BusinessCardManagerAPI.Enums;
using BusinessCardManagerAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace BusinessCardManagerAPI.Services
{
    public class BusinessCardService : IBusinessCardService
    {
        private readonly BusinessCardDbContext _context;

        public BusinessCardService(BusinessCardDbContext context)
        {
            _context = context;
        }

        public async Task<(HttpStatusCode StatusCode, IEnumerable<BusinessCard>? BusinessCards)> GetAllBusinessCardsAsync()
        {
            if (_context.BusinessCards == null)
            {
                return (HttpStatusCode.InternalServerError, null);
            }

            var businessCards = await _context.BusinessCards.ToListAsync();

            if (businessCards == null)
            {
                return (HttpStatusCode.NotFound, null);
            }

            return (HttpStatusCode.OK, businessCards);
        }

        public async Task<(HttpStatusCode StatusCode, BusinessCard? BusinessCard)> GetBusinessCardAsync(string id)
        {
            if (_context.BusinessCards == null)
            {
                return (HttpStatusCode.InternalServerError, null);
            }

            var businessCard = await _context.BusinessCards.FindAsync(id);
            if (businessCard == null)
            {
                return (HttpStatusCode.NotFound, null);
            }

            return (HttpStatusCode.OK, businessCard);
        }

        public async Task<(HttpStatusCode StatusCode, string? Id)> CreateBusinessCardAsync(BusinessCardDTO businessCardDTO)
        {
            if (_context.BusinessCards == null)
            {
                return (HttpStatusCode.InternalServerError, null);
            }

            if (!IsPhotoValid(businessCardDTO.PhotoBase64))
            {
                return (HttpStatusCode.BadRequest, null);
            }

            var businessCard = new BusinessCard()
            {
                Name = businessCardDTO.Name,
                Gender = businessCardDTO.Gender,
                DateOfBirth = businessCardDTO.DateOfBirth,
                Email = businessCardDTO.Email,
                Phone = businessCardDTO.Phone,
                Address = businessCardDTO.Address,
                PhotoBase64 = businessCardDTO.PhotoBase64
            };

            await _context.BusinessCards.AddAsync(businessCard);
            await _context.SaveChangesAsync();

            return (HttpStatusCode.OK, businessCard.Id);
        }

        public async Task<HttpStatusCode> DeleteBusinessCardAsync(string id)
        {
            if (_context.BusinessCards == null)
            {
                return (HttpStatusCode.InternalServerError);
            }

            var businessCard = await _context.BusinessCards.FindAsync(id);
            if (businessCard == null)
            {
                return (HttpStatusCode.NotFound);
            }

            _context.BusinessCards.Remove(businessCard);
            await _context.SaveChangesAsync();

            return (HttpStatusCode.OK);
        }

        public async Task<(HttpStatusCode StatusCode, string? FileContent, string? Message)> ExportBusinessCardAsync(string id, FileType fileType)
        {
            if (_context.BusinessCards == null)
            {
                return (HttpStatusCode.InternalServerError, null, "Entity set 'DbContext.BusinessCards' is null.");
            }

            var businessCard = await _context.BusinessCards.FindAsync(id);
            if (businessCard == null)
            {
                return (HttpStatusCode.NotFound, null, $"Business card with ID {id} not found.");
            }

            switch (fileType)
            {
                case FileType.Csv:
                    // Create CSV content
                    var csvBuilder = new StringBuilder();
                    csvBuilder.AppendLine("Name,Gender,DateOfBirth,Email,Phone,Address,PhotoBase64");
                    csvBuilder.AppendLine($"{businessCard.Name},{businessCard.Gender},{businessCard.DateOfBirth.Date.ToString("yyyy-MM-dd")},{businessCard.Email},{businessCard.Phone},{businessCard.Address},{businessCard.PhotoBase64}");
                    return (HttpStatusCode.OK, csvBuilder.ToString(), null);

                case FileType.Xml:
                    // Create XML content
                    var serializer = new XmlSerializer(typeof(BusinessCard));
                    using (var writer = new StringWriter())
                    {
                        serializer.Serialize(writer, businessCard);
                        return (HttpStatusCode.OK, writer.ToString(), null);
                    }

                default:
                    return (HttpStatusCode.BadRequest, null, "Invalid file type. Supported types are 'csv' and 'xml'.");
            }
        }

        public async Task<IEnumerable<BusinessCard>> GetFilteredBusinessCards(
            string? name,
            DateTime? dateOfBirth,
            string? phone,
            string? gender,
            string? email)
        {
            var query = _context.BusinessCards.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(bc => bc.Name.Contains(name));
            }

            if (dateOfBirth.HasValue)
            {
                query = query.Where(bc => bc.DateOfBirth.Date == dateOfBirth);
            }

            if (!string.IsNullOrEmpty(phone))
            {
                query = query.Where(bc => bc.Phone.Contains(phone));
            }

            if (!string.IsNullOrEmpty(gender))
            {
                query = query.Where(bc => bc.Gender == gender);
            }

            if (!string.IsNullOrEmpty(email))
            {
                query = query.Where(bc => bc.Email.Contains(email));
            }

            return await query.ToListAsync();
        }

        public bool IsPhotoValid(string? photoBase64)
        {
            const int maxSizeInBytes = 1 * 1024 * 1024; // 1 MB

            if (string.IsNullOrEmpty(photoBase64)) //Because it's optional..
                return true;

            try
            {
                int imageSizeInBytes = (int)(photoBase64.Length * 3 / 4) - (photoBase64.EndsWith("==") ? 2 : (photoBase64.EndsWith("=") ? 1 : 0));

                return imageSizeInBytes <= maxSizeInBytes;
            }
            catch
            {
                return false;
            }
        }
    }
}
