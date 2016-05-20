using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;


/// <summary>
/// Dient zum laden und schreiben der Highscoreliste 
/// </summary>
/// TODO: HighScoreList.xml erstellen, falls nicht vorhanden
/// TODO: Möglichkeit des Eintrages in die Liste über das SPiel
/// TODO: grafisches Darstellen der highscorelist im Spiel
/// 
namespace Space_Assault.States
{
    public struct HighscoreEntity : IComparable<HighscoreEntity>
    {

        public string Name { get; set; }
        public int Points { get; set; }

        /*
        return < 0 , davor
        return ==0, gleiche posi
        return > 0 , danach
        */
        public int CompareTo(HighscoreEntity item)
        {
            return Points - item.Points;
        }
    }

    class HighScoreList : AGameState
    {
        int listLength = 10;
        string filename = "HighScoreList.xml";
        HighscoreEntity[] scoresList;

        /// <summary>
        /// 
        /// </summary>
        public override void Draw()
        {
            foreach (HighscoreEntity x in scoresList)
            {
                Console.WriteLine(x.Name + " ____ " + x.Points);
            }
        }


        public override void Update()
        {
        }

        public HighScoreList()
        {

            scoresList = new HighscoreEntity[listLength];

            for (int i = 1; i < 20; i++)
            {
                this.Add("Peter", i * 250);
            }
            Save();
        }

        //laden der XML und schreiben in scoresList
        public void Load()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);

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
            doc.Save(filename);
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