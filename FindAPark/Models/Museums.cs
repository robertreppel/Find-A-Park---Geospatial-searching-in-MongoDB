using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FindAPark.Models
{
    public interface IMuseums
    {
        Museum PickARandomOne();
    }

    public class Museums : IMuseums
    {
        public Museum PickARandomOne()
        {
            return new Museum() { Name = "Smithsonian"};
        }
    }

    public class Museum
    {
        public string Name { get; set; }
    }
}