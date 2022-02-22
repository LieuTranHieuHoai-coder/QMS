namespace QMS.Models
{
    public class StickerInformation
    {
        public string CodeId { get; set; }
        public string RFIDCode { get; set; }
        public string Factory { get; set; }
        public string Customer { get; set; }
        public string Style { get; set; }
        public string Season { get; set; }
        public string BuyMonth { get; set; }
        public string ShipDest { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string PoNo { get; set; }
        public string IsPrint { get; set; }
        public string PrintQty { get; set; }
        public string PrintDated { get; set; }
        public string PrintStatus { get; set; }
        public int? BundleStatus { get; set; }
        public string WorkNo { get; set; }
        public string BreakDownType { get; set; }
        public string WorkType { get; set; }
        public string IsRecycle { get; set; }
        public string CreateUser { get; set; }
        public string CreateDated { get; set; }
        public string ChangeDated { get; set; }
        public string RelativeCode { get; set; }
    }
}
