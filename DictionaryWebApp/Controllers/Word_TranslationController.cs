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
            var existingTheme = await _context.Themes.FindAsync(word_tr.ThemeId);
            if (existingTheme == null)
            {
                return BadRequest("Theme with the specified ID does not exist.");
            }

            _context.Words_Themes.Add(word_tr);
            await _context.SaveChangesAsync();

            return Ok(await _context.Words_Themes.ToListAsync());
        }

        /// <summary>
        /// Добавление слова с переводом к определенной теме по Id
        /// </summary>
        /// <param name="themeId">Идентификатор темы</param>
        /// <param name="word_tr">Слово с переводом</param>
        /// <returns>Список слов после добавления</returns>
        [HttpPost("AddWord_trToTheme/{themeId}")]
        public async Task<ActionResult<List<Word_Tr>>> AddWordToTheme(int themeId, [FromBody] Word_Tr word_tr)
        {
            // Проверка существует ли тема
            var theme = await _context.Themes.FindAsync(themeId);
            if (theme == null)
            {
                return NotFound($"Theme with Id {themeId} not found.");
            }

            // Установить идентификатор темы для слова с переводом
            word_tr.ThemeId = themeId;

            // Добавление слова с переводом
            _context.Words_Themes.Add(word_tr);
            await _context.SaveChangesAsync();

            // Получение и возврат обновленного списка слов для темы
            var wordsForTheme = await _context.Words_Themes
                .Where(wt => wt.ThemeId == themeId)
                .ToListAsync();

            return Ok(wordsForTheme);
        }

        /// <summary>
        /// Создание новой темы и добавление слова с переводом в эту тему
        /// </summary>
        /// <param name="name">Название темы</param>
        /// <param name="word">Слово</param>
        /// <param name="translation">Перевод слова</param>
        /// <returns>ActionResult</returns>
        [HttpPost("AddNewThemeWithWord_tr/")]
        public async Task<IActionResult> AddNewThemeWithWord([FromBody] NewThemeWithWordDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.Name) || string.IsNullOrEmpty(dto.Word) || string.IsNullOrEmpty(dto.Translation))
            {
                return BadRequest("Некорректные данные.");
            }

            try
            {
                // Создаем новую тему
                var theme = new Theme { Name = dto.Name };
                _context.Themes.Add(theme);
                await _context.SaveChangesAsync();

                // После сохранения темы, у нее будет Id. Мы используем этот Id для создания нового слова.
                var word_tr = new Word_Tr { Word = dto.Word, Translation = dto.Translation, ThemeId = theme.Id };
                _context.Words_Themes.Add(word_tr);
                await _context.SaveChangesAsync();

                // Возврат успешного создания
                return Ok("Тема и слово успешно добавлены");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Внутренняя ошибка сервера: {ex.Message}");
            }
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

        /// <summary>
        /// Удаление слова с переводом по Id
        /// </summary>
        /// <param name="id">Id слова с переводом</param>
        /// <returns>Статусный код и список оставшихся слов</returns>
        [HttpDelete("DeleteWord_Tr/{id}")]
        public async Task<ActionResult<List<Word_Tr>>> DeleteWord_Tr(int id)
        {
            var word_tr = await _context.Words_Themes.FindAsync(id);
            if (word_tr == null)
            {
                return NotFound("Word_Translation not found.");
            }

            _context.Words_Themes.Remove(word_tr);
            await _context.SaveChangesAsync();

            return Ok(await _context.Words_Themes.ToListAsync());
        }

    }
}
