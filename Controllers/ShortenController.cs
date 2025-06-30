using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortenerAPI.Data;
using UrlShortenerAPI.Models;

namespace UrlShortenerAPI.Controllers;

[ApiController]
[Route("Shorten")]
public class ShortenController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ShortenController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateShortUrl([FromBody] UrlMappingRequest request)
    {
        if (!Uri.IsWellFormedUriString(request.Url, UriKind.Absolute))
            return BadRequest("URL inválida");
       
        var shortCode = GenerateShortCode();

        var now = DateTime.UtcNow;

        var mapping = new UrlMapping
        {
            Url = request.Url,
            ShortCode = shortCode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            AccessCount = 0
        };

        _context.UrlMappings.Add(mapping);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetOriginalUrl), new { code = shortCode }, mapping);
       
    }

    [HttpGet("{code}")]
    public async Task<IActionResult> GetOriginalUrl(string code)
    {
        var mapping = await _context.UrlMappings.FirstOrDefaultAsync(u => u.ShortCode == code);
        if (mapping == null)
            return NotFound();

        mapping.AccessCount++;
        mapping.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        return Redirect(mapping.Url);
        //return Ok(mapping);
    }
        private string GenerateShortCode()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    [HttpPut("{code}")]
    public async Task<IActionResult> UpdateUrl(string code, [FromBody] UrlMapping update)
    {
        var mapping = await _context.UrlMappings.FirstOrDefaultAsync(u => u.ShortCode == code);
        if (mapping == null)
            return NotFound();

        mapping.Url = update.Url;
        mapping.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return Ok(mapping);
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> DeleteUrl(string code)
    {
        var mapping = await _context.UrlMappings.FirstOrDefaultAsync(u => u.ShortCode == code);
        if (mapping == null)
            return NotFound();

        _context.UrlMappings.Remove(mapping);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{code}/stats")]
    public async Task<IActionResult> GetStats(string code)
    {
        var mapping = await _context.UrlMappings.FirstOrDefaultAsync(u => u.ShortCode == code);
        if (mapping == null)
            return NotFound();

        return Ok(mapping);
    }
}
