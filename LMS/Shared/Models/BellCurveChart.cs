namespace LMS.Shared.Models
{
    public class BellCurveChart
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public Series Series { get; set; }
    }

    public class Series
    {
        public int[] Data { get; set; }
        public int PointsPossible { get; set; }
        public int AssignmentId { get; set; }
    }
}
