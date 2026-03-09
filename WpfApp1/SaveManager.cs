using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


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
                    ProfileImagePath = GetRelativeImagePath(user.ProfileImage),
                    Games = user.Games.Select(g => ToGameData(g)).ToList()
                },
                AllGames = allGames.Select(g => ToGameData(g)).ToList()
            };

            if (string.IsNullOrEmpty(saveFile.User.ProfileImagePath) ||
                    !saveFile.User.ProfileImagePath.StartsWith("Images"))
            {
                saveFile.User.ProfileImagePath = "Images/Player1.jpg";
            }

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
            GameData res = new GameData()
            {
                Name = g.Name,
                tags = g.tags,
                genre = g.genre,
                device = g.device,
                release = g.release,
                ImagePath = GetRelativeImagePath(g.Illustration),
                Time = g.Time,
                Score = g.Score,
                State = g.State
            };
            if (string.IsNullOrEmpty(res.ImagePath) ||
                    !res.ImagePath.StartsWith("Images"))
            {
                res.ImagePath = "Images/GameTest.png";
            }
            
            return res;
        }

        private static string GetRelativeImagePath(BitmapImage image)
        {
            if (image?.UriSource == null)
                return null;

            Uri uri = image.UriSource;

            // Si c'est une URL web
            if (uri.IsAbsoluteUri && uri.Scheme.StartsWith("http"))
                return null;

            // Si c'est déjà un chemin relatif (Images/...)
            if (!uri.IsAbsoluteUri)
                return uri.ToString();

            // Si c'est un chemin absolu local
            string fullPath = uri.LocalPath;
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            if (fullPath.StartsWith(basePath))
                return fullPath.Substring(basePath.Length);

            return null;
        }

        public class SaveFileModel
        {
            public UserData User { get; set; }
            public List<GameData> AllGames { get; set; }
        }
     

    }


}
