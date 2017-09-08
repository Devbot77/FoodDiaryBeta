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
    [Activity(Label = "AnalysisActivity")]
    public class AnalysisActivity : Activity
    {
        Button reportOneButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Analysis);
            // Create your application here
            reportOneButton = FindViewById<Button>(Resource.Id.reportOneButton);

            reportOneButton.Click += reportOneButton_Click;
        }

        private void reportOneButton_Click(object sender, EventArgs e)
        {
            var reportOneIntent = new Intent(this, typeof(ReportOneActivity));
            StartActivity(reportOneIntent);
        }
    }
}