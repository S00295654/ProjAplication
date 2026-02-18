using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.IO;
using System.Diagnostics;


namespace WpfApp1
{


    public static class SaveManager
    {
        private static string filePath = "userdata.json";

        public static void Save(User user, ObservableCollection<Game> allGames)
        {
            Debug.WriteLine("Saving...");
            var saveFile = new SaveFileModel
            {
                User = new UserData
                {
                    Name = user.Name,
                    Description = user.Description,
                    ProfileImagePath = user.ProfileImage?.UriSource?.ToString(),
                    Games = user.Games.Select(g => ToGameData(g)).ToList()
                },
                AllGames = allGames.Select(g => ToGameData(g)).ToList()
            };

            var options = new JsonSerializerOptions { WriteIndented = true, PropertyNameCaseInsensitive = true };
            string json = JsonSerializer.Serialize(saveFile, options);

            File.WriteAllText(filePath, json);
        }

        public static SaveFileModel Load()
        {
            if (!File.Exists(filePath))
                return null;

            string json = File.ReadAllText(filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<SaveFileModel>(json, options);
        }

        private static GameData ToGameData(Game g)
        {
            return new GameData
            {
                Name = g.Name,
                tags = g.tags,
                genre = g.genre,
                device = g.device,
                release = g.release,
                ImagePath = g.Illustration?.UriSource?.ToString(),
                Time = g.Time,
                Score = g.Score,
                State = g.State
            };
        }
        public class SaveFileModel
        {
            public UserData User { get; set; }
            public List<GameData> AllGames { get; set; }
        }

    }


}
