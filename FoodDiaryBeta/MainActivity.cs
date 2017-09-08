using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace FoodDiaryBeta
{
    [Activity(Label = "FoodDiaryBeta", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            //this method neest to be called before any database calls ca be made!
            //DatabaseManager mydata = new DatabaseManager();

            //get diary button by ad and assign click method to the click event for that button
            Button analysisButton = FindViewById<Button>(Resource.Id.analysisButton);
            Button diaryButton = FindViewById<Button>(Resource.Id.diaryButton);
            analysisButton.Click += analysisButton_Click;
            diaryButton.Click += diaryButton_Click;

        }

        //go to diary activity on diary button click
        private void diaryButton_Click(object sender, EventArgs e)
        {
            var diaryIntent = new Intent(this, typeof(DiaryActivity));
            // Pass current date along to diary activity
            //diaryIntent.PutExtra("Date", DateTime.Today.ToShortDateString());
            StartActivity(diaryIntent);
        }

        private void analysisButton_Click(object sender, EventArgs e)
        {
            var analysisIntent = new Intent(this, typeof(AnalysisActivity));
            StartActivity(analysisIntent);
        }
    }
}

