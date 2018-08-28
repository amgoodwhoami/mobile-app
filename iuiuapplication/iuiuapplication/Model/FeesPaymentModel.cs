using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iuiuapplication.Model
{
    class FeesPaymentModel
    {
        public string accName { get; set; }
        public string transaction_amount { get; set; }
        public string particulars { get; set; }
        public string voucherNo { get; set; }
        public string formated_date { get; set; }
        public string transactionType { get; set; }
        public string teller { get; set; }
        public string timeLog { get; set; }
        public string curr_balance { get; set; }
        public string label_color { get; set; }
    }
}
