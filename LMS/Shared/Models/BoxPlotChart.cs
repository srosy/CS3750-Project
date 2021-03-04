namespace LMS.Shared.Models
{
    public class BoxPlotChart
    {
        public string Name { get; set; }
        public Series[] Series { get; set; }
    }

    public class Series
    {
        public string Name { get; set; }
        public SeriesData Data { get; set; }
        public int PointsPossible { get; set; }
        public int AssignmentId { get; set; }
    }

    public class SeriesData
    {
        public int low { get; set; }
        public int q1 { get; set; }
        public int median { get; set; }
        public int q3 { get; set; }
        public int high { get; set; }
    }
}
