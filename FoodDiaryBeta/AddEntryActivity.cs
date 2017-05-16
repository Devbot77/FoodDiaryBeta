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

namespace FoodDiaryBeta
{
    [Activity(Label = "Add Entry")]
    public class AddEntryActivity : Activity
    {
        DiaryRepository repository;
        RadioButton foodRadioButton;
        RadioButton eventRadioButton;
        EditText descriptionText;
        EditText quantityText;
        Button saveButton;
        int dateId;
        string date;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddEntry);

            //dateId = Intent.GetIntExtra("DateId", 0);
            Bundle extras = Intent.GetBundleExtra("DATE_INFO");
            dateId = extras.GetInt("DATE_ID");
            date = extras.GetString("DATE_STRING");
            repository = new DiaryRepository();

            foodRadioButton = FindViewById<RadioButton>(Resource.Id.foodRadioButton);
            eventRadioButton = FindViewById<RadioButton>(Resource.Id.eventRadioButton);
            descriptionText = FindViewById<EditText>(Resource.Id.descriptionText);
            quantityText = FindViewById<EditText>(Resource.Id.quantityText);
            saveButton = FindViewById<Button>(Resource.Id.saveButton);

            saveButton.Click += saveButton_Click;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            // Check to see if DiaryDate exists for date of this entry. If not, create one
            //DiaryDate diaryDate = await repository.GetDateByString(date);
            //if (diaryDate == null)
            //{
            //    diaryDate.Date = date;
            //    await repository.AddDate(diaryDate);
            //}
            if (dateId == 0)
            {
                DiaryDate diaryDate = new DiaryDate();
                diaryDate.Date = date;
                await repository.AddDate(diaryDate);

                // reassign dateId
                diaryDate = await repository.GetDateByString(date);
                dateId = diaryDate.DateId;
            }

            // Assign new entry details
            Entry entry = new Entry();
            entry.EntryDateId = dateId;
            entry.Description = descriptionText.Text;
            entry.Quantity = Convert.ToInt32(quantityText.Text);
            if (foodRadioButton.Checked)
                entry.EntryType = 0;
            else if (eventRadioButton.Checked)
                entry.EntryType = 1;

            // Insert new entry and finish activity
            await repository.AddEntry(entry);
            Toast.MakeText(this, "Entry added!", ToastLength.Short).Show();
            Finish();
        }
    }
}