using DictionaryUI_WPF.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        public void Dispose()
        {
            _client.Dispose();
        }
    }
}
