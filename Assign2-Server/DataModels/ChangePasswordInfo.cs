using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Assign2_Server.DataModels
{
    public class ChangePasswordInfo
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string NewPassword { get; set; }

        [Required]
        public string OldPassword { get; set; }

    }
}
