using iuiuapplication.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iuiuapplication.DB
{
    class MyDB
    {
        private SQLiteConnection _connection;

        public MyDB()
        {
            _connection = DependencyService.Get<ISQLiteService>().GetConnection("iuiu_db");
            _connection.CreateTable<ResultStringModel>();
            _connection.CreateTable<SalarySummaryModel>();
            _connection.CreateTable<CourseworkStringModel>();
            _connection.CreateTable<FeesPaymentString>();
            _connection.CreateTable<ProgrammeString>();
            _connection.CreateTable<RegistrationStringModel>();
            _connection.CreateTable<GraduandString>();
            _connection.CreateTable<TimetableStringModel>();
            _connection.CreateTable<TeachingClaimStringModel>();
            _connection.CreateTable<LectureStringModel>();
            _connection.CreateTable<SalaryDetailsModel>();
            _connection.CreateTable<CourseAllocationString>();
        }
        public void AddClaims(string acadyear,string claimString)
        {
            _connection.Query<TeachingClaimStringModel>("INSERT INTO [TeachingClaimStringModel] (acadyear,claimString) VALUES ('" + acadyear + "','" + claimString + "')");
        }
        public void AddResults(string resultString)
        {
            _connection.Query<ResultStringModel>("INSERT INTO [ResultStringModel] (resultString) VALUES ('" + resultString + "')");
        }

        public void AddLectures(string lectureString)
        {
            _connection.Query<LectureStringModel>("INSERT INTO [LectureStringModel] (LectureString) VALUES ('" + lectureString + "')");
        }

        public void AddCourseworkResults(string resultString)
        {
            _connection.Query<CourseworkStringModel>("INSERT INTO [CourseworkStringModel] (resultString) VALUES ('" + resultString + "')");
        }

        public void AddProgrammes(string resultString)
        {
            _connection.Query<ProgrammeString>("INSERT INTO [ProgrammeString] (programmeString) VALUES ('" + resultString + "')");
        }

        public void AddRegistration(string resultString)
        {
            _connection.Query<RegistrationStringModel>("INSERT INTO [RegistrationStringModel] (regString) VALUES ('" + resultString + "')");
        }

        public void AddGraduands(string gradString,string prog)
        {
            _connection.Query<GraduandString>("INSERT INTO [GraduandString] (gradString,prog_id) VALUES ('" + gradString + "','" + prog + "')");
        }

        public void AddTimetables(string TTString,string acad,string sem)
        {
            _connection.Query<TimetableStringModel>("INSERT INTO [TimetableStringModel] (TTString,AcadYear,Semester) VALUES ('" + TTString + "','" + acad + "','" + sem + "')");
        }
        public void AddCourseAllocation(string empcode,string allocationString)
        {
            _connection.Query<CourseAllocationString>("INSERT INTO [CourseAllocationString] (empcode,allocationString) VALUES ('" + empcode + "','" + allocationString+ "')");
        }
        public void resetResults()
        {
            _connection.Query<ResultStringModel>("DELETE  FROM [ResultStringModel]");
        }
        public void resetLectures()
        {
            _connection.Query<LectureStringModel>("DELETE  FROM [LectureStringModel]");
        }
        public void resetClaims(string acadyear)
        {
            _connection.Query<TeachingClaimStringModel>("DELETE  FROM [TeachingClaimStringModel] WHERE acadyear='"+acadyear+"'");
        }
        public void resetTimetables(string acad,string sem)
        {
            _connection.Query<TimetableStringModel>("DELETE  FROM [TimetableStringModel] WHERE AcadYear='"+acad+ "' AND Semester='" + sem + "'");
        }
        public void resetProgrammes()
        {
            _connection.Query<ProgrammeString>("DELETE  FROM [ProgrammeString]");
        }

        public void resetRegistration()
        {
            _connection.Query<RegistrationStringModel>("DELETE  FROM [RegistrationStringModel]");
        }

        public void resetGraduands(string prog)
        {
            _connection.Query<GraduandString>("DELETE  FROM [GraduandString] WHERE prog_id='" + prog + "'");
        }

        public void AddFeesPayment(string paymentString)
        {
            _connection.Query<FeesPaymentString>("INSERT INTO [FeesPaymentString] (paymentString) VALUES ('" + paymentString + "')");
        }

        public void resetCourseworkResults()
        {
            _connection.Query<CourseworkStringModel>("DELETE  FROM [CourseworkStringModel]");
        }

        public void resetFeesPayment()
        {
            _connection.Query<FeesPaymentString>("DELETE  FROM [FeesPaymentString]");
        }
        //
        public void resetCourseAllocations()
        {
            _connection.Query<FeesPaymentString>("DELETE  FROM [CourseAllocationString]");
        }
        public string GetAllResults()
        {
            List<ResultStringModel> list = _connection.Query<ResultStringModel>("SELECT * FROM [ResultStringModel]");
            return list[0].resultString;

        }
        public string GetMyCourseALlocations(string empcode)
        {
            List<CourseAllocationString> list = _connection.Query<CourseAllocationString>("SELECT * FROM [CourseAllocationString] WHERE empcode='"+ empcode + "'");
            return list[0].allocationString;

        }
        public string GetAllLectures()
        {
            try
            {
                List<LectureStringModel> list = _connection.Query<LectureStringModel>("SELECT * FROM [LectureStringModel]");
                return list[0].LectureString;
            }
            catch (Exception) { return "[]"; }

        }
        public string GetAllClaims(string acadyear)
        {
            List<TeachingClaimStringModel> list = _connection.Query<TeachingClaimStringModel>("SELECT * FROM [TeachingClaimStringModel] WHERE acadyear='"+acadyear+"'");
            return list[0].claimString;

        }
        public string GetAllCourseworkResults()
        {
            List<CourseworkStringModel> list = _connection.Query<CourseworkStringModel>("SELECT * FROM [CourseworkStringModel]");
            return list[0].resultString;

        }

        public string GetAllFeesPayments()
        {
            List<FeesPaymentString> list = _connection.Query<FeesPaymentString>("SELECT * FROM [FeesPaymentString]");
            return list[0].paymentString;

        }

        public string GetAllProgrammes()
        {
            List<ProgrammeString> list = _connection.Query<ProgrammeString>("SELECT * FROM [ProgrammeString]");
            return list[0].programmeString;

        }

        public string GetAllRegistration()
        {
            List<RegistrationStringModel> list = _connection.Query<RegistrationStringModel>("SELECT * FROM [RegistrationStringModel]");
            return list[0].regString;

        }
        public string GetAllTimetables(string acad,string sem)
        {
            List<TimetableStringModel> list = _connection.Query<TimetableStringModel>("SELECT * FROM [TimetableStringModel] WHERE AcadYear='" + acad + "' AND Semester='" + sem + "' ");
            return list[0].TTString;

        }

        public string GetAllGraduands(string prog)
        {
            List<GraduandString> list = _connection.Query<GraduandString>("SELECT * FROM [GraduandString] WHERE prog_id='"+prog+"'");
            return list[0].gradString;

        }

        /*Salary Summary Data*/
        public void AddSalarySummary(string Item, string amount, string months, string Years, string empCodes, string more_img)
        {
            _connection.Query<SalarySummaryModel>("INSERT INTO [SalarySummaryModel] (Item, amount, months, Years, empCodes,more_img) VALUES ('" + Item + "','" + amount + "','"
                + months + "','" + Years + "','" + empCodes + "','" + more_img + "')");
        }
        public void resetSalarySummary(string empCodes, string months, string Years)
        {
            _connection.Query<SalarySummaryModel>("DELETE  FROM [SalarySummaryModel] WHERE empCodes='" + empCodes + "' AND months='" + months + "' AND Years='" + Years + "'");
        }

        public List<SalarySummaryModel> GetMonthlySalary(string empCodes, string months, string Years)
        {
            List<SalarySummaryModel> list = _connection.Query<SalarySummaryModel>("SELECT * FROM [SalarySummaryModel] WHERE empCodes='" + empCodes + "' AND months='" + months + "' AND Years='" + Years + "'");
            return list;

        }

        /*Salary Details Data*/
        public string AddSalaryDetail(string Item, string amount, string months, string Years, string empCodes,string category)
        {
            try
            {
                _connection.Query<StudentResultModel>("INSERT INTO [SalaryDetailsModel] (Item, amount, months, Years, empCodes,category) VALUES ('" + Item + "','" + amount + "','"
                    + months + "','" + Years + "','" + empCodes + "','" + category + "')");
                return "Item Added";
            }
            catch (Exception ex) {
                return "Error! "+ex.Message;
            }
        }
        public void resetSalaryDetails(string empCodes, string months, string Years,string Category)
        {
            _connection.Query<StudentResultModel>("DELETE  FROM [SalaryDetailsModel] WHERE empCodes='" + empCodes + "' AND months='" + months + "' AND Years='" + Years + "' AND category='" + Category + "'");
        }

        public List<SalaryDetailsModel> GetMonthlySalaryDetails(string empCodes, string months, string Years, string Category)
        {
            List<SalaryDetailsModel> list = _connection.Query<SalaryDetailsModel>("SELECT * FROM [SalaryDetailsModel] WHERE empCodes='" + empCodes + "' AND months='" + months + "' AND Years='" + Years + 
            "' AND category='" + Category + "'");
            return list;
        }
    }
}
