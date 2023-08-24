using PDFreaderTEST;
using RelayCalculator.Services.Enums;

namespace RelayCalculator.Services.Models
{
    public class Record
    {
        public Distance Distance
        {
            get;
            set;
        }

        public Stroke Stroke
        {
            get;
            set;
        }

        public double Time
        {
            get;
            set;
        }

        public int Age
        {
            get;
            set;
        }

        public Gender Gender
        {
            get;
            set;
        }

        public Course Course
        {
            get;
            set;
        }

        public RecordType RecordType
        {
            get;
            set;
        }
    }
}
