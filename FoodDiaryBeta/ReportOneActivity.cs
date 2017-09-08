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
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Xamarin.Android;

namespace FoodDiaryBeta
{
    [Activity(Label = "ReportOneActivity")]
    public class ReportOneActivity : Activity
    {
        // demo chart following https://poojargaonkar.wordpress.com/2015/02/17/plotting-graphs-in-xamarin-android-using-oxyplot/

        PlotView plotViewModel;
        LinearLayout layoutModel;
        public PlotModel myModel { get; set; }

        private int[] modelAllocValues = new int[] { 12, 5, 2, 40, 40, 1 };
        private string[] modelAllocations = new string[] { "Slice1", "Slice2", "Slice3", "Slice4", "Slice5", "Slice6" };
        string[] colors = new string[] { "#7DA137", "#6EA6F3", "#999999", "#3B8DA5", "#F0BA22", "#EC8542" };
        int total = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ReportOne);

            // Create your application here
            plotViewModel = FindViewById<PlotView>(Resource.Id.plot_view);
            layoutModel = FindViewById<LinearLayout>(Resource.Id.linearLayoutModel);

            //Model Allocation Pie Chart
            var plotModel2 = new PlotModel();
            var pieSeries2 = new PieSeries();
            pieSeries2.InsideLabelPosition = 0.0;
            pieSeries2.InsideLabelFormat = null;

            for (int i = 0; i < modelAllocations.Length && i < modelAllocValues.Length && i < colors.Length; i++)
            {
                pieSeries2.Slices.Add(new PieSlice(modelAllocations[i], modelAllocValues[i]) { Fill = OxyColor.Parse(colors[i]) });
                pieSeries2.OutsideLabelFormat = null;

                double mValue = modelAllocValues[i];
                double percentValue = (mValue / total) * 100;
                string percent = percentValue.ToString("#.##");

                // Add horizontal layout for titles and colors of slices
                LinearLayout hLayout = new LinearLayout(this);
                hLayout.Orientation = Android.Widget.Orientation.Horizontal;
                LinearLayout.LayoutParams param = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
                hLayout.LayoutParameters = param;

                //Add views with colors
                LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(15, 15);

                View mView = new View(this);
                lp.TopMargin = 5;
                mView.LayoutParameters = lp;
                mView.SetBackgroundColor(Android.Graphics.Color.ParseColor(colors[i]));

                //Add titles
                TextView label = new TextView(this);
                label.TextSize = 10;
                label.SetTextColor(Android.Graphics.Color.Black);
                label.Text = string.Join(" ", modelAllocations[i]);
                param.LeftMargin = 8;
                label.LayoutParameters = param;

                hLayout.AddView(mView);
                hLayout.AddView(label);
                layoutModel.AddView(hLayout);
            }

            plotModel2.Series.Add(pieSeries2);
            myModel = plotModel2;
            plotViewModel.Model = myModel;
        }
    }
}