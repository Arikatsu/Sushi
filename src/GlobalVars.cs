using Sushi.Utils;
using MongoDB.Driver;

namespace Sushi
{
    internal class GlobalVars
    {
#pragma warning disable CS8618

        public static ConfigStruct Config { get; set; }

        public static IMongoDatabase Database { get; set; }
#pragma warning restore CS8618
    }
}
