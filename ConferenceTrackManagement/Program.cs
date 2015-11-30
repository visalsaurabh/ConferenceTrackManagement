using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace ConferenceTrackManagement
{
    public class Talk
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public Boolean IsSchedule { get; set; }

        public void Getduration(Talk[] talks, String[] lines)
        {
            for (int i = 0; i < talks.Length; i++)
            {
                talks[i] = new Talk();
                talks[i].Name = lines[i]; ;
                talks[i].IsSchedule = false;
                string[] items = lines[i].Trim().Split(' ');
                int len2 = items.Length - 1;
                string last = items[len2].ToLower();
                if (last == "lightning")
                {
                    talks[i].Duration = 5;
                }
                else
                {
                    talks[i].Duration = int.Parse(last.Replace("min", ""));
                }
            }
        }

        public void sortDisplay(Talk[] talks)
        {
            List<Talk> track1am = new List<Talk>();
            List<Talk> track1pm = new List<Talk>();
            List<Talk> track2am = new List<Talk>();
            List<Talk> track2pm = new List<Talk>();

            int CheckedAllSession = 0;

            talks = talks.OrderByDescending(t => t.Duration).ToArray();

            int schedule1am = 0, schedule1pm = 0, schedule2am = 0, schedule2pm = 0;
            int next = 0;

            for (int i = 0; i < talks.Length; i++)
            {
                int duration = talks[i].Duration;

                if (next == 0 && (schedule1am + duration) <= 180)
                {
                    track1am.Add(talks[i]);
                    talks[i].IsSchedule = true;
                    schedule1am += duration;
                    next++;
                    CheckedAllSession = 0;
                }
                else if (next == 1 && (schedule2am + duration) <= 180)
                {
                    track2am.Add(talks[i]);
                    talks[i].IsSchedule = true;
                    schedule2am += duration;
                    next++;
                    CheckedAllSession = 0;
                }
                else if (next == 2 && (schedule1pm + duration) <= 240)
                {
                    track1pm.Add(talks[i]);
                    talks[i].IsSchedule = true;
                    schedule1pm += duration;
                    next++;
                    CheckedAllSession = 0;
                }
                else if (next == 3 && (schedule2pm + duration) <= 240)
                {
                    track2pm.Add(talks[i]);
                    talks[i].IsSchedule = true;
                    schedule2pm += duration;
                    next = 0;
                    CheckedAllSession = 0;
                }
                else
                {
                    i--;
                    next++;
                    if (next == 4) next = 0;
                    CheckedAllSession++;
                    if (CheckedAllSession == 4)
                    {
                        i++;
                        CheckedAllSession = 0;
                    }
                }
            }

            int mins = Math.Max(schedule1pm, schedule2pm);
            if (mins < 180) 
                mins = 180; // can't be before 4.00 pm
            if (mins > 180) 
                mins = 240;
            DateTime nwe = DateTime.Today.AddHours(13).AddMinutes(mins);

            // print results
            Console.Clear();
            Console.WriteLine("Track1:");
            DateTime dt = DateTime.Today.AddHours(9);


            foreach (Talk t in track1am)
            {
                Console.WriteLine("{0} {1}", dt.ToString("hh:mmtt"), t.Name);
                dt = dt.AddMinutes(t.Duration);
            }

            Console.WriteLine("12:00PM Lunch");
            dt = DateTime.Today.AddHours(13);

            foreach (Talk t in track1pm)
            {
                Console.WriteLine("{0} {1}", dt.ToString("hh:mmtt"), t.Name);
                dt = dt.AddMinutes(t.Duration);
            }

            Console.WriteLine("{0} Networking Event", nwe.ToString("hh:mmtt"));

            Console.WriteLine("\nTrack2:");
            dt = DateTime.Today.AddHours(9);

            foreach (Talk t in track2am)
            {
                Console.WriteLine("{0} {1}", dt.ToString("hh:mmtt"), t.Name);
                dt = dt.AddMinutes(t.Duration);
            }

            Console.WriteLine("12:00PM Lunch");
            dt = DateTime.Today.AddHours(13);

            foreach (Talk t in track2pm)
            {
                Console.WriteLine("{0} {1}", dt.ToString("hh:mmtt"), t.Name);
                dt = dt.AddMinutes(t.Duration);
            }

            Console.WriteLine("{0} Networking Event", nwe.ToString("hh:mmtt"));

            Console.WriteLine(" ");
            Console.WriteLine("Un scheduled Event");

            foreach (Talk t in talks)
            {
                if (t.IsSchedule == false)
                {
                    Console.WriteLine(t.Name);
                }
            }

            Console.ReadKey();
        }
    }

  
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                string[] lines = File.ReadAllLines(@"InputData\schedule.txt");
                int len = lines.Length;
                Talk[] talks = new Talk[len];

                Talk talk = new Talk();
                talk.Getduration(talks,lines);
                talk.sortDisplay(talks);
            }
            catch(Exception e)
            {
                Console.WriteLine("Something went wrong.Please check");
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
    }
}
