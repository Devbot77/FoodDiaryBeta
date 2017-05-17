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
        TextView timeDisplay;
        Button timeButton;
        //TimePicker timePicker;
        EditText descriptionText;
        EditText quantityText;
        Button saveButton;
        int dateId;
        string date;
        int hour;
        int minute;
        const int TIME_DIALOG_ID = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AddEntry);

            //dateId = Intent.GetIntExtra("DateId", 0);
            Bundle extras = Intent.GetBundleExtra("DATE_INFO");
            dateId = extras.GetInt("DATE_ID");
            date = extras.GetString("DATE_STRING");
            repository = new DiaryRepository();
            // Get current time
            hour = DateTime.Now.Hour;
            minute = DateTime.Now.Minute;
            

            foodRadioButton = FindViewById<RadioButton>(Resource.Id.foodRadioButton);
            eventRadioButton = FindViewById<RadioButton>(Resource.Id.eventRadioButton);
            timeDisplay = FindViewById<TextView>(Resource.Id.timeDisplay);
            timeButton = FindViewById<Button>(Resource.Id.timeButton);
            descriptionText = FindViewById<EditText>(Resource.Id.descriptionText);
            quantityText = FindViewById<EditText>(Resource.Id.quantityText);
            saveButton = FindViewById<Button>(Resource.Id.saveButton);

            timeButton.Click += (o, e) => ShowDialog(TIME_DIALOG_ID);
            saveButton.Click += saveButton_Click;

            //DateTime fullDate = DateTime.ParseExact(date.PadLeft(10, Convert.ToChar("0")) + " " + hour + ":" + minute, "MM/dd/yyyy HH:mm", null);
            UpdateTimeDisplay();
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
            // build full date
            DateTime fullDate = DateTime.ParseExact(date.PadLeft(10, Convert.ToChar("0")) + " " + hour + ":" + minute, "MM/dd/yyyy HH:mm", null);

            // Assign new entry details
            Entry entry = new Entry();
            entry.EntryDate = fullDate;
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

        private void UpdateTimeDisplay()
        {
            string period = "AM";
            int twelveHour = 12;
            if (hour > 12)
            {
                twelveHour = hour - 12;
                period = "PM";
            }
            else if (hour < 12 && hour != 0)
                twelveHour = hour;
            else if (hour == 12)
                period = "PM";

            string time = string.Format("{0}:{1} {2}", twelveHour, minute.ToString().PadLeft(2, '0'), period);
            timeDisplay.Text = time;
        }

        private void TimePickerCallback (object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            hour = e.HourOfDay;
            minute = e.Minute;
            UpdateTimeDisplay();
        }

        protected override Dialog OnCreateDialog(int id)
        {
            if (id == TIME_DIALOG_ID)
                return new TimePickerDialog(this, TimePickerCallback, hour, minute, false);

            return null;
        }
    }
}