
using System.Text;

namespace AottgBotLib
{
    public class RoomCreationInfo
    {
        /// <summary>
        /// ID of the Room (Generates automatically, but if you want to specify it, you can)
        /// </summary>
        public string ID { get; set; } = "50505" + new System.Random().Next(100, 999).ToString();
        public string Name { get; set; }
        public MapDifficulity Difficulity { get; set; }
        public MapName Map { get; set; }
        public int ServerTime { get; set; }
        /// <summary>
        /// Sets pasword of room (NOT IMPLEMENTED YET)
        /// </summary>
        public string Password { get; set; }
        public Daylight Daylight { get; set; }

        /// <summary>
        /// Returns map name of Room
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public string GetMapName()
        {
            switch (Map)
            {
                case MapName.CityI:
                    return "The City";

                case MapName.CityIII:
                    return "The City III";

                case MapName.ForestI:
                    return "The Forest";

                case MapName.ForestII:
                    return "The Forest II";

                case MapName.ForestIII:
                    return "The Forest III";

                case MapName.ForestIV:
                    return "The Forest IV  - LAVA";

                case MapName.RacingAkina:
                    return "Racing - Akina";

                case MapName.Custom:
                    return "Custom";

                case MapName.CustomNoPt:
                    return "Custom (No PT)";

                default:
                    throw new System.NotSupportedException();
            }
        }

        /// <summary>
        /// Converts to aottg RoomInfo Nam, that can be read by AoTTG serverlist
        /// </summary>
        /// <returns></returns>
        public string ToServerString()
        {
            string[] data = new string[7];

            data[0] = Name;
            data[1] = GetMapName();
            data[2] = Difficulity.ToString().ToLower();
            data[3] = ServerTime.ToString();
            data[4] = Daylight.ToString().ToLower();
            data[5] = string.Empty; //TODO: Password
            data[6] = ID.ToString();

            return string.Join("`", data);
        }
    }
}
