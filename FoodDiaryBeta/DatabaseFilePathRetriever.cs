using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
//using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FoodDiaryBeta
{
    public static class DatabaseFilePathRetriever
    {
        public static string GetPath()
        {
            const string filename = "FoodDiaryDB.db3";
            var documentPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentPath, filename);
            return path;
        }
    }
}