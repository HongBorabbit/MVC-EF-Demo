using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Demo_CRUD.Models
{
    public class NewInfos
    {
        public int newsid { get; set; }
        public string newsName { get; set; }
        public string newImage { get; set; }
        public string typeName { get; set; }
        public int typeid { get; set; }
    }
}