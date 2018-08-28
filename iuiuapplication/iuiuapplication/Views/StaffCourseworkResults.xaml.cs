using iuiuapplication.Libraries;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace iuiuapplication.Views
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StaffCourseworkResults : ContentPage
    {
        private HttpClient _client = new HttpClient();

        public StaffCourseworkResults(int ID, string cs_code, string course_nm, string sem, string yr, string acad, string prog, string sess)
        {
            InitializeComponent();

            lbl_progheader.Text = prog + "  YEAR " + yr + " " + sess;
            lbl_csheader.Text = "SETTINGS FOR " + course_nm + "\n[" + cs_code + "]";
            txt_compformat.SelectedIndex = 0;
            LoadCourseworkSettings(ID, cs_code, course_nm, sem, yr, acad, prog, sess);
            

        }


        public async void LoadCourseworkSettings(int ID, string cs_code, string course_nm, string sem, string yr, string acad, string prog, string sess)
        {
            try
            {

                if (CrossConnectivity.Current.IsConnected)
                {
                    //retrive the course work settings.

                    string myURL = MobileConfig.
                        KampalaCampuslink + string.
                        Format("DataFinder.aspx?dataFormat=cwk_settings&lect_id={0}&acadyr={1}&sem={2}" +
                        "&courseID={3}&prog={4}&session={5}&intk=-&cyr={6}", ID, acad, sem, cs_code, "-", sess, yr);

                    var content = await _client.GetStringAsync(myURL);
                    Debug.WriteLine("URL ->  " + myURL);

                    //saving our json data locally
                    MobileConfig.set_course_work_settings(content);

                    setCourseWorkSettings();


                }
                else
                {
                    setCourseWorkSettings();
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error huxy :  " + ex);
            }


            setCourseWorkSettings();
        }


        public void setCourseWorkSettings()
        {
            // retrieving data saved locally and formatting it to JSON object.
            string saved_data = MobileConfig.get_course_work_settings_json().Replace("[", "").Replace("]", "");
            //deserializing JSON object.
            Debug.WriteLine("DATA : " + saved_data);
            var des_json = JsonConvert.DeserializeObject<Model.CourseworkSettings>(saved_data);
            int cw1 = des_json.cs_maxmark_1;
            int cw2 = des_json.cs_maxmark_2;
            int cw3 = des_json.cs_maxmark_3;
            int cw4 = des_json.cs_maxmark_4;
            int cw5 = des_json.cs_maxmark_5;
            int cyear = des_json.cs_year;

            int total = des_json.finalMark;

            //setting data into entries 
            txtAss1.Text = "" + cw1;
            txtAss2.Text = "" + cw2;
            txtAss3.Text = "" + cw3;
            txtAss4.Text = "" + cw4;
            txtAss5.Text = "" + cw5;
            txtTotal.Text = "" + total;

        }

        private  void onSaveChangesTapped(object sender, EventArgs e)
        {

            saveSettings();

        }

        public  async void saveSettings()
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                // retrieving data saved locally and formatting it to JSON object.
                string saved_data = MobileConfig.get_course_work_settings_json().Replace("[", "").Replace("]", "");
                //deserializing JSON object.
                Debug.WriteLine("DATA : " + saved_data);
                var des_json = JsonConvert.DeserializeObject<Model.CourseworkSettings>(saved_data);

                string lecture_id = des_json.cs_lecturerID;
                string acadYr = des_json.cs_acadYear;
                string sem = "" + des_json.cs_semester;
                string course_id = "" + des_json.cs_courseID;
                string prog_id = "" + des_json.progid;
                string intake = "-";
                string sess = "" + des_json.cs_session;
                string cyear = "" + des_json.cs_year;
                string assn1 = txtAss1.Text;
                string assn2 = txtAss2.Text;
                string assn3 = txtAss3.Text;
                string Test1 = txtAss4.Text;
                string Test2 = txtAss5.Text; ;
                string noTask = "0";
                string finalMark = txtTotal.Text;
                string csid = "" + des_json.CSID;
                string compFormat = "" + txt_compformat.Items[txt_compformat.SelectedIndex];

                Debug.WriteLine("acadyer " + acadYr + " compFormat : " + compFormat);



                var url = MobileConfig.
                            KampalaCampuslink + string.
                            Format("DataFinder.aspx?dataFormat=courseworksettingsEdit&acad={0}&semester={1}&course_id={2}" +
                            "&prog_id={3}&intake={4}&sess={5}&cyear={6}&assn_1={7}&assn_2={8}&assn_3={9}" +
                            "&test_1={10}&test_2={11}&noTasks={12}&finalMark={13}&CSID={14}&usn={15}" +
                            "&compFormat={16}", acadYr, sem, course_id, prog_id, intake, sess, cyear
                            , assn1, assn2, assn3, Test1, Test2, noTask, finalMark, csid, lecture_id, compFormat);

                Debug.WriteLine("POST : " + url);

                var response = await _client.PostAsync(url, null);

                var responseContent = await response.Content.ReadAsStringAsync();

                // set the server reply a message to the Display Alert
                await DisplayAlert("", "" + responseContent, "Ok");
            }

            else
            {
                await DisplayAlert("No internet Connection", "sorry, please first connect to the internet.", "Ok");

            }
        }
    }
}

