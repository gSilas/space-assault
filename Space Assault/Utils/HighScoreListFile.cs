﻿using System;
using System.Xml;
using System.IO;
using System.Diagnostics;

namespace SpaceAssault.Utils
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

    public class HighScoreListFile
    {
        public string _filePath;
        public HighscoreEntity[] _scoresList;

        public HighScoreListFile()
        {
            int _listLength = 10;
            _scoresList = new HighscoreEntity[_listLength];

            _filePath = "HighScoreList.xml";

            //wenn die Datei nicht existiert erstelle sie
            if (File.Exists(_filePath))
            {
                LoadFile();
            }
            else
            {
                Add("Philipp", 50);
                Add("Daniel", 50);
                Add("Dustin", 50);
                Add("Hans-Martin", 50);
                Add("Acagamic", 550);
                Add("Major Tom", 201);
                Add("Han Solo", 199);
                Add("Cpt. Picard", 200);
                Add("Ripley", 180);
                Add("Kapt'n Balu", 10);
            }
        }

        //laden der XML und schreiben in scoresList
        public void LoadFile()
        {
            XmlDocument highScoreDoc = new XmlDocument();
            highScoreDoc.Load(_filePath);

            _scoresList = new HighscoreEntity[highScoreDoc.ChildNodes[0].ChildNodes.Count];
            int i = 0;
            HighscoreEntity currentEntry;
            foreach (XmlNode entry in highScoreDoc.ChildNodes[0].ChildNodes)
            {
                if (!string.IsNullOrEmpty(entry.ChildNodes[0].InnerText))
                {
                    currentEntry = new HighscoreEntity();
                    currentEntry.Name = entry.ChildNodes[0].InnerText;
                    currentEntry.Points = int.Parse(entry.ChildNodes[1].InnerText);
                    _scoresList[i] = currentEntry;
                    i++;
                }
            }
        }

        //scoresList in die XML schreiben
        public void SaveFile()
        {
            XmlDocument highScoreDoc = new XmlDocument();
            highScoreDoc.AppendChild(highScoreDoc.CreateElement("Highscore"));

            int i = 1;
            XmlElement curNum, curName, curPoints;

            foreach (HighscoreEntity entry in _scoresList)
            {
                curNum = highScoreDoc.CreateElement("Place" + i);
                curName = highScoreDoc.CreateElement("Name");
                curPoints = highScoreDoc.CreateElement("Points");

                curName.InnerText = entry.Name;
                curPoints.InnerText = entry.Points.ToString();

                curNum.AppendChild(curName);
                curNum.AppendChild(curPoints);

                highScoreDoc.ChildNodes[0].AppendChild(curNum);

                i++;
            }
            highScoreDoc.Save(_filePath);

        }

        public void Add(string Name, int Points)
        {

            HighscoreEntity newEntry = new HighscoreEntity();
            newEntry.Name = Name;
            newEntry.Points = Points;

            //neuer eintrag ist besser als letzter eintrag
            if (_scoresList[_scoresList.Length - 1].Points < newEntry.Points)
                _scoresList[_scoresList.Length - 1] = newEntry;

            //liste sortieren
            for (int i = _scoresList.Length - 2; i >= 0; i--)
            {
                if (_scoresList[i].Points < _scoresList[i + 1].Points)
                {
                    HighscoreEntity tempEntity = _scoresList[i + 1];
                    _scoresList[i + 1] = _scoresList[i];
                    _scoresList[i] = tempEntity;
                }
            }
            SaveFile();
        }
    }
}
