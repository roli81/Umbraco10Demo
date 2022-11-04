﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sss.Umb9.Mutobo.PoCo
{
    public class ContactFormData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public int SenderMailConfigId { get; set; }
        public int ReceiverMailConfigId { get; set; }
        public Guid LandingPageId { get; set; }
        public string FuSb { get; set; }
        public Guid CaptchaId { get; set; }
        public string CaptchaText { get; set; }

    }
}
