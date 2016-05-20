using System;
using System.Xml;
using System.IO;


/// <summary>
/// Dient zum laden und schreiben der Highscoreliste 
/// </summary>
/// TODO: Grafische Oberfläche der HighscoreList im Spiel
/// 
namespace Space_Assault.States
{
    public struct HighscoreEntity : IComparable<HighscoreEntity>
    {
        public string Name { get; set; }
        public int Points { get; set; }

        //methode für IComparable um objekte vergleichen zu können
        public int CompareTo(HighscoreEntity item)
        {
            return Points - item.Points;
        }
    }

    class HighScoreList : AGameState
    {
        int listLength = 10;
        string filePath = "HighScoreList.xml";
        HighscoreEntity[] scoresList;

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
        }

        public override void Update()
        {
        }

        public HighScoreList()
        {
            scoresList = new HighscoreEntity[listLength];
            //wenn die Datei nicht existiert erstelle eine
            if (!File.Exists(filePath))
            {
                Add("Philipp", 666);
                Add("Daniel", 777);
                Add("Dustin", 888);
                Add("Gerd", 999);
                Add("Hans-Martin", 1337);
                Save();
            }
            else
            {
                Load();
            }

            foreach (HighscoreEntity x in scoresList)
            {
                Console.WriteLine(x.Name + " " + x.Points);
            }

        }

        //laden der XML und schreiben in scoresList
        public void Load()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);

            int count = 0;
            scoresList = new HighscoreEntity[doc.ChildNodes[0].ChildNodes.Count];
            foreach (XmlNode platz in doc.ChildNodes[0].ChildNodes)
            {
                if (!string.IsNullOrEmpty(platz.ChildNodes[0].InnerText))
                {
                    //ein neues Item erzeugen, wenn der Spielername nicht null oder leer ist
                    HighscoreEntity i = new HighscoreEntity();
                    i.Name = platz.ChildNodes[0].InnerText;
                    i.Points = int.Parse(platz.ChildNodes[1].InnerText);
                    this.scoresList[count] = i;
                    count++;
                }
            }
            Console.WriteLine("File Loaded");
        }

        //scoresList in die XML schreiben
        public void Save()
        {
            XmlDocument doc = new XmlDocument();
            //root-node
            doc.AppendChild(doc.CreateElement("Highscore"));
            int platzNum = 1;
            foreach (HighscoreEntity i in scoresList)
            {
                XmlElement num = doc.CreateElement("Platz" + platzNum);

                XmlElement name = doc.CreateElement("Name");
                name.InnerText = i.Name;

                XmlElement points = doc.CreateElement("Punkte");
                points.InnerText = i.Points.ToString();

                num.AppendChild(name);
                num.AppendChild(points);

                platzNum++;
                doc.ChildNodes[0].AppendChild(num);
            }
            doc.Save(filePath);
            Console.WriteLine("File Saved");
        }

        //neue Eintrag hinzufügen
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

            Console.WriteLine("Entry added");
        }
    }
}