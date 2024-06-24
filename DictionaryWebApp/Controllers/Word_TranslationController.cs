using DictionaryWebApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DictionaryWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Word_TranslationController : Controller
    {
        private readonly DataContext _context;

        public Word_TranslationController(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Добавление слова с переводом
        /// </summary>
        /// <param name="word_tr"></param>
        /// <returns></returns>
        [HttpPost("AddWord_Tr/")]
        public async Task<ActionResult<List<Word_Tr>>> AddWord_Tr([FromBody] Word_Tr word_tr)
        {
            _context.Words_Themes.Add(word_tr);
            await _context.SaveChangesAsync();

            return Ok(await _context.Words_Themes.ToListAsync());
        }

        /// <summary>
        /// Получение всех слов
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllWords_Tr/")]
        public async Task<ActionResult<List<Word_Tr>>> GetAllWords_Trs()
        {
            return Ok(await _context.Words_Themes.ToListAsync());
        }

        /// <summary>
        /// Получение Слова с переводом по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Word_Tr</returns>
        [HttpGet("GetWordsById/{id}")]
        public async Task<ActionResult<Word_Tr>> GetWord_Tr(int id)
        {
            var word_tr = await _context.Words_Themes.FindAsync(id);
            if (word_tr == null)
            {
                return BadRequest("Word_Translation not found.");
            }
            return Ok(word_tr);
        }

        /// <summary>
        /// Получние слов по {id}темы
        /// </summary>
        /// <param name="themeId">{id}темы</param>
        /// <returns>Word_Tr</returns>
        [HttpGet("GetWordsByTheme/{themeId}")]
        public async Task<ActionResult<List<Word_Tr>>> GetWordsByTheme(int themeId)
        {
            var wordsByTheme = await _context.Words_Themes
            .Where(wt => wt.ThemeId == themeId)
            .ToListAsync();

            if (!wordsByTheme.Any())
            {
                return NotFound("No words found for the specified theme.");
            }

            return Ok(wordsByTheme);
        }

    }
}
