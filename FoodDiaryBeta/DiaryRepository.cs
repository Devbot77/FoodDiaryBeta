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

using SQLite;
using System.Threading.Tasks;

namespace FoodDiaryBeta
{
    class DiaryRepository
    {
        private readonly SQLiteAsyncConnection db;

        public DiaryRepository()
        {
            db = new SQLiteAsyncConnection(DatabaseFilePathRetriever.GetPath());
            db.CreateTableAsync<DiaryDate>();
            db.CreateTableAsync<Entry>();
            Console.WriteLine("tables created");
        }

        //get all diary dates
        public async Task<List<DiaryDate>> GetDates()
        {
            return await db.Table<DiaryDate>().ToListAsync();
        }
        //get all entries
        public async Task<List<Entry>> GetEntries()
        {
            // instead of this, should maybe grab by today's date?
            return await db.Table<Entry>().ToListAsync();
        }

        public async Task<List<Entry>> GetEntriesByDate(int dateId)
        {
            // grab entries of specific date?
            return await db.Table<Entry>().Where(e => e.EntryDateId == dateId).ToListAsync();
        }

        // returns a string array for use with diary listview
        public async Task<string[]> GetEntryStrings()
        {
            var entryList = await db.Table<Entry>().ToListAsync();
            int count = entryList.Count;
            string[] entries = new string[30];
            int index = 0;
            foreach (var i in entryList)
            {
                entries[index] = i.Description;
                index += 1;
            }
            entries = entries.Where(x => x != null).ToArray();
            return entries;
        }

        //get specific date
        public async Task<DiaryDate> GetDate(int id)
        {
            return await db.Table<DiaryDate>().Where(dd => dd.DateId == id).FirstOrDefaultAsync();
        }

        //Get date by date string
        public async Task<DiaryDate> GetDateByString(string s)
        {
            return await db.Table<DiaryDate>().Where(dd => dd.Date == s).FirstOrDefaultAsync();
        }

        //get specific entry
        public async Task<Entry> GetEntry(int id)
        {
            return await db.Table<Entry>().Where(e => e.EntryId == id).FirstOrDefaultAsync();
        }

        //delete specific date
        public async Task DeleteDate(DiaryDate date)
        {
            await db.DeleteAsync(date);
        }
        //delete specific entry
        public async Task DeleteEntry(Entry entry)
        {
            await db.DeleteAsync(entry);
        }

        //add new date to db
        public async Task AddDate(DiaryDate date)
        {
            await db.InsertAsync(date);
        }
        //add new entry
        public async Task AddEntry(Entry entry)
        {
            await db.InsertAsync(entry);
        }

        public async Task UpdateEntry(Entry entry)
        {
            await db.UpdateAsync(entry);
        }

        public async Task UpdateDate(DiaryDate date)
        {
            await db.UpdateAsync(date);
        }
    }
}