using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using Newtonsoft.Json;
using System.Collections.Specialized;
using FragenGerangel.GameBase;
using System.Runtime.CompilerServices;
using FragenGerangel.Utils.Exceptions;

namespace FragenGerangel.Utils.API
{
    public class APIManager
    {
        private static readonly HttpClient client = new HttpClient()
        {
            BaseAddress = new Uri("http://127.0.0.1:80/PHPAPI/")
        };

        private string username;
        private string password;
        private string auth;
        private DateTime authSince;

        public APIManager(string username, string password)
        {
            this.username = username;
            this.password = password;

            Init().Wait();
            Globals.Player = new Player(username);
        }

        private async Task Init()
        {
            try
            {
                await CreateUser().ConfigureAwait(false);
            }
            catch
            {

            }
            auth = await GetAuthToken().ConfigureAwait(false);
            authSince = DateTime.Now;
        }

        private async Task<string> Post(string uri, JObject json)
        {
            StringContent content = new StringContent(json.ToString(), Encoding.UTF8, "application/json");
            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, uri);
            message.Content = content;
            HttpResponseMessage response = await client.SendAsync(message).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        private async Task<JObject> PostReturnJson(string uri, JObject json)
        {
            if (uri != "getAuthToken.php" && uri != "createUser.php" && DateTime.Now - authSince >= new TimeSpan(24, 0, 0))
                Init().Wait();
            string resultStr = await Post(uri, json).ConfigureAwait(false);
            Console.WriteLine(resultStr);
            JObject resultJson = JObject.Parse(resultStr);
            if (resultJson["result"].ToString() != "ok")
                throw APIExceptionManager.FromID(resultJson["error_code"].ToObject<int>());
            return resultJson;
        }

        private async Task CreateUser()
        {
            JObject json = new JObject();
            json["username"] = username;
            json["password"] = password;
            JObject resultJson = await PostReturnJson("createUser.php", json).ConfigureAwait(false);
        }

        private async Task<string> GetAuthToken()
        {
            JObject json = new JObject();
            json["username"] = username;
            json["password"] = password;
            JObject resultJson = await PostReturnJson("getAuthToken.php", json).ConfigureAwait(false);
            return resultJson["token"].ToString();
        }

        public async Task BefriendUser(string username)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["username"] = username;
            JObject resultJson = await PostReturnJson("befriendUser.php", json).ConfigureAwait(false);
        }

        public async Task<Player[]> GetFriendRequests()
        {
            JObject json = new JObject();
            json["auth"] = auth;
            JObject resultJson = await PostReturnJson("getFriendRequests.php", json).ConfigureAwait(false);
            Player[] res = new Player[resultJson["usernames"].Count()];
            for (int i = 0; i < res.Length; i++)
                res[i] = new Player(resultJson["usernames"][i].ToObject<string>());
            return res;
        }

        public async Task<Player[]> GetFriends()
        {
            JObject json = new JObject();
            json["auth"] = auth;
            JObject resultJson = await PostReturnJson("getFriends.php", json).ConfigureAwait(false);
            Player[] res = new Player[resultJson["usernames"].Count()];
            for (int i = 0; i < res.Length; i++)
                res[i] = new Player(resultJson["usernames"][i].ToObject<string>());
            return res;
        }

        public async Task StartDuel(Player p)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["username"] = p.Name;
            await PostReturnJson("startDuel.php", json).ConfigureAwait(false);
        }

        public async Task<Player[]> GetDuelRequests()
        {
            JObject json = new JObject();
            json["auth"] = auth;
            JObject resultJson = await PostReturnJson("getDuelRequests.php", json).ConfigureAwait(false);
            Player[] res = new Player[resultJson["usernames"].Count()];
            for (int i = 0; i < res.Length; i++)
                res[i] = new Player(resultJson["usernames"][i].ToObject<string>());
            return res;
        }

        private async Task<Game[]> GetDuelIDs()
        {
            JObject json = new JObject();
            json["auth"] = auth;
            JObject resultJson = await PostReturnJson("getDuelIDs.php", json).ConfigureAwait(false);
            Game[] res = new Game[resultJson["games"].Count()];
            for (int i = 0; i < res.Length; i++)
            {
                Player p = new Player(resultJson["games"][i]["username"].ToObject<string>());
                int onlineID = resultJson["games"][i]["gameID"].ToObject<int>();
                res[i] = new Game(p, onlineID);
            }
            return res;
        }

        private async Task<Round[]> GetRounds(int gameID) 
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["gameID"] = gameID;
            JObject resultJson = await PostReturnJson("getRounds.php", json).ConfigureAwait(false);
            Round[] rounds = new Round[6];
            for (int i = 0; i < resultJson["rounds"].Count(); i++)
            {
                int id = resultJson["rounds"][i]["id"].ToObject<int>();
                string cat1 = resultJson["rounds"][i]["cat1"].ToObject<string>();
                string cat2 = resultJson["rounds"][i]["cat2"].ToObject<string>();
                string cat3 = resultJson["rounds"][i]["cat3"].ToObject<string>();
                rounds[i] = new Round(id, cat1, cat2, cat3);
            }
            return rounds;
        }

        public async Task ChooseCategory(Game g, int category)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["gameID"] = g.OnlineID;
            json["category"] = category;
            await PostReturnJson("chooseCategory.php", json).ConfigureAwait(false);
        }

        private async Task<QuestionAnswer[]> GetQuestionAnswers(Round r)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["roundID"] = r.OnlineID;
            JObject resultJson;
            try
            {
                resultJson = await PostReturnJson("getQuestionAnswers.php", json).ConfigureAwait(false);
            }
            catch (InsufficientPermissionsException e)
            {
                return null;
            }
            QuestionAnswer[] qas = new QuestionAnswer[3];
            foreach (JToken t in resultJson["questionAnswers"])
            {
                int id = t["id"].ToObject<int>();
                int order = t["order"].ToObject<int>();
                int questionID = t["questionID"].ToObject<int>();
                int answerPlayer = t["yourAnswer"].ToObject<object>() == null ? -1 : t["yourAnswer"].ToObject<int>();
                int answerRemotePlayer = t["enemyAnswer"].ToObject<object>() == null ? -1 : t["enemyAnswer"].ToObject<int>();
                Question question = await GetQuestion(questionID).ConfigureAwait(false);
                QuestionAnswer qa = new QuestionAnswer(id, question);
                qa.AnswerPlayer = answerPlayer;
                qa.AnswerRemotePlayer = answerRemotePlayer;
                qas[order - 1] = qa;
            }
            return qas;
        }

        private async Task<Question> GetQuestion(int questionID)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["questionID"] = questionID;
            JObject jsonResult = await PostReturnJson("getQuestion.php", json).ConfigureAwait(false);
            string question = jsonResult["question"]["question"].ToObject<string>();
            string category = jsonResult["question"]["category"].ToObject<string>();
            string correctAnswer = jsonResult["question"]["correctAnswer"].ToObject<string>();
            string wrongAnswer1 = jsonResult["question"]["wrongAnswer1"].ToObject<string>();
            string wrongAnswer2 = jsonResult["question"]["wrongAnswer2"].ToObject<string>();
            string wrongAnswer3 = jsonResult["question"]["wrongAnswer3"].ToObject<string>();
            return new Question(question, category, correctAnswer, wrongAnswer1, wrongAnswer2, wrongAnswer3);
        }

        public async Task<float?> UploadQuestionAnswer(QuestionAnswer qa)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["id"] = qa.OnlineID;
            json["answer"] = qa.AnswerPlayer;
            JObject resultJson = await PostReturnJson("uploadQuestionAnswer.php", json).ConfigureAwait(false);
            return resultJson["deltaElo"]?.ToObject<float?>();
        } 

        private async Task GetGame(Game g)
        {
            g.Rounds = await GetRounds(g.OnlineID).ConfigureAwait(false);
            foreach (Round r in g.Rounds)
                if (r != null)
                    r.Questions = await GetQuestionAnswers(r).ConfigureAwait(false);
        }

        public async Task<Game[]> GetGames()
        {
            Game[] games = await GetDuelIDs().ConfigureAwait(false);
            foreach (Game g in games)
                await GetGame(g).ConfigureAwait(false);
            return games;
        }

        public async Task<Statistic> GetStatistics(Player p = null)
        {
            JObject json = new JObject();
            json["auth"] = auth;
            json["username"] = p == null ? username : p.Name;
            JObject resultJson = await PostReturnJson("getStatistics.php", json).ConfigureAwait(false);
            int elo = resultJson["elo"].ToObject<int>();
            int wins = resultJson["wins"].ToObject<int>();
            int draws = resultJson["draws"].ToObject<int>();
            int losses = resultJson["losses"].ToObject<int>();
            int perfectGames = resultJson["perfectGames"].ToObject<int>();
            Dictionary<string, float> categoryPercentages = new Dictionary<string, float>();
            foreach (JToken t in resultJson["categoryPercentages"])
            {
                string category = t["category"].ToObject<string>();
                float percentage = t["percentage"].ToObject<float>();
                categoryPercentages.Add(category, percentage);
            }
            return new Statistic()
            {
                ELO = elo,
                Wins = wins,
                Draws = draws,
                Losses = losses,
                PerfectGames = perfectGames,
                CategoryPercentages = categoryPercentages,
                Player = p == null ? new Player(username) : p
            };
        }


        Game[] games; // DEBUG
        /// <summary>
        /// Debug function, plays game with highest ID automatically for one round
        /// </summary>
        /// <returns></returns>
        public async Task Test()
        {
            //Statistic s = await GetStatistics().ConfigureAwait(false);
            if (games == null)
                games = await GetGames().ConfigureAwait(false);
            await GetGame(games.Last()).ConfigureAwait(false);
            Game g = games.Last();
            if (g.LastRound.Questions != null)
                foreach (QuestionAnswer q in g.LastRound.Questions)
                {
                    q.AnswerPlayer = new Random().Next(4);
                    float? eloChange = await UploadQuestionAnswer(q).ConfigureAwait(false);
                    if (eloChange != null)
                        Console.WriteLine("Elo-Change for this game for " + username + ": " + eloChange);
                    g.EloChange = eloChange;
                }

            try
            {
                await ChooseCategory(g, new Random().Next(1, 4)).ConfigureAwait(false);
            }
            catch (InsufficientPermissionsException e)
            {

            }
        }
    }
}
