using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.IO;
using System.Net;


/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/


namespace ConsoleApp1
{


    public class Program
    {
        public static string xmlURL = "https://will-bergman.github.io/CSE445_a4/Hotels.xml"; // Q1.2
        public static string xmlErrorURL = "https://will-bergman.github.io/CSE445_a4/HotelsErrors.xml"; // Q1.3
        public static string xsdURL = "https://will-bergman.github.io/CSE445_a4/Hotels.xsd"; // Q1.1

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);


            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);


            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            try
            {
                // Download XSD file and XML file from URLs
                XmlSchemaSet schemas = new XmlSchemaSet();
                XmlReaderSettings settings = new XmlReaderSettings();

                // Load XSD schema
                XmlReader xsdReader = XmlReader.Create(xsdUrl);
                schemas.Add(null, xsdReader);

                // Set validation settings
                settings.Schemas = schemas;
                settings.ValidationType = ValidationType.Schema;

                // Handle validation events
                settings.ValidationEventHandler += (sender, e) =>
                {
                    if (e.Severity == XmlSeverityType.Error || e.Severity == XmlSeverityType.Warning)
                    {
                        throw new Exception($"Error: {e.Message} at {e.Exception.LineNumber}:{e.Exception.LinePosition}");
                    }
                };

                // Validate the XML
                XmlReader xmlReader = XmlReader.Create(xmlUrl, settings);
                while (xmlReader.Read()) { }

                return "No Error";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

            //return "No Error" if XML is valid. Otherwise, return the desired exception message.
        }

        public static string Xml2Json(string xmlUrl)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    // Download XML file
                    string xmlContent = client.DownloadString(xmlUrl);

                    // Convert XML to JSON using Newtonsoft.Json
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(xmlContent);

                    // Create root element to match the expected format
                    XmlElement root = xmlDoc.DocumentElement;

                    // Serialize the XML to JSON
                    string jsonText = JsonConvert.SerializeXmlNode(root, Newtonsoft.Json.Formatting.Indented, true);

                    // Confirm validity by deserialization
                    JsonConvert.DeserializeXmlNode(jsonText);

                    return jsonText;
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }

            // The returned jsonText needs to be de-serializable by Newtonsoft.Json package. (JsonConvert.DeserializeXmlNode(jsonText))
        }
    }

}
