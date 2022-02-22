using System.Collections.Generic;

namespace QMS.Models
{
    public class QmsApiResponseData
    {
        public int? rescode { get; set; }
        public string msg { get; set; }
        public string method { get; set; }
        public List<StickerInformation> data { get; set; }
    }
}
