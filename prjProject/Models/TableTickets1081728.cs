//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace prjProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class TableTickets1081728
    {
        [DisplayName("票券編號")]
        public string TicId { get; set; }

        [DisplayName("票券名稱")]
        public string TicName { get; set; }

        [DisplayName("票券價格")]
        public Nullable<int> Price { get; set; }

        [DisplayName("票券圖片")]
        public string TictImage { get; set; }
    }
}
