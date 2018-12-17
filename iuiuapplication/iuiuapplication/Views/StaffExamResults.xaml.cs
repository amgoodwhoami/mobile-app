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
    public partial class StaffExamResults : ContentPage
    {
        private HttpClient _client = new HttpClient();
        public StaffExamResults(string cs_code, string course_nm, string sem, string yr, string acad, string prog_id, string sess)
        {
            InitializeComponent();
            try
            {
                lbl_progheader.Text = prog_id + "  YEAR " + yr + " " + sess;
            lbl_csheader.Text = "SETTINGS FOR " + course_nm + "\n[" + cs_code + "]";

            LoadExamSettings(Application.Current.Properties["userno"].ToString(), cs_code, sem, yr, acad, prog_id, sess);
            }
            catch (Exception ey)
            {

                DisplayAlert("General Error!  ", "" + ey.InnerException.Message, "Ok");

            }


        }


        public async void LoadExamSettings(string username, string cs_code, string sem, string yr, string acad, string prog_id, string sess)
        {
            try
            {

                if (CrossConnectivity.Current.IsConnected)
                {
                    //retrive the course work settings.

                    string myURL = MobileConfig.
                        GetWebAddress(Application.Current.Properties["campus"].ToString()) + string.Format("DataFinder.aspx?dataFormat=examsettings&empcode={0}&acad={1}&semester={2}" +
                        "&course_id={3}&prog_id={4}&session={5}&intake=-&cyear={6}", username, acad, sem, cs_code, prog_id, sess, yr);

                    
                   var content = await _client.GetStringAsync(myURL);
                    //Debug.WriteLine("URL ->  " + myURL);
                    //await DisplayAlert("General Error!  ", "" + myURL, "Ok");
                    //saving our json data locally
                    MobileConfig.set_exam_results(content);

                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error huxy :  " + ex);

            }
            setExamresultsSettings();

        }

        void setExamresultsSettings()
        {
            // retrieving data saved locally and formatting it to JSON object.
            string saved_data = MobileConfig.get_exam_results();
            //deserializing JSON object.
            Debug.WriteLine("DATA : " + saved_data);
            var des_json = JsonConvert.DeserializeObject<Model.Examresultssettings>(saved_data);

            int mark1 = des_json.maxMark1;
            int mark2 = des_json.maxMark2;
            int mark3 = des_json.maxMark3;
            int mark4 = des_json.maxMark4;
            int mark5 = des_json.maxMark5;
            int mark6 = des_json.maxMark6;
            int mark7 = des_json.maxMark7;
            int mark8 = des_json.maxMark8;
            int mark9 = des_json.maxMark9;
            int mark10 = des_json.maxMark10;
            int total = des_json.TotalMaxMark;
            int cwk_ratio = des_json.cwRatio;
            int examratio = des_json.examRatio;


            //setting data into entries 
            txtAss1.Text = "" + mark1;
            txtAss2.Text = "" + mark2;
            txtAss3.Text = "" + mark3;
            txtAss4.Text = "" + mark4;
            txtAss5.Text = "" + mark5;
            txtAss6.Text = "" + mark6;
            txtAss7.Text = "" + mark7;
            txtAss8.Text = "" + mark8;
            txtAss9.Text = "" + mark9;
            txtAss10.Text = "" + mark10;
            txtTotal.Text = "" + total;
            txtCWPercent.Text = "" + cwk_ratio;
            txtEXPercent.Text = "" + examratio;

        }


        public async void saveSettings()
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                // retrieving data saved locally and formatting it to JSON object.
                string saved_data = MobileConfig.get_exam_results().Replace("[", "").Replace("]", "");
                //deserializing JSON object.
                Debug.WriteLine("DATA : " + saved_data);
                var des_json = JsonConvert.DeserializeObject<Model.Examresultssettings>(saved_data);
                string lecturerID = des_json.lecturerID;
                int exam_settingsID = des_json.examSettingsID;
                string acadYr = des_json.acadyear;
                string sem = "" + des_json.semester;
                string course_id = "" + des_json.course_id;
                string prog_id = "" + des_json.progid;
                string intake = "-";
                string sess = "" + des_json.studSession;
                string cyear = "" + des_json.cyear;
                string mark1 = txtAss1.Text;
                string mark2 = txtAss2.Text;
                string mark3 = txtAss3.Text;
                string mark4 = txtAss4.Text;
                string mark5 = txtAss5.Text;
                string mark6 = txtAss6.Text;
                string mark7 = txtAss7.Text;
                string mark8 = txtAss8.Text;
                string mark9 = txtAss9.Text;
                string mark10 = txtAss10.Text;
                string examratio = txtEXPercent.Text;
                string cwk_ratio = txtCWPercent.Text;
                string exam_format = "";
                string noQns = "0";
                string finalMark = txtTotal.Text;
               
         
                var url = MobileConfig.GetWebAddress(Application.Current.Properties["campus"].ToString()) + string.
                            Format("DataFinder.aspx?dataFormat=ExamSettingsEdit&acad={0}&semester={1}&course_id={2}" +
                            "&prog_id={3}&intake={4}&sess={5}&cyear={6}&maxQ1={7}&maxQ2={8}&maxQ3={9}" +
                            "&maxQ4={10}&maxQ5={11}&maxQ6={12}&maxQ7={13}&maxQ8={14}&maxQ9={15}" +
                            "&maxQ10={16}&cwRatio={17}&examRatio={18}&noQns={19}&" +
                            "maxTotal={20}&EXID={21}&examFormat={22}&empcode={23}",
                            acadYr, sem, course_id, prog_id, intake, sess, cyear
                            , mark1, mark2, mark3, mark4, mark5, mark6, mark7, mark8, mark9, mark10,
                            cwk_ratio,examratio,noQns,finalMark,exam_settingsID,exam_format,lecturerID);

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

        //editting the results by a click.
        private void onSaveChangesTapped(object sender, EventArgs e)
        {
            saveSettings();
        }

        private async void onViewResultsSheet(object sender, EventArgs e)
        {
            // retrieving data saved locally and formatting it to JSON object.
            string saved_data = MobileConfig.get_exam_results().Replace("[", "").Replace("]", "");
            //deserializing JSON object.
            Debug.WriteLine("DATA : " + saved_data);
            var des_json = JsonConvert.DeserializeObject<Model.Examresultssettings>(saved_data);
            string lecturerID = des_json.lecturerID;
            int exam_settingsID = des_json.examSettingsID;
            MobileConfig.save_exam_settings_id(""+exam_settingsID);
            await Navigation.PushAsync(new ExamViewSheet());

        }
    }
}


   

