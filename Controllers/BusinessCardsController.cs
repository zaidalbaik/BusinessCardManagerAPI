using BusinessCardManagerAPI.Data;
using BusinessCardManagerAPI.Data.Models;
using BusinessCardManagerAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BusinessCardManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardsController : ControllerBase
    {
        private readonly BusinessCardDbContext _context;

        public BusinessCardsController(BusinessCardDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetBusinessCards()
        {
            try
            {
                if (_context.BusinessCards == null)
                {
                    return Problem("Entity set 'DbContext.BusinessCards' is null.");
                }

                var businessCards = await _context.BusinessCards.Select(bc => new
                {
                    bc.Id,
                    bc.Name,
                    bc.Gender,
                    bc.DateOfBirth,
                    bc.Email,
                    bc.Phone,
                    bc.Address,
                    bc.PhotoBase64
                }).ToListAsync();

                if (businessCards == null)
                {
                    return NotFound(new { Message = "Business Cards not found" });
                }

                return Ok(new { Message = "Successfully retreve business cards", BusinessCards = businessCards });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "An error occurred when retreving the data", ErrorMessage = $"{e}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBusinessCard(string id)
        {
            try
            {
                if (_context.BusinessCards == null)
                {
                    return Problem("Entity set 'DbContext.BusinessCards' is null.");
                }

                var businessCard = await _context.BusinessCards.FindAsync(id);
                if (businessCard == null)
                {
                    return NotFound(new { Message = "Business card not found" });
                }

                var businessCardJson = new
                {
                    businessCard.Id,
                    businessCard.Name,
                    businessCard.Gender,
                    businessCard.DateOfBirth,
                    businessCard.Email,
                    businessCard.Phone,
                    businessCard.Address,
                    businessCard.PhotoBase64
                };

                return Ok(new { Message = "Successfully retreve business card", BusinessCard = businessCardJson });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "An error occurred when retreving the data", ErrorMessage = $"{e}" });
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutBusinessCard(string id, BusinessCardDTO businessCardDTO)
        {
            try
            {
                if (_context.BusinessCards == null)
                {
                    return Problem("Entity set 'DbContext.BusinessCards' is null.");
                }

                var businessCard = await _context.BusinessCards.FindAsync(id);
                if (businessCard == null)
                {
                    return NotFound(new { Message = "Business card not found" });
                }

                businessCard.Name = businessCardDTO.Name;
                businessCard.Gender = businessCardDTO.Gender;
                businessCard.DateOfBirth = businessCardDTO.DateOfBirth;
                businessCard.Email = businessCardDTO.Email;
                businessCard.Phone = businessCardDTO.Phone;
                businessCard.Address = businessCardDTO.Address;
                businessCard.PhotoBase64 = businessCardDTO.PhotoBase64;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Updated Successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "An error occurred when deleting", ErrorMessage = $"{e}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostBusinessCard(BusinessCardDTO businessCardDTO)
        {
            try
            {
                if (_context.BusinessCards == null)
                {
                    return Problem("Entity set 'DbContext.BusinessCards' is null.");
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

                return Ok(new { Message = "Successfully create new business card", BusinessCardId = businessCard.Id });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "An error occurred when adding", ErrorMessage = $"{e}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBusinessCard(string id)
        {
            try
            {
                if (_context.BusinessCards == null)
                {
                    return Problem("Entity set 'DbContext.BusinessCards' is null.");
                }

                var businessCard = await _context.BusinessCards.FindAsync(id);
                if (businessCard == null)
                {
                    return NotFound(new { Message = "Business card not found" });
                }

                _context.BusinessCards.Remove(businessCard);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Deleted Successfully" });
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "An error occurred when deleting", ErrorMessage = $"{e}" });
            }
        }
    }
}
