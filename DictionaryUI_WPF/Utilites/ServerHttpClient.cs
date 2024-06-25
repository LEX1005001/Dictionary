using DictionaryUI_WPF.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryUI_WPF.Utilites
{
    public class ServerHttpClient : IDisposable
    {
        private readonly HttpClient _client;

        public ServerHttpClient()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:7223/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        // Метод для получения списка всех тем
        public async Task<List<Theme>> GetAllThemesAsync()
        {
            List<Theme> themes = new List<Theme>();

            HttpResponseMessage response = await _client.GetAsync("api/Theme/GetAllThemes");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                themes = JsonConvert.DeserializeObject<List<Theme>>(json);
            }

            return themes;
        }

        // Метод для получения темы по ID
        public async Task<Theme> GetThemeByIdAsync(int themeId)
        {
            Theme theme = null;

            HttpResponseMessage response = await _client.GetAsync($"api/Theme/GetThemeById/{themeId}");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                theme = JsonConvert.DeserializeObject<Theme>(json);
            }

            return theme;
        }

        // Метод для получения списка слов по теме
        public async Task<List<Word_Tr>> GetWordsByThemeAsync(int themeId)
        {
            List<Word_Tr> words = new List<Word_Tr>();

            HttpResponseMessage response = await _client.GetAsync($"api/Word_Translation/GetWordsByTheme/{themeId}");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                words = JsonConvert.DeserializeObject<List<Word_Tr>>(json);
            }

            return words;
        }

        // Метод для добавления слова с переводом к определенной теме по Id
        public async Task<List<Word_Tr>> AddWordToThemeAsync(int themeId, Word_Tr word_tr)
        {
            var jsonContent = JsonConvert.SerializeObject(word_tr);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _client.PostAsync($"api/Word_Translation/AddWord_trToTheme/{themeId}", contentString);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                var wordsForTheme = JsonConvert.DeserializeObject<List<Word_Tr>>(json);
                return wordsForTheme;
            }

            throw new HttpRequestException($"Error adding word to theme with ID {themeId}: {response.ReasonPhrase}");
        }

        // Метод для добавления новой темы с словом и переводом
        public async Task<bool> AddNewThemeWithWordAsync(string name, string word, string translation)
        {
            try
            {
                var newThemeData = new
                {
                    name = name,
                    word = word,
                    translation = translation
                };

                HttpResponseMessage response = await _client.PostAsync("api/Word_Translation/AddNewThemeWithWord_tr/",
                    new StringContent(JsonConvert.SerializeObject(newThemeData), Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Server error: {responseContent}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception occurred: {ex.Message}");
                return false;
            }
        }

        // Метод для удаления слова с переводом по Id
        public async Task<List<Word_Tr>> DeleteWord_TrAsync(int id)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/Word_Translation/DeleteWord_Tr/{id}");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Word_Tr>>(json);
            }
            else
            {
                throw new Exception("Ошибка при удалении слова с переводом: " + response.ReasonPhrase);
            }
        }

        // Метод для удаления темы и всех связанных с ней слов и переводов
        public async Task<string> DeleteThemeWithWordsAsync(int themeId)
        {
            HttpResponseMessage response = await _client.DeleteAsync($"api/Theme/DeleteThemeWithWords/{themeId}");

            if (response.IsSuccessStatusCode)
            {
                return "Theme and related words and translations have been deleted successfully.";
            }
            else
            {
                return $"Failed to delete theme: {response.StatusCode}.";
            }
        }




        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
