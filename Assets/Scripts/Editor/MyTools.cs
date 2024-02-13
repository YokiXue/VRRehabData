using UnityEditor;
using UnityEngine;

public static class MyTools
{
    private static int knotNr = 0;

    [MenuItem("My Tools/1. Add Defaults to Report %F1")]
    private static void DEV_AppendDefaultsToReport()
    {
        CSVManager.AppendToReport(
            new string[2]
            {
                knotNr.ToString(),
                Random.Range(0,100).ToString() + "," +
                Random.Range(0,100).ToString() + "," + 
                Random.Range(0,100).ToString()
            });

        knotNr++;

        EditorApplication.Beep();
        Debug.Log("<color=green>Report updated succesfully</color>");

    }

    [MenuItem("My Tools/2. Reset Report %F12")]
    private static void DEV_ResetReport()
    {
        knotNr = 0;

        CSVManager.CreateReport();
        EditorApplication.Beep();
        Debug.Log("<color=orange>The report has been reset</color>");

    }

    public static void DEV_AppendSpecificsToReport(string[] strings)
    {
        //if (strings.Length > CSVManager.reportHeaders.Length)
        //    CSVManager.ExpandHeadersArray(strings);

        CSVManager.AppendToReport(strings);

        EditorApplication.Beep();
        Debug.Log("<color=green>Report updated succesfully</color>");

    }
}
