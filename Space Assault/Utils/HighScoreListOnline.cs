using System;
using System.Text;
using System.Net;
using System.Security.Cryptography;
using System.Collections.Specialized;
using System.Threading;
using System.Linq;

/// <summary>
/// Using:
/// https://github.com/omgwtfgames/omgleaderboards
/// </summary>
namespace SpaceAssault.Utils
{

    public class HighScoreListOnline
    {

        private string _gameName = "spaceAssault";
        private string _addUrl = "https://omgleaderboards.appspot.com/add";
        private string _getUrl = "http://omgleaderboards.appspot.com/get/";

        public string[,] _scoresList;

        public bool isReachable = true;

        public HighScoreListOnline()
        {
            _getUrl += _gameName + "?timeframes=alltime&limit=10";
            int _listLength = 10;
            _scoresList = new string[_listLength, 2];
            for (int i = 0; i < _listLength; i++)
            {
                _scoresList[i, 0] = "  -";
                _scoresList[i, 1] = "    -";
            }
            Thread t1 = new Thread(new ThreadStart(getLeaderboard));
            t1.Start();
        }

        public void getLeaderboard()
        {
            var wb = new WebClient();
            string response = "";
            isReachable = true;
            try
            {
                response = wb.DownloadString(_getUrl);
            }
            catch (System.Net.WebException e)
            {
                isReachable = false;
            }
            char[] seperators = { '{', '}', '[', ']' };
            string[] split = response.Split(seperators, StringSplitOptions.RemoveEmptyEntries);

            //2,4,6,8,10,12,14,16,18,20 (2*10 = 20)
            for (int i = 2; i < split.Length; i += 2)
            {
                string[] splitScnd = split[i].Split(',');
                string[] score = splitScnd[2].Split(':', '"', ' ');
                string[] name = splitScnd[4].Split(':', '"', ' ');
                _scoresList[i / 2 - 1, 0] = name[6].Replace('#',' ');
                int numCharsMissing = 7 - score[5].Length;
                if (numCharsMissing < 0) numCharsMissing = 0;
                _scoresList[i / 2 - 1, 1] = String.Concat(Enumerable.Repeat(" ", numCharsMissing)) + score[5];

            }
        }

        public void addScore(string name)
        {
            var wb = new WebClient();
            var data = new NameValueCollection();
            name = name.Replace(' ', '#');
            data["id"] = _gameName;
            data["nickname"] = name;
            data["score"] = Global.HighScorePoints.ToString();
            //game_id + str(score) + nickname + platform + extra + secret_salt
            data["hash"] = Md5Sum(_gameName + Global.HighScorePoints.ToString() + name + "BK8566xJ6mUid18M97Qowm78ap39T4J3");
            byte[] response;
            try
            {
                response = wb.UploadValues(_addUrl, "POST", data);
                //Console.WriteLine(Encoding.ASCII.GetString(response));
            }
            catch (System.Net.WebException e)
            {
                isReachable = false;
            }
            //var responseCon = Encoding.ASCII.GetString(response);
            Thread t1 = new Thread(new ThreadStart(getLeaderboard));
            t1.Start();
        }


        public string Md5Sum(string strToEncrypt)
        {
            UTF8Encoding ue = new UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);

            // encrypt bytes
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }

            return hashString.PadLeft(32, '0');
        }
    }
}
