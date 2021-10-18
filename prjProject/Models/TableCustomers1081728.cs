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

    public partial class TableCustomers1081728
    {
        [DisplayName("會員帳號")]
        [Required(ErrorMessage = "會員帳號不可空白")]
        [StringLength(20, ErrorMessage = "員工編號必須是5~20個字元", MinimumLength = 5)]
        public string UserId { get; set; }

        [DisplayName("會員密碼")]
        [Required(ErrorMessage = "密碼不可空白")]
        public string UserPwd { get; set; }

        [DisplayName("會員姓名")]
        [Required(ErrorMessage = "姓名不可空白")]
        public string UserName { get; set; }

        [DisplayName("會員性別")]
        public string Gender { get; set; }

        [DisplayName("會員信箱")]
        [Required]
        [EmailAddress(ErrorMessage = "E-Mail 格式有誤")]
        public string Email { get; set; }

        [DisplayName("會員地址")]
        [Required(ErrorMessage = "會員地址不可空白")]
        public string UserAddress { get; set; }
    }
}