using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.Json;
using System.Windows.Media.Imaging;

namespace WpfApp1
{
    public class GameApiService
    {
        private readonly HttpClient _client = new HttpClient();
        private const string ApiKey = "4939f3f2309f4bb8a7ee195d7cc1dade";

        public async Task<List<Game>> SearchGames(string search)
        {
            string url =
                $"https://api.rawg.io/api/games?key={ApiKey}&search={search}";

            var response = await _client.GetStringAsync(url);

            var rawgData = JsonSerializer.Deserialize<RawgResponse>(response);

            var games = new List<Game>();

            foreach (var g in rawgData.results)
            {
                games.Add(new Game
                {
                    Name = g.name,
                    OnlineImageUrl = g.background_image,
                    release = new List<string> { g.released },
                    genre = g.genres?.Select(x => x.name).ToList() ?? new List<string>(),
                    device = new List<string>(),
                    tags = new List<string>(),
                    Illustration = !string.IsNullOrEmpty(g.background_image)
                        ? new BitmapImage(new Uri(g.background_image))
                        : null,
                    State = GameState.Planned,
                    Score = 0,
                    Time = 0
                });
            }

            return games;
        }
    }
}
