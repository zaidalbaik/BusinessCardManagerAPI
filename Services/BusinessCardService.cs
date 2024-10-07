using BusinessCardManagerAPI.Data;
using BusinessCardManagerAPI.Data.Models;
using BusinessCardManagerAPI.DTOs;
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

        public async Task<(HttpStatusCode StatusCode, List<BusinessCard>? BusinessCards)> GetAllBusinessCardsAsync()
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

        public async Task<(HttpStatusCode StatusCode, string? XMLText)> ExportToXmlAsync(string id)
        {
            if (_context.BusinessCards == null)
            {
                return (HttpStatusCode.InternalServerError, null);
            }

            var businessCard = await _context.BusinessCards.FindAsync(id);
            if (businessCard == null)
                return (HttpStatusCode.NotFound, null);

            var serializer = new XmlSerializer(typeof(BusinessCard));
            using var writer = new StringWriter();
            serializer.Serialize(writer, businessCard);

            return (HttpStatusCode.OK, writer.ToString());
        }

        public async Task<HttpStatusCode> ImportXmlAsync(IFormFile file)
        {
            if (_context.BusinessCards == null)
            {
                return (HttpStatusCode.InternalServerError);
            }

            if (file == null || file.Length == 0)
                return (HttpStatusCode.BadRequest);

            using var stream = new StreamReader(file.OpenReadStream());
            var serializer = new XmlSerializer(typeof(BusinessCard));
            var businessCard = (BusinessCard?)serializer?.Deserialize(stream);

            if (businessCard == null)
            {
                return HttpStatusCode.BadRequest;
            }

            await _context.BusinessCards.AddAsync(businessCard);
            await _context.SaveChangesAsync();

            return (HttpStatusCode.OK);
        }

        public async Task<(HttpStatusCode StatusCode, string? CsvText)> ExportToCsvAsync(string id)
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

            // Create CSV content
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Name,Gender,DateOfBirth,Email,Phone,Address");

            csvBuilder.AppendLine($"{businessCard.Name},{businessCard.Gender},{businessCard.DateOfBirth},{businessCard.Email},{businessCard.Phone},{businessCard.Address}");

            return (HttpStatusCode.OK, csvBuilder.ToString());
        }

        public async Task<HttpStatusCode> ImportCsvAsync(IFormFile file)
        {
            if (_context.BusinessCards == null)
            {
                return HttpStatusCode.InternalServerError;
            }

            if (file == null || file.Length == 0)
            {
                return HttpStatusCode.BadRequest;
            }

            using var reader = new StreamReader(file.OpenReadStream());

            // Skip header line
            var header = await reader.ReadLineAsync();
            if (header == null)
            {
                return HttpStatusCode.BadRequest;
            }

            // Read the CSV content
            var line = await reader.ReadLineAsync();
            if (line == null)
            {
                return HttpStatusCode.BadRequest;
            }

            var fields = line.Split(',');

            if (fields.Length != 6)
            {
                return HttpStatusCode.BadRequest;
            }

            var businessCard = new BusinessCard
            { 
                Name = fields[0],
                Gender = fields[1],
                DateOfBirth = DateTime.Parse(fields[2]),
                Email = fields[3],
                Phone = fields[4],
                Address = fields[5]
            };
             
            await _context.BusinessCards.AddAsync(businessCard);
            await _context.SaveChangesAsync();

            return HttpStatusCode.OK;
        }
    }
}
