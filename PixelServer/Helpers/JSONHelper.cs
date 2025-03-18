using PixelServer.External.SimpleJSON;
using PixelServer.Objects;

namespace PixelServer.Helpers
{
    public static class JSONHelper
    {
        public static List<long> GetIdsList(string json)
        {
            try
            {
                List<long> result = new();

                JSONNode index = JSON.Parse(json);

                foreach (JSONNode node in index) 
                    if (long.TryParse(node.Value, out long res)) result.Add(res);

                return result;
            }
            catch (Exception ex)
            {
                DebugHelper.LogException("Unable to parse Ids list", ex, false);
                return new();
            }
        }

        public static string PlayerDataToIdList(List<PlayerData> data)
        {
            JSONObject index = new();

            foreach (PlayerData p in data)
            {
                JSONObject playerNode = new()
                {
                    ["id"] = p.id,
                    ["nick"] = p.nick,
                    ["rank"] = p.rank,
                    ["skin"] = p.skin,
                    ["clan_name"] = p.clan_name,
                    ["clan_logo"] = p.clan_logo
                };

                index[p.id.ToString()]["player"] = playerNode;
            }

            return index.ToString();
        }

        public static string GetInfoByIDSerializer(PlayerData p)
        {
            JSONObject index = new();

            JSONObject wincount = new();
            foreach (var v in p.wincount) wincount.Add(v.Key.ToString(), v.Value);

            JSONObject playerNode = new()
            {
                ["id"] = p.id,
                ["nick"] = p.nick,
                ["rank"] = p.rank,
                ["skin"] = p.skin,
                ["clan_name"] = p.clan_name,
                ["clan_logo"] = p.clan_logo,
                ["clan_creator_id"] = p.clan_creator_id,
                ["wincount"] = wincount
            };

            index["player"] = playerNode;

            return index.ToString();
        }

        public static string GetInfoByParamSerializer(List<PlayerData> data)
        {
            JSONNode index = new JSONArray();

            foreach (PlayerData p in data)
            {
                JSONNode playerNode = new JSONObject()
                {
                    ["id"] = p.id
                };

                playerNode["nick"] = p.nick;
                playerNode["rank"] = p.rank;
                playerNode["skin"] = p.skin;
                playerNode["clan_name"] = p.clan_name;
                playerNode["clan_logo"] = p.clan_logo;
                playerNode["clan_creator_id"] = p.clan_creator_id;

                index.Add(playerNode);
            }

            return index.ToString();
        }
    }
}
