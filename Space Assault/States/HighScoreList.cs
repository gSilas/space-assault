using System;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

/// <summary>
/// Dient zum laden und schreiben der Highscoreliste 
/// </summary>
/// TODO: Grafische Oberfläche der HighscoreList im Spiel
/// 
namespace Space_Assault.States
{
    internal struct HighscoreEntity : IComparable<HighscoreEntity>
    {
        public string Name { get; set; }
        public int Points { get; set; }

        //methode für IComparable um objekte vergleichen zu können
        public int CompareTo(HighscoreEntity item)
        {
            return Points - item.Points;
        }
    }

    class HighScoreList : IGameState, IUpdateableState, IDrawableState
    {
        int listLength;
        string filePath;
        HighscoreEntity[] scoresList;

        public bool IsStopped { get; set; }

        public void Initialize()
        {
            listLength = 10;
            scoresList = new HighscoreEntity[listLength];
            filePath = "HighScoreList.xml";
        }

        public void LoadContent()
        {
            //wenn die Datei nicht existiert erstelle sie
            if (File.Exists(filePath))
            {
                Load();
            }
            else
            {
                Add("Philipp", 789);
                Add("Daniel", 777);
                Add("Dustin", 888);
                Add("Gerd", 999);
                Add("Hans-Martin", 1337);
                Add("Markus", 1111);
                Add("Arne", 1234);
                Add("Andre", 666);
                Add("Ulrich", 555);
                Add("Acagamic", 1777);
                Save();
            }
        }

        public void Update(GameTime elapsedTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {

            }
        }


        public void Draw(GameTime elapsedTime)
        {
            int zeilenAbstand = 50;
            int spaltenAbstand = 200;
            int spawnPointX = 400;
            int spawnPointY = 100;
            for(int i = 0; i < listLength; i++)
            {
                Global.SpriteBatch.DrawString(Global.Arial, (i+1) + ". Platz", new Vector2(spawnPointX, spawnPointY+i*zeilenAbstand), Color.Black);
                Global.SpriteBatch.DrawString(Global.Arial, scoresList[i].Name , new Vector2(spawnPointX + spaltenAbstand, spawnPointY + i * zeilenAbstand), Color.Black);
                Global.SpriteBatch.DrawString(Global.Arial, (scoresList[i].Points).ToString(), new Vector2(spawnPointX + spaltenAbstand*2, spawnPointY + i * zeilenAbstand), Color.Black);
            }
        }

        //laden der XML und schreiben in scoresList
        public void Load()
        {
            XmlDocument highScoreDoc = new XmlDocument();
            highScoreDoc.Load(filePath);

            scoresList = new HighscoreEntity[highScoreDoc.ChildNodes[0].ChildNodes.Count];
            int i = 0;
            HighscoreEntity currentEntry;
            foreach (XmlNode entry in highScoreDoc.ChildNodes[0].ChildNodes)
            {
                if (!string.IsNullOrEmpty(entry.ChildNodes[0].InnerText))
                {
                    currentEntry = new HighscoreEntity();
                    currentEntry.Name = entry.ChildNodes[0].InnerText;
                    currentEntry.Points = int.Parse(entry.ChildNodes[1].InnerText);
                    scoresList[i] = currentEntry;
                    i++;
                }
            }
        }

        //scoresList in die XML schreiben
        public void Save()
        {
            XmlDocument highScoreDoc = new XmlDocument();
            highScoreDoc.AppendChild(highScoreDoc.CreateElement("Highscore"));

            int i = 1;
            XmlElement curNum;
            XmlElement curName;
            XmlElement curPoints;

            foreach (HighscoreEntity entry in scoresList)
            {
                curNum = highScoreDoc.CreateElement("Platz" + i);
                curName = highScoreDoc.CreateElement("Name");
                curPoints = highScoreDoc.CreateElement("Punkte");

                curName.InnerText = entry.Name;
                curPoints.InnerText = entry.Points.ToString();

                curNum.AppendChild(curName);
                curNum.AppendChild(curPoints);

                highScoreDoc.ChildNodes[0].AppendChild(curNum);

                i++;
            }
            highScoreDoc.Save(filePath);

        }

        //neuen Eintrag hinzufügen
        public void Add(string Name, int Points)
        {

            HighscoreEntity newEntry = new HighscoreEntity();
            newEntry.Name = Name;
            newEntry.Points = Points;

            // neuer eintrag ist besser als letzter platz
            if (scoresList[scoresList.Length - 1].Points < newEntry.Points)
                scoresList[scoresList.Length - 1] = newEntry;

            //liste sortieren
            for (int i = scoresList.Length - 2; i >= 0; i--)
            {
                if (scoresList[i].Points < scoresList[i + 1].Points)
                {
                    HighscoreEntity tempEntity = scoresList[i + 1];
                    scoresList[i + 1] = scoresList[i];
                    scoresList[i] = tempEntity;
                }
            }

            Debug.WriteLine("Entry added");
        }


        /// <summary>
        /// GameState stuff
        /// </summary>
        public void Kill()
        {
            Save();
        }

        public void Resume()
        {
            Load();
        }

        public bool Equals(IGameState other)
        {
            return other.GetType() == this.GetType();
        }

        public bool Equals(IUpdateable other)
        {
            return other.GetType() == this.GetType();
        }

        public bool Equals(IDrawableState other)
        {
            return other.GetType() == this.GetType();
        }
    }
}