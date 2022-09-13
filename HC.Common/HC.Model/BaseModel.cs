using System;

namespace HC.Model
{
    /// <summary>
    /// Base model for the basic things in all models
    /// </summary>
    public class BaseModel
    {
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public byte Status { get; set; }
    }
}
