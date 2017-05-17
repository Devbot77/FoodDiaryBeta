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
    public class EntryListAdapter : BaseAdapter<Entry>
    {
        Activity context = null;
        IList<Entry> entries = new List<Entry>();

        public EntryListAdapter(Activity context, IList<Entry> entries) : base()
        {
            this.context = context;
            this.entries = entries;
        }

        public override Entry this[int position]
        {
            get { return entries[position]; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override int Count
        {
            get
            {
                return entries.Count;
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //var entry = entries[position];
            View view = convertView;

            //re-use an existing view, if one is available, otherwise create one
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.ListRowLayout, parent, false);

            Entry entry = this.entries[position];
            view.FindViewById<TextView>(Resource.Id.Description).Text = entry.Description;
            view.FindViewById<TextView>(Resource.Id.Quantity).Text = entry.Quantity.ToString();
            view.FindViewById<TextView>(Resource.Id.entryDate).Text = entry.EntryDate.ToShortTimeString();

            //example from https://developer.xamarin.com/guides/android/user_interface/working_with_listviews_and_adapters/part_2_-_populating_a_listview_with_data/
            //var view = (convertView ??
            //    context.LayoutInflater.Inflate(
            //        Android.Resource.Layout.SimpleListItem1, parent, false)) as TextView;
            //view.SetText(entry.Description == "" ? "<new entry>" : entry.Description, TextView.BufferType.Normal);

            return view;
        }
    }
}