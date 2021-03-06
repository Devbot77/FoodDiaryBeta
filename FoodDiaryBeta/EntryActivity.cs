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
    [Activity(Label = "Edit Entry Details")]
    public class EntryActivity : Activity
    {
        private DiaryRepository repository;

        RadioButton foodRadiobutton;
        RadioButton eventRadioButton;
        TextView timeDisplay;
        Button timeButton;
        EditText descriptionText;
        //DatePicker calendarDatePicker;
        //NumberPicker quantityPicker;
        EditText quantityText;
        Button saveEntryButton;
        Entry entry;
        string date;
        int hour;
        int minute;
        const int TIME_DIALOG_ID = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Entry);
            repository = new DiaryRepository();

            //int entryID = Intent.GetIntExtra("Entry", 0);
            Bundle extras = Intent.GetBundleExtra("ENTRY_INFO");
            int entryId = extras.GetInt("ENTRY_ID");
            date = extras.GetString("DATE_STRING");

            foodRadiobutton = FindViewById<RadioButton>(Resource.Id.foodRadioButton);
            eventRadioButton = FindViewById<RadioButton>(Resource.Id.eventRadioButton);
            timeDisplay = FindViewById<TextView>(Resource.Id.timeDisplay);
            timeButton = FindViewById<Button>(Resource.Id.timeButton);
            //calendarDatePicker = FindViewById<DatePicker>(Resource.Id.datePicker1);
            descriptionText = FindViewById<EditText>(Resource.Id.descriptionEditText);
            //quantityPicker = FindViewById<NumberPicker>(Resource.Id.quantityPicker);
            quantityText = FindViewById<EditText>(Resource.Id.quantityText);
            saveEntryButton = FindViewById<Button>(Resource.Id.saveButton);

            timeButton.Click += (o, e) => ShowDialog(TIME_DIALOG_ID);
            saveEntryButton.Click += saveEntryButton_Click;
            //calendarDatePicker.Click += calendarDatePicker_Click;
            getEntryDetails(entryId);
        }

        private async void getEntryDetails(int id)
        {
            entry = await repository.GetEntry(id);

            //calendarDatePicker.DateTime = Convert.ToDateTime(date);
            hour = entry.EntryDate.Hour;
            minute = entry.EntryDate.Minute;
            UpdateTimeDisplay();

            descriptionText.Text = entry.Description;
            quantityText.Text = entry.Quantity.ToString();

            if (entry.EntryType == 0)
                foodRadiobutton.Checked = true;
            else if (entry.EntryType == 1)
                eventRadioButton.Checked = true;

        }

        private async void saveEntryButton_Click(object sender, EventArgs e)
        {
            // Implement update entry function
            //entry.EntryDate = calendarDatePicker.DateTime.ToShortDateString();
            DateTime fullDate = DateTime.ParseExact(date.PadLeft(10, Convert.ToChar("0")) + " " + hour + ":" + minute, "MM/dd/yyyy HH:mm", null);
            entry.EntryDate = fullDate;
            entry.Description = descriptionText.Text;
            entry.Quantity = Convert.ToInt32(quantityText.Text);
            if (foodRadiobutton.Checked)
                entry.EntryType = 0;
            else if (eventRadioButton.Checked)
                entry.EntryType = 1;

            await repository.UpdateEntry(entry);
            Toast.MakeText(this, "Entry updated!", ToastLength.Short).Show();
            Finish();
        }

        //private void calendarDatePicker_Click(object sender, EventArgs e)
        //{
        //    DateTime current = calendarDatePicker.DateTime;
        //    DatePickerDialog dialog = new DatePickerDialog(this, OnDateSet, current.Year, current.Month - 1, current.Day);
        //    dialog.DatePicker.MinDate = current.Millisecond;
        //    dialog.Show();
        //}

        //private void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        //{
        //    calendarDatePicker.DateTime = e.Date;
        //}

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

        private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
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