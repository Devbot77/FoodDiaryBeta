using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FoodDiaryBeta
{
    [Table("Dates")]
    class DiaryDate
    {
        [PrimaryKey, AutoIncrement]
        public int DateId { get; set; }
        public string Date { get; set; }
        public int Rating { get; set; }
    }
}