using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Data
{
    public class Comment
    {
        public int postId { get; set; }
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string body { get; set; }
    }
}
