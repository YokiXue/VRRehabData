using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public static class CSVManager
{
    private static string reportDirectoryName = "Report";
    private static string reportFileName = "report.csv";
    private static string reportSeparator = ";";
    public static string[] reportHeaders { get; set; } = new string[3]  // This format is made so only one spline can be saved at a time, TBD how to work around this
    {
        "Spline nr.",
        "Radius",
        "Knot 0"
    };

    //private static List<string> reportHeaders = new List<string>() // Suggestion for formatting, might have to change to a list? Or method for updating array when needed
    //{
    //    "Spline nr.",
    //    "Radius",
    //    "Something else idk",
    //    "Knot 0"
    //};

    private static string timeStampHeader = "Time Stamp"; // TODO: could be changed to something else

    #region Interactions
    public static void AppendToReport(string[] strings)
    {
        VerifyDirectory();
        VerifyFile();

        using(StreamWriter sw = File.AppendText(GetFilePath()))
        {
            string finalString = "";

            for (int i = 0; i < strings.Length; i++)
            {
                if (finalString != "")
                    finalString += reportSeparator;

                finalString += strings[i];
            }

            finalString += reportSeparator + GetTimeStamp();

            sw.WriteLine(finalString);
        }
    }

    public static void CreateReport()
    {
        VerifyDirectory();

        using (StreamWriter sw = File.CreateText(GetFilePath()))
        {
            string finalString = "";

            // Writes all the headers to one string
            for (int i = 0; i < reportHeaders.Length; i++)
            {
                if (finalString != "")
                    finalString += reportSeparator;

                finalString += reportHeaders[i];
            }

            finalString += reportSeparator + timeStampHeader;

            sw.WriteLine(finalString);
        }
    }
    #endregion

    #region Operations
    private static void VerifyDirectory()
    {
        string dir = GetDirectoryPath();

        // Checks if directory exists, creates one if not
        if (!Directory.Exists(dir)) 
            Directory.CreateDirectory(dir);
    }

    private static void VerifyFile()
    {
        string file = GetFilePath();

        if (!File.Exists(file))
            CreateReport();
    }
    #endregion

    #region Queries
    private static string GetDirectoryPath()
    {
        return Application.dataPath + "/" + reportDirectoryName;
    }

    private static string GetFilePath()
    {
        return GetDirectoryPath() + "/" + reportFileName;
    }

    private static string GetTimeStamp()
    {
        return System.DateTime.UtcNow.ToString();
    }
    #endregion

    public static void ExpandHeadersArray(string[] newAppendArray)
    {

    }
}
