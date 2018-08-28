using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iuiuapplication.Libraries
{
    class MobileConfig
    {
        //192.168.137.35
        //192.168.137.1
        //Local link :192.168.137.1
   
        public static string MainCampuslink = "http://196.43.172.15/Mobile/";
        public static string MainCampusPhotolink = "https://196.43.172.18/ERP/Academics/studentimages/";
        public static string KampalaCampuslink = "http://196.43.182.5/mobile/";
        public static string KampalaCampusPhotolink = "http://196.43.182.7/ERP/Academics/studentimages/";
        public static string FemalesCampuslink = "http://137.63.131.2:92/Mobile/";
        public static string FemalesCampusPhotolink = "https://137.63.131.2:83/ERP/Academics/studentimages/";

        public static string GetWebAddress(string campus)
        {
            if (campus == "Main Campus") return MainCampuslink; else if (campus == "Kampala Campus") return KampalaCampuslink; else return FemalesCampuslink;
        }
        public static string GetPhotoAddress(string campus)
        {
            if (campus == "Main Campus") return MainCampusPhotolink; else if (campus == "Kampala Campus") return KampalaCampusPhotolink; else return FemalesCampusPhotolink;
        }

        public static void set_course_work_settings(string json)
        {
            Application.Current.Properties["cwk_settings_json"] = json;

        }

        public static string get_course_work_settings_json()
        {
            return Application.Current.Properties["cwk_settings_json"].ToString().Replace("-","");

        }


        public static void set_exam_results(string json)
        {
            Application.Current.Properties["exam_settings_json"] = json;
        }

        public static string get_exam_results()
        {
            return Application.Current.Properties["exam_settings_json"].ToString().Replace("[","").Replace("]", "");
        }

        public static void save_exam_results_sheet(string json)
        {
            Application.Current.Properties["exam_results_sheet"] = json;
        }

        public static string  get_exam_results_sheet()
        {
           return  Application.Current.Properties["exam_results_sheet"].ToString();
        }

        public static void save_exam_settings_id(string id)
        {
            Application.Current.Properties["exid"] = id;
        }
        public static string  get_exam_settings_id()
        {
           return  Application.Current.Properties["exid"].ToString();
        }

    }
}
