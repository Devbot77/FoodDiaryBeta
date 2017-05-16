using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;
using Android.Database.Sqlite;

namespace FoodDiaryBeta
{
    [Activity(Label = "Diary")]
    public class DiaryActivity : Activity
    {
        private DiaryRepository repository;

        TextView dateText;
        Button dateButton;
        RatingBar ratingBar;
        Button rateButton;
        Button insertButton;
        Button entryButton;
        ListView entryTable;
        string date;
        int dateId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Diary);

            repository = new DiaryRepository();

            dateText = FindViewById<TextView>(Resource.Id.dateText);
            dateButton = FindViewById<Button>(Resource.Id.dateButton);
            ratingBar = FindViewById<RatingBar>(Resource.Id.ratingBar);
            rateButton = FindViewById<Button>(Resource.Id.rateButton);
            insertButton = FindViewById<Button>(Resource.Id.insertButton);
            entryButton = FindViewById<Button>(Resource.Id.getEntryButton);
            entryTable = FindViewById<ListView>(Resource.Id.entryList);

            dateText.Text = DateTime.Today.ToLongDateString();
            date = DateTime.Today.ToShortDateString();
            SetDateId(date);

            dateButton.Click += (sender, e) =>
            {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnDateSet, today.Year, today.Month - 1, today.Day);
                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };

            rateButton.Click += rateButton_Click;
            insertButton.Click += insertButton_Click2;
            entryButton.Click += entryButton_Click;
            entryTable.ItemClick += ListItemClicked;

            //refreshList();
            refreshList(dateId);  // call this in OnStart method instead
        }

        //protected override void OnStart()     // currently throws exception about not calling through to super.OnStart or something
        //{
        //    // Refresh the list view when first starting this activity or coming back to it 
        //    // after it was backgrounded (like when user adds or edits entry)
        //    refreshList(dateId);
        //}

        private void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            dateText.Text = e.Date.ToLongDateString();
            date = e.Date.ToShortDateString();
            SetDateId(date);
            refreshList(dateId);
        }

        //private async void insertButton_Click(object sender, EventArgs e)
        //{
        //    var newEntry = new Entry
        //    {
        //        EntryDate = DateTime.Today.ToShortDateString(),
        //        Description = entryText.Text
        //    };
        //    await repository.AddEntry(newEntry);
        //    Console.WriteLine("Entry added");
        //    refreshList();
        //    entryText.Text = "";
        //    Toast.MakeText(this, "Entry added, sucka!!!", ToastLength.Short).Show();
        //}

        private async void rateButton_Click(object sender, EventArgs e)
        {
            if (dateId == 0)
            {
                Toast.MakeText(this, "No entries exist for this date!", ToastLength.Short).Show();
            }
            else
            {
                DiaryDate diaryDate = new DiaryDate();
                diaryDate.DateId = dateId;
                diaryDate.Date = date;
                diaryDate.Rating = (int)ratingBar.Rating;

                try
                {
                    await repository.UpdateDate(diaryDate);
                }
                catch (SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private void insertButton_Click2(object sender, EventArgs e)
        {
            //string newEntryDate = date; // Convert.ToDateTime(date);
            var AddEntryIntent = new Intent(this, typeof(AddEntryActivity));
            // Pass current diary date along to the add entry activity
            //AddEntryIntent.PutExtra("Date", newEntryDate);
            Bundle extras = new Bundle();
            extras.PutInt("DATE_ID", dateId);
            extras.PutString("DATE_STRING", date);
            AddEntryIntent.PutExtra("DATE_INFO",extras);
            //AddEntryIntent.PutExtra("DateId", dateId);
            StartActivity(AddEntryIntent);

        }

        private void entryButton_Click(object sender, EventArgs e)
        {
            //var entryList = await repository.GetEntryStrings();
            //entryTable.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, entryList);
            //Console.WriteLine(entryList);

            //var entryList = await repository.GetEntries();
            //entryTable.Adapter = new EntryListAdapter(this, entryList);
            SetDateId(date);
            refreshList(dateId);
        }

        private async void refreshList()
        {
            // the method below uses a string array to populate the listview
            //var entryList = await repository.GetEntryStrings();
            ////ListView entryTable = FindViewById<ListView>(Resource.Id.entryList);
            //entryTable.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, entryList);

            //this method populates the listview with actual entries of the 'Entry' datatype
            var entryList = await repository.GetEntries();
            entryTable.Adapter = new EntryListAdapter(this, entryList);
        }

        private async void refreshList(int dateId)
        {
            // Create list of all entries by specified date
            //if (dateId == 0)
            //    return;
            var entryList = await repository.GetEntriesByDate(dateId);
            entryTable.Adapter = new EntryListAdapter(this, entryList);
        }

        private void ListItemClicked(object sender, AdapterView.ItemClickEventArgs e)
        {
            //this method of getting the entry and displaying a toast works, however it does not seem to grab the actual entryID in the first line?
            // it appears to grab the position of the item in the listview which does not correspond to the actual entryID. Works for now, but will be broken
            // as soon as items are deleted or otherwise displayed in an order different from the table ids.
            //var entryID = this.entryTable.Adapter.GetItemId(e.Position);
            // Entry entry = await repository.GetEntry((int)entryID + 1);

            // The below code makes use of a custom class to convert the data item from Java.Lang.Object to Entry type
            Entry entry = entryTable.Adapter.GetItem(e.Position).Cast<Entry>();
            //var newEntryID = entry.EntryId;

            //Display dialog to edit or delete selected entry and take appropriate steps
            if (entry == null)
            {
                Toast.MakeText(this, "Entry is null!", ToastLength.Short).Show();
            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("Edit or Delete?");
                //alert.SetMessage("Edit or Delete?");
                alert.SetPositiveButton("Edit", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Edit!", ToastLength.Short).Show();
                    // Start edit details activity?
                    var EntryIntent = new Intent(this, typeof(EntryActivity));
                    // Pass entryID along to the activity
                    //EntryIntent.PutExtra("Entry", entry.EntryId);
                    Bundle extras = new Bundle();
                    extras.PutInt("ENTRY_ID", entry.EntryId);
                    extras.PutString("DATE_STRING", date);
                    EntryIntent.PutExtra("ENTRY_INFO", extras);
                    StartActivity(EntryIntent);
                });

                alert.SetNegativeButton("Delete", async (senderAlert, args) =>
                {

                    await repository.DeleteEntry(entry);
                    Toast.MakeText(this, entry.Description + " deleted!", ToastLength.Short).Show();
                    refreshList();
                });

                Dialog dialog = alert.Create();
                dialog.Show();
            };
        }

        private void SetDateId(string s)
        {
            DiaryDate diaryDate = new DiaryDate();
            var task = Task.Run(async () =>
            {
                diaryDate = await repository.GetDateByString(s);
            });
            task.Wait();
            if (diaryDate != null)
            {
                dateId = diaryDate.DateId;
                // set date rating if in accepted range
                if (diaryDate.Rating >= 1 && diaryDate.Rating <= 5)
                    ratingBar.Rating = (float)diaryDate.Rating;
                else
                    ratingBar.Rating = 0F;
            }
            else
            {
                dateId = 0;
                ratingBar.Rating = 0F;
            }


        }

    }
}