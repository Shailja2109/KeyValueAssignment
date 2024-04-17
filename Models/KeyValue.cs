using System.ComponentModel.DataAnnotations;

namespace KeyValue.Models
{
    public class KeyValueData
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}