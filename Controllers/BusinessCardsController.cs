using BusinessCardManagerAPI.Data;
using BusinessCardManagerAPI.DTOs;
using BusinessCardManagerAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using System.Xml.Linq;

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
                        return Ok(new { result.BusinessCards });

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

        [HttpPost("import/xml")]
        public async Task<IActionResult> ImportXmlAsync(IFormFile file)
        {
            try
            {
                var result = await _businessCardService.ImportXmlAsync(file);
                switch (result)
                {
                    case HttpStatusCode.OK:
                        return Ok(new { Message = "Business card imported successfully from XML." });

                    case HttpStatusCode.BadRequest:
                        return BadRequest(new { Message = "No file provided or the file is empty." });

                    case HttpStatusCode.InternalServerError:
                        return Problem("Entity set 'DbContext.BusinessCards' is null.", statusCode: 500);

                    default:
                        return Problem("It was not imported correctly");
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "There was a problem reading the file or a problem occurred.", ErrorMessage = $"{e}" });
            }
        }

        // Export a single Business Card to XML based on the ID
        [HttpGet("export/xml/{id}")]
        public async Task<IActionResult> ExportBusinessCardToXml(string id)
        {
            try
            {
                var result = await _businessCardService.ExportToXmlAsync(id);
                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        if (string.IsNullOrEmpty(result.XMLText))
                        {
                            return Problem("It was not exported correctly");
                        }
                        var fileName = $"BusinessCard_{id}.xml";
                        return File(Encoding.UTF8.GetBytes(result.XMLText), "application/xml", fileName);

                    case HttpStatusCode.NotFound:
                        return NotFound(new { Message = $"Business card with ID {id} not found." });

                    case HttpStatusCode.InternalServerError:
                        return Problem("Entity set 'DbContext.BusinessCards' is null.", statusCode: 500);

                    default:
                        return Problem("It was not exported correctly");
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "A problem occurred while exporting this xml file.", ErrorMessage = $"{e}" });
            }
        }

        // Import Business Card from CSV
        [HttpPost("import/csv")]
        public async Task<IActionResult> ImportCsvAsync(IFormFile file)
        {
            try
            {
                var result = await _businessCardService.ImportCsvAsync(file);
                switch (result)
                {
                    case HttpStatusCode.OK:
                        return Ok(new { Message = "Business card(s) imported successfully from CSV." });

                    case HttpStatusCode.BadRequest:
                        return BadRequest(new { Message = "No file provided or the file is empty, or CSV format is invalid." });

                    case HttpStatusCode.InternalServerError:
                        return Problem("Entity set 'DbContext.BusinessCards' is null.", statusCode: 500);

                    default:
                        return Problem("It was not imported correctly");
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "There was a problem reading the file or a problem occurred.", ErrorMessage = $"{e}" });
            }
        }

        // Export a single Business Card to CSV based on the ID
        [HttpGet("export/csv/{id}")]
        public async Task<IActionResult> ExportBusinessCardToCsv(string id)
        {
            try
            {
                var result = await _businessCardService.ExportToCsvAsync(id);
                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        if (string.IsNullOrEmpty(result.CsvText))
                        {
                            return Problem("It was not exported correctly");
                        }
                        var fileName = $"BusinessCard_{id}.csv";
                        return File(Encoding.UTF8.GetBytes(result.CsvText), "text/csv", fileName);

                    case HttpStatusCode.NotFound:
                        return NotFound(new { Message = $"Business card with ID {id} not found." });

                    case HttpStatusCode.InternalServerError:
                        return Problem("Entity set 'DbContext.BusinessCards' is null.", statusCode: 500);

                    default:
                        return Problem("It was not exported correctly");
                }
            }
            catch (Exception e)
            {
                return BadRequest(new { Message = "A problem occurred while exporting this CSV file.", ErrorMessage = $"{e}" });
            }
        } 
    }
}
