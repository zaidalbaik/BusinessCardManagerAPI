using BusinessCardManagerAPI.Data;
using BusinessCardManagerAPI.DTOs;
using BusinessCardManagerAPI.Enums;
using BusinessCardManagerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace BusinessCardManagerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessCardsController : ControllerBase
    {
        private readonly BusinessCardDbContext _context;
        private readonly IBusinessCardService _businessCardService;

        public BusinessCardsController(BusinessCardDbContext context, IBusinessCardService businessCardService)
        {
            _context = context;
            _businessCardService = businessCardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetBusinessCards()
        {
            try
            {
                var result = await _businessCardService.GetAllBusinessCardsAsync();

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Ok(result.BusinessCards);

                    case HttpStatusCode.NotFound:
                        return NotFound(new { Message = "Business Cards not found" });

                    case HttpStatusCode.InternalServerError:
                        return Problem("Entity set 'DbContext.BusinessCards' is null.", statusCode: 500);

                    default:
                        return Problem("An error occurred when retreving the data");
                }
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
                var result = await _businessCardService.GetBusinessCardAsync(id);

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Ok(new { result.BusinessCard });

                    case HttpStatusCode.NotFound:
                        return NotFound(new { Message = "Business Card not found" });

                    case HttpStatusCode.InternalServerError:
                        return Problem("Entity set 'DbContext.BusinessCards' is null.", statusCode: 500);

                    default:
                        return Problem("An error occurred when retreving the data");
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "An error occurred when retreving the data", ErrorMessage = $"{e}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostBusinessCard(BusinessCardDTO businessCardDTO)
        {
            try
            {
                var result = await _businessCardService.CreateBusinessCardAsync(businessCardDTO);

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        return Ok(new { Message = "Successfully create new business card", result.Id });

                    case HttpStatusCode.InternalServerError:
                        return Problem("Entity set 'DbContext.BusinessCards' is null.", statusCode: 500);

                    case HttpStatusCode.BadRequest:
                        return BadRequest(new { Message = "Image size exceeds the 1 MB limit." });

                    default:
                        return Problem("Not added correctly");
                }
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
                var result = await _businessCardService.DeleteBusinessCardAsync(id);
                switch (result)
                {
                    case HttpStatusCode.OK:
                        return Ok(new { Message = "Deleted Successfully" });

                    case HttpStatusCode.NotFound:
                        return NotFound(new { Message = "Business card not found" });

                    case HttpStatusCode.InternalServerError:
                        return Problem("Entity set 'DbContext.BusinessCards' is null.", statusCode: 500);

                    default:
                        return Problem("Not deleted correctly");
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "An error occurred when deleting", ErrorMessage = $"{e}" });
            }
        }

        // Export a Business Card based on the ID and fileType (CSV or XML)
        [HttpGet("export")]
        public async Task<IActionResult> ExportBusinessCard([FromQuery] string id, [FromQuery] FileType fileType)
        {
            try
            {
                var result = await _businessCardService.ExportBusinessCardAsync(id, fileType);

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        if (string.IsNullOrEmpty(result.FileContent))
                        {
                            return Problem("It was not exported correctly.");
                        }

                        var fileExtension = fileType.ToString().ToLower();
                        var contentType = fileExtension == "csv" ? "text/csv" : "application/xml";
                        var fileName = $"BusinessCard_{id}.{fileExtension}";

                        return File(Encoding.UTF8.GetBytes(result.FileContent), contentType, fileName);

                    case HttpStatusCode.NotFound:
                        return NotFound(new { result.Message });

                    case HttpStatusCode.InternalServerError:
                        return Problem(result.Message, statusCode: 500);

                    case HttpStatusCode.BadRequest:
                        return BadRequest(new { result.Message });

                    default:
                        return Problem("It was not exported correctly.");
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = $"A problem occurred while exporting the {fileType.ToString()} file.", ErrorMessage = $"{e}" });
            }
        }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredBusinessCards(
        [FromQuery] string? name,
        [FromQuery] DateTime? dateOfBirth,
        [FromQuery] string? phone,
        [FromQuery] string? gender,
        [FromQuery] string? email)
        {
            var businessCards = await _businessCardService.GetFilteredBusinessCards(name, dateOfBirth, phone, gender, email);
            return Ok(businessCards);
        }
    }
}
