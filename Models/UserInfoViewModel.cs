using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PIP_Todo1.Models {
    public class UserInfoViewModel {

        [Required(ErrorMessage = "員工編號是必填欄位!")]
        public string UserID { get; set; }

        [Required(ErrorMessage = "員工英文姓名是必填欄位!")]
        public string UserNameEN { get; set; }

        [Required(ErrorMessage = "地址是必填欄位!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "職位是必填欄位!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "電子郵件是必填欄位!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "團隊編號是必填欄位!")]
        public string TeamID { get; set; }

        [Required(ErrorMessage = "團隊名稱是必填欄位!")]
        public string TeamName { get; set; }
        public Nullable<System.DateTime> CreateDT { get; set; }
        public Nullable<System.DateTime> UpdateDT { get; set; }
    }
}