using System;
using System.Text;
using System.Xml;

/*
 * Dient zum darstellen der Highscoreliste 
 * */
namespace Space_Assault.States
{
    class HighScoreList : AGameState
    {
        XmlDocument xmlDoc = new XmlDocument();

        public override void Draw()
        {

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml");
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes[2].ChildNodes[0].ChildNodes)
                Console.WriteLine(xmlNode.Attributes["currency"].Value + ": " + xmlNode.Attributes["rate"].Value);
            Console.Read();

        }

        public override void Update()
        {
        }
    }
}
