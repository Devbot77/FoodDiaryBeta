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
    public class Entry
    {
        [PrimaryKey, AutoIncrement]
        public int EntryId { get; set; }
        //public string EntryDate { get; set; }
        public DateTime EntryDate { get; set; }
        public int EntryDateId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public int EntryType { get; set; }  // 0=Food Entry, 1=Event Entry
    }
}