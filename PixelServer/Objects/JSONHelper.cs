using PixelServer.External.SimpleJSON;

namespace PixelServer.Objects
{
    public static class JSONHelper
    {
        public static string GetInfoByIDSerializer(PlayerData? p)
        {
            JSONNode index = new JSONArray();

            if (p == null)
            {
                index["player"] = new JSONObject();
                return index.ToString();
            }

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

            foreach (var v in p.wincount)
            {
                JSONNode wc = new JSONObject();

                wc.Add(v.Key.ToString(), v.Value);

                playerNode["wincount"] = wc;
            }

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
