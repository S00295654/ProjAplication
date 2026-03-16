using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class RawgResponse
    {
        public List<RawgGame> results { get; set; }
    }

    public class RawgGame
    {
        public string name { get; set; }
        public string background_image { get; set; }
        public string released { get; set; }
        public List<RawgGenre> genres { get; set; }
        public List<RawgTags> tags { get; set; }
        public List<RawgPlatformWrapper> platforms { get; set; }
    }

    public class RawgPlatformWrapper
    {
        public RawgPlatform platform { get; set; }
    }

    public class RawgPlatform
    {
        public string name { get; set; }
    }

    public class RawgGenre
    {
        public string name { get; set; }
    }

    public class RawgTags
    {
        public string name { get; set; }
    }
}
