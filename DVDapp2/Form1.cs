using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace DVDapp2
{
    public partial class Form1 : Form
    {

        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern Int64 libvlc_new(int argc, string argv);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern Int64 libvlc_media_new_location(Int64 p_instance, string psz_mrl);

        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern Int64 libvlc_media_player_new_from_media(Int64 p_md);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern int libvlc_media_player_play(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern void libvlc_media_player_stop(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern void libvlc_media_player_pause(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern void libvlc_toggle_fullscreen(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern void libvlc_set_fullscreen(Int64 p_mi, int EnableFullscreen);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern void libvlc_audio_toggle_mute(Int64 p_mi);

        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern Int64 libvlc_media_player_get_time(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern void libvlc_media_player_set_time(Int64 p_mi, Int64 i_time);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern void libvlc_media_player_next_chapter(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern Int64 libvlc_media_player_get_length(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern int libvlc_media_player_get_chapter(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern int libvlc_media_player_get_title(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern void libvlc_media_player_set_title(Int64 p_mi, int i_title);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern int libvlc_video_get_track(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern int libvlc_video_set_track(Int64 p_mi, int i_track);

        //[DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        //LIBVLC_API int libvlc_media_player_get_full_title_descriptions(libvlc_media_player_t* p_mi, libvlc_title_description_t*** titles);
        //[DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        //LIBVLC_API int libvlc_media_player_get_full_chapter_descriptions(libvlc_media_player_t* p_mi, int i_chapters_of_title, libvlc_chapter_description_t*** pp_chapters);
        //[DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        //LIBVLC_API libvlc_track_description_t * 	libvlc_video_get_track_description(libvlc_media_player_t* p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern int libvlc_video_get_track_count(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern int libvlc_media_player_get_chapter_count(Int64 p_mi);
        [DllImport("C:\\Program Files\\VideoLAN\\VLC\\libvlc.dll")]
        public static extern int libvlc_media_player_get_title_count(Int64 p_mi);

        public Form1()
        {
            InitializeComponent();
            MyDvd = libvlc_new(0, "");
            MyMediaPlayer = libvlc_media_player_new_from_media(libvlc_media_new_location(MyDvd, "dvd:///d:/"));
            FilterFileEntries = new List<string[]>();
            CurrentFilterIndx = 0;
            // init filter to something bogus until loaded
            FilterFileEntry = new string [] { "9999999999", "", "9999999999" };
            FilterFileEntries.Add(FilterFileEntry);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // start filter events at beginning, or should check where current postion, as this button can also unpause
            CurrentFilterIndx = 0;
            FilterFileEntry = FilterFileEntries[CurrentFilterIndx];
            NextTimeEvent = Convert.ToInt64(FilterFileEntry[0]);

            libvlc_media_player_play(MyMediaPlayer);
            timer1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            libvlc_media_player_stop(MyMediaPlayer);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            libvlc_media_player_pause(MyMediaPlayer);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            libvlc_toggle_fullscreen(MyMediaPlayer);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            libvlc_audio_toggle_mute(MyMediaPlayer);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox1.Text = libvlc_media_player_get_time(MyMediaPlayer).ToString();
            textBox3.Text = libvlc_media_player_get_length(MyMediaPlayer).ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            libvlc_media_player_set_time(MyMediaPlayer, Convert.ToInt64(textBox1.Text));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            libvlc_media_player_next_chapter(MyMediaPlayer);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //StreamReader myfilestream;
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.SafeFileName;
                myFilterFile = new StreamReader(openFileDialog1.FileName);
                FilterFileEntries.Clear();
                do
                {
                    FilterFileEntry = myFilterFile.ReadLine().Split(',');
                    FilterFileEntries.Add(FilterFileEntry);
                    textBox1.Text = FilterFileEntry[0];
                } while (!myFilterFile.EndOfStream);
                myFilterFile.Close();
            }
            // reload first time event, really, should do this relative to current playing position
            //while(Entry.. < currenttime)
            //{
            CurrentFilterIndx = 0;
            FilterFileEntry = FilterFileEntries[CurrentFilterIndx];
            NextTimeEvent = Convert.ToInt64(FilterFileEntry[0]);
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // title 1 appears to be main event (ie. the movie); only run this code once the main event starts
            // seems this is not always the case, which breaks fastplay, too; so, allow option to force
            // until figure out how to detect main event
            if((libvlc_media_player_get_title(MyMediaPlayer) == 1) || (checkBox1.Checked))
            {
                // check if we've hit next time event
                if (libvlc_media_player_get_time(MyMediaPlayer) >= NextTimeEvent)
                {
                    if (FilterFileEntry[1] == "mute")
                    {
                        libvlc_audio_toggle_mute(MyMediaPlayer);
                        // setup unmute event (still uses 'mute' keyword); only do this once
                        if (Convert.ToInt64(FilterFileEntry[2]) > NextTimeEvent)
                        {
                            NextTimeEvent = Convert.ToInt64(FilterFileEntry[2]);
                        }
                        else
                        {
                            CurrentFilterIndx++;
                            if (CurrentFilterIndx < FilterFileEntries.Count)
                            {
                                FilterFileEntry = FilterFileEntries[CurrentFilterIndx];
                                NextTimeEvent = Convert.ToInt64(FilterFileEntry[0]);
                            }
                            else
                            {
                                NextTimeEvent = 9999999999; // set to value that is too large
                                // disable this timer, since we don't need it anymore
                                //timer1.Enabled = false;
                            }
                        }

                    }
                    else if (FilterFileEntry[1] == "skip")
                    {
                        libvlc_media_player_set_time(MyMediaPlayer, Convert.ToInt64(FilterFileEntry[2]));
                        CurrentFilterIndx++;
                        if (CurrentFilterIndx < FilterFileEntries.Count)
                        {
                            FilterFileEntry = FilterFileEntries[CurrentFilterIndx];
                            NextTimeEvent = Convert.ToInt64(FilterFileEntry[0]);
                        }
                        else
                        {
                            NextTimeEvent = 9999999999; // set to value that is too large
                                                          // disable this timer, since we don't need it anymore
                            //timer1.Enabled = false;
                        }
                    }
                }
            }
            //update progress on trackbar
            hScrollBar1.Maximum = (int)libvlc_media_player_get_length(MyMediaPlayer);
            // sometimes it seems out of context and hscrollbar values are not valid???
            if (hScrollBar1.Maximum != 0)
            {
                hScrollBar1.Value = (int)libvlc_media_player_get_time(MyMediaPlayer);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            libvlc_media_player_set_time(MyMediaPlayer, libvlc_media_player_get_time(MyMediaPlayer) - 5000);
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            libvlc_media_player_set_time(MyMediaPlayer, hScrollBar1.Value);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            textBox1.Text = libvlc_media_player_get_chapter(MyMediaPlayer).ToString();
            textBox2.Text = libvlc_media_player_get_title(MyMediaPlayer).ToString();
            textBox3.Text = libvlc_video_get_track(MyMediaPlayer).ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            // seems that title is generally the main event...
            libvlc_media_player_set_title(MyMediaPlayer, 1);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            libvlc_media_player_set_time(MyMediaPlayer, libvlc_media_player_get_time(MyMediaPlayer) + 5000);
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
    }
}
