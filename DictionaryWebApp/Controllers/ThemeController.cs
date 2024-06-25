using DictionaryWebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DictionaryWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemeController : Controller
    {
        private readonly DataContext _context;

        public ThemeController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("AddTheme/")]
        public async Task<ActionResult<List<Theme>>> AddTheme([FromBody] Theme theme)
        {
            _context.Themes.Add(theme);
            await _context.SaveChangesAsync();

            return Ok(await _context.Themes.ToListAsync());
        }

        [HttpGet("GetAllThemes/")]
        public async Task<ActionResult<List<Theme>>> GetAllThemes()
        {
            return Ok(await _context.Themes.ToListAsync());
        }

        [HttpGet("GetThemeById/{id}")]
        public async Task<ActionResult<Theme>> GetTheme(int id)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound("Theme not found.");
            }
            return Ok(theme);
        }

        /// <summary>
        /// Удаление темы и всех связанных с ней слов и переводов
        /// </summary>
        /// <param name="themeId">ID темы, которую необходимо удалить</param>
        /// <returns></returns>
        [HttpDelete("DeleteThemeWithWords/{themeId}")]
        public async Task<IActionResult> DeleteThemeWithWords(int themeId)
        {
            var theme = await _context.Themes.FindAsync(themeId);
            if (theme == null)
            {
                return NotFound("Theme not found.");
            }

            var words = await _context.Words_Themes.Where(w => w.ThemeId == themeId).ToListAsync();
            _context.Words_Themes.RemoveRange(words);
            _context.Themes.Remove(theme);

            await _context.SaveChangesAsync();
            return Ok("Theme and related words and translations have been deleted successfully.");
        }
    }
}

